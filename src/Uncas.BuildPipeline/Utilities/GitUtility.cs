﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Uncas.BuildPipeline.Utilities
{
    public class GitUtility : IGitUtility
    {
        #region IGitUtility Members

        public IEnumerable<string> GetBranchesMerged(
            string repository, string from, string to, params string[] excludeBranches)
        {
            string command = string.Format("log --merges {0}..{1} --oneline", from, to);
            string output = ExecuteGitCommand(repository, command);
            IEnumerable<string> mergeStatements = GetLines(output);
            IEnumerable<string> branchesMerged =
                mergeStatements.Select(ExtractMergeSource).Where(
                    x => !string.IsNullOrWhiteSpace(x)).Select(RemoveRemoteName).Distinct()
                    .OrderBy(x => x).Where(x => x != "origin");
            if (excludeBranches == null || excludeBranches.Length == 0)
                return branchesMerged;
            return branchesMerged.Where(x => !excludeBranches.Contains(x));
        }

        public IEnumerable<GitLog> GetLogs(
            string localMirror,
            string from,
            string to,
            string gitHubUrl,
            int limit = 1000,
            bool includeMerges = false)
        {
            string mergesCommand = includeMerges ? string.Empty : "--no-merges";
            string command =
                string.Format(
                    "log {0}..{1} -{2} --pretty=format:\"<entry><hash>%H</hash><author_name>%an</author_name><author_email>%ae</author_email><author_date>%ai</author_date><subject>%s</subject></entry>\" {3}",
                    from,
                    to,
                    limit,
                    mergesCommand);
            string output = ExecuteGitCommand(localMirror, command);
            string xml = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
<gitlog>
<entries>
  {0}
</entries>
</gitlog>", output);
            return Deserialize(xml, gitHubUrl);
        }

        public string GetShortLog(string repository, string from, string to)
        {
            string command = string.Format("shortlog {0}..{1} --no-merges", @from, to);
            return ExecuteGitCommand(repository, command);
        }

        public IEnumerable<string> GetChangedFiles(
            string repository, string @from, string to)
        {
            string command =
                string.Format(
                    "diff {0}..{1} --stat --stat-width=600 --stat-name-width=400",
                    @from,
                    to);
            string output = ExecuteGitCommand(repository, command);
            return
                GetLines(output).Where(l => l.Contains("|")).Select(
                    x => x.Split('|').First().Trim());
        }

        public bool ContainsCommit(string repository, string first, string second)
        {
            string command = string.Format("merge-base --is-ancestor {0} {1}",
                                           second,
                                           first);
            return ExecuteGitCommandAndGetResults(repository, command).ExitCode == 0;
        }

        public void Mirror(string sourceUrl, string mirrorsFolder, string mirrorName)
        {
            if (!Directory.Exists(mirrorsFolder))
                Directory.CreateDirectory(mirrorsFolder);
            string mirrorFolder = Path.Combine(mirrorsFolder, mirrorName);
            if (Directory.Exists(mirrorFolder))
            {
                const string command = "remote update";
                ExecuteGitCommand(mirrorFolder, command);
            }
            else
            {
                string command = string.Format("clone {0} {1} --mirror", sourceUrl, mirrorName);
                ExecuteGitCommand(mirrorsFolder, command);
            }
        }

        #endregion

        private static string SanatizeOutput(string output)
        {
            return output.Replace("&", "&amp;").Replace("<>", "&lt;&gt;");
        }

        public static IEnumerable<GitLog> Deserialize(string xml, string repo)
        {
            string cleanXml = SanatizeOutput(xml);
            var serializer = new XmlSerializer(typeof (GitLogXmlList));
            var gitLogXmlList =
                (GitLogXmlList) serializer.Deserialize(new StringReader(cleanXml));
            return
                gitLogXmlList.Entries.Select(
                    x =>
                    new GitLog
                        {
                            AuthorDate = DateTime.Parse(x.AuthorDate),
                            AuthorEmail = x.AuthorEmail,
                            AuthorName = x.AuthorName,
                            Revision = x.Hash,
                            Subject = x.Subject,
                            RevisionLink = GitHubLink.Revision(repo, x.Hash)
                        });
        }

        private static IEnumerable<string> GetLines(string output)
        {
            return output.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string RemoveRemoteName(string reference)
        {
            string[] parts = reference.Split('/');
            if (parts.Length < 2)
                return reference;
            return parts[1];
        }

        private static string ExtractMergeSource(string mergeStatement)
        {
            string[] parts = mergeStatement.Split('\'');
            if (parts.Length < 2)
                return string.Empty;
            return parts[1];
        }

        private string ExecuteGitCommand(string repository, string command)
        {
            return ExecuteGitCommandAndGetResults(repository, command).StandardOutput;
        }

        private ProcessResult ExecuteGitCommandAndGetResults(
            string repository, string command)
        {
            int exitCode;
            string standardOutput;
            using (Process gitProcess = GetGitProcess(repository, command))
            {
                gitProcess.Start();
                standardOutput = gitProcess.StandardOutput.ReadToEnd();
                gitProcess.WaitForExit();
                exitCode = gitProcess.ExitCode;
                gitProcess.Close();
            }

            return new ProcessResult {ExitCode = exitCode, StandardOutput = standardOutput};
        }

        private Process GetGitProcess(string repository, string command)
        {
            var startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = @"C:\Program Files (x86)\Git\bin\git.exe",
                    UseShellExecute = false,
                    Arguments = command,
                    WorkingDirectory = repository
                };
            return new Process {StartInfo = startInfo};
        }

        #region Nested type: GitLogXmlEntry

        public class GitLogXmlEntry
        {
            [XmlElement("author_email")]
            public string AuthorEmail { get; set; }

            [XmlElement("author_name")]
            public string AuthorName { get; set; }

            [XmlElement("author_date")]
            public string AuthorDate { get; set; }

            [XmlElement("hash")]
            public string Hash { get; set; }

            [XmlElement("subject")]
            public string Subject { get; set; }
        }

        #endregion

        #region Nested type: GitLogXmlList

        [XmlRoot("gitlog")]
        public class GitLogXmlList
        {
            [XmlArray("entries")]
            [XmlArrayItem("entry", typeof (GitLogXmlEntry))]
            public List<GitLogXmlEntry> Entries { get; set; }
        }

        #endregion

        #region Nested type: ProcessResult

        private class ProcessResult
        {
            public int ExitCode { get; set; }
            public string StandardOutput { get; set; }
        }

        #endregion
    }
}
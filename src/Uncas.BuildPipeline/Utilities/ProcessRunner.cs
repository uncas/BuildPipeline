using System;
using System.Diagnostics;

namespace Uncas.BuildPipeline.Utilities
{
    public static class ProcessRunner
    {
        public static ProcessResult ExecuteCommandAndGetResults(
            string workingDirectory,
            string fileName,
            string arguments, Action actionOnError)
        {
            int exitCode;
            string standardOutput;
            using (Process process = GetProcess(workingDirectory, fileName, arguments))
            {
                process.Start();
                standardOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                exitCode = process.ExitCode;
                if (!process.HasExited)
                {
                    process.Kill();
                    actionOnError();
                }

                process.Close();
            }

            return new ProcessResult
                {ExitCode = exitCode, StandardOutput = standardOutput};
        }

        private static Process GetProcess(
            string workingDirectory,
            string fileName,
            string arguments)
        {
            var startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = fileName,
                    UseShellExecute = false,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory
                };
            return new Process {StartInfo = startInfo};
        }
    }
}
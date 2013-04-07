using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Uncas.BuildPipeline.Utilities
{
    public static class ProcessRunner
    {
        /// <remarks>
        ///     See http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
        /// </remarks>
        public static ProcessResult ExecuteCommandAndGetResults(
            string workingDirectory,
            string fileName,
            string arguments,
            Action<string> actionOnError)
        {
            const int millisecondsTimeout = 1000*10;
            int exitCode;
            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var process = new Process())
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = workingDirectory;

                using (var outputWaitHandle = new AutoResetEvent(false))
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                                outputWaitHandle.Set();
                            else
                                output.AppendLine(e.Data);
                        };
                    process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                                errorWaitHandle.Set();
                            else
                                error.AppendLine(e.Data);
                        };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (process.WaitForExit(millisecondsTimeout) &&
                        outputWaitHandle.WaitOne(millisecondsTimeout) &&
                        errorWaitHandle.WaitOne(millisecondsTimeout))
                    {
                        exitCode = process.ExitCode;
                    }
                    else
                    {
                        exitCode = -1;
                    }
                }
            }

            string errorString = error.ToString();
            if (exitCode != 0 || !string.IsNullOrWhiteSpace(errorString))
            {
                // TODO: Consider throwing exception instead...
                string errorMessage = string.Format(
                    "Exit code {0}; error message: {1}",
                    exitCode,
                    errorString);
                actionOnError(errorMessage);
            }

            string standardOutput = output.ToString();

            return new ProcessResult
                {ExitCode = exitCode, StandardOutput = standardOutput};
        }
    }
}
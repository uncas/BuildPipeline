using System.Diagnostics;

namespace Uncas.BuildPipeline.Utilities
{
    public class PowershellUtility : IPowershellUtility
    {
        #region IPowershellUtility Members

        public void RunPowershell(string workingDirectory, string arguments)
        {
            const string powershellExe = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            var startInfo =
                new ProcessStartInfo(
                    powershellExe)
                    {
                        WorkingDirectory = workingDirectory,
                        Arguments = arguments
                    };
            Process.Start(startInfo);
        }

        #endregion
    }
}
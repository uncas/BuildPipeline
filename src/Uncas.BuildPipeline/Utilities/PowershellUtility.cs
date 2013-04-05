namespace Uncas.BuildPipeline.Utilities
{
    public class PowershellUtility : IPowershellUtility
    {
        private readonly ILogger _logger;

        public PowershellUtility(ILogger logger)
        {
            _logger = logger;
        }

        #region IPowershellUtility Members

        public void RunPowershell(string workingDirectory, string arguments)
        {
            ProcessRunner.ExecuteCommandAndGetResults(
                workingDirectory,
                @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                arguments,
                () => _logger.Error(
                    "Powershell was hanging with the arguments '{0}' in directory '{1}'.",
                    arguments,
                    workingDirectory));
        }

        #endregion
    }
}
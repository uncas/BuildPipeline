namespace Uncas.BuildPipeline.Utilities
{
    public interface IPowershellUtility
    {
        void RunPowershell(string workingDirectory, string arguments);
    }
}
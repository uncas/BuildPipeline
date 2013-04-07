using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    public class PowershellUtilityTests : WithBootstrapping<PowershellUtility>
    {
        [Test]
        public void RunPowershell_InvalidCommand()
        {
            Sut.RunPowershell(@"C:\Temp", "blablabla < !! lækælkqwåeoqåwoe");
        }

        [Test]
        public void RunPowershell_ValidCommand()
        {
            Sut.RunPowershell(@"C:\Temp", "Write-Host 2");
        }
    }
}
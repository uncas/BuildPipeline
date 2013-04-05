using NUnit.Framework;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    public class PowershellUtilityTests : WithBootstrapping<PowershellUtility>
    {
        [Test]
        public void RunPowershell()
        {
            Sut.RunPowershell(@"C:\Temp", "Write-Host 2");
        }
    }
}
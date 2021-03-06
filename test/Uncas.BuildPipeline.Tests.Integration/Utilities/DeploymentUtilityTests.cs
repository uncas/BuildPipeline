﻿using NUnit.Framework;
using Uncas.BuildPipeline.Models;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    [TestFixture]
    public class DeploymentUtilityTests : WithBootstrapping<IDeploymentUtility>
    {
        [Test]
        [Ignore]
        public void Deploy_ExistingPackage_IsDeployedAlright()
        {
            const string workingDirectory = @"C:\temp\ziptest";
            const string packagePath =
                @"C:\Builds\BuildPipeline\Artifacts\Unit\packages\Uncas.BuildPipeline-0.1.35.966.zip";

            Sut.Deploy(packagePath, workingDirectory, new Environment(), "Write-Host Hello");
        }
    }
}
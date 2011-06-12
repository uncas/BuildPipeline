﻿namespace Uncas.BuildPipeline.Tests.Integration.Utilities
{
    using NUnit.Framework;
    using Uncas.BuildPipeline.Models;
    using Uncas.BuildPipeline.Utilities;

    [TestFixture]
    public class DeploymentUtilityTests
    {
        [Test]
        [Ignore]
        public void Deploy_ExistingPackage_IsDeployedAlright()
        {
            var utility = new DeploymentUtility();
            const string workingDirectory = @"C:\temp\ziptest";
            const string packagePath =
                @"C:\Builds\BuildPipeline\Artifacts\Unit\packages\Uncas.BuildPipeline-0.1.35.966.zip";

            utility.Deploy(packagePath, workingDirectory, new Environment());
        }
    }
}
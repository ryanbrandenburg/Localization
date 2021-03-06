// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.IntegrationTesting;
using Microsoft.AspNetCore.Testing.xunit;
using Xunit;

namespace Microsoft.AspNetCore.Localization.FunctionalTests
{
    public class LocalizationSampleTest
    {
        private static readonly string _applicationPath = Path.Combine("samples", "LocalizationSample");

        [ConditionalFact]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public Task RunSite_WindowsOnly()
        {
            var testRunner = new TestRunner(_applicationPath);
            return testRunner.RunTestAndVerifyResponseHeading(
                RuntimeFlavor.Clr,
                RuntimeArchitecture.x64,
                "http://localhost:5080",
                "My/Resources",
                "fr-FR",
                "<h1>Bonjour</h1>");
        }
        [Fact]
        public Task RunSite_AnyOS()
        {
            var testRunner = new TestRunner(_applicationPath);
            return testRunner.RunTestAndVerifyResponseHeading(
                RuntimeFlavor.CoreClr,
                RuntimeArchitecture.x64,
                "http://localhost:5081/",
                "My/Resources",
                "fr-FR",
                "<h1>Bonjour</h1>");
        }
    }
}

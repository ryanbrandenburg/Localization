﻿using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Microsoft.Extensions.Localization
{
    public class POStringLocalizerTest
    {
        [Fact]
        [ReplaceCulture("en-US", "en-US")]
        public void GetString_Base()
        {
            // Arrange
            var localizer = CreatePOLocalizer("BaseFile");

            // Act
            var result = localizer["this is a multiline"];

            // Assert
            Assert.Equal("Multi line str", result.Value);
        }

        [Fact]
        [ReplaceCulture("en-US", "en-US")]
        public void GetString_WalksCultureTree()
        {
            // Arrange
            var localizer = CreatePOLocalizer("CultureFile");

            // Act
            var result = localizer["culture"];

            // Assert
            Assert.Equal("culture en", result.Value);
        }

        [Fact]
        public void GetAllStrings_IncludeParentCultures()
        {
            // Arrange
            var localizer = CreatePOLocalizer("CultureFile");

            // Act
            var result = localizer.GetAllStrings(includeParentCultures: true);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetAllString_ExcludeparentCultes()
        {
            // Arrange
            var localizer = CreatePOLocalizer("CultureFile");

            // Act
            var result = localizer.GetAllStrings(includeParentCultures: false);

            // Assert
            Assert.Equal(1, result.Count());
        }

        [Fact]
        [ReplaceCulture("fr-FR", "fr-FR")]
        public void WithCulture()
        {
            // Arrange
            IStringLocalizer localizer = CreatePOLocalizer("CultureFile");

            // Act
            localizer = localizer.WithCulture(new CultureInfo("en-US"));
            var result = localizer.GetAllStrings(includeParentCultures: true);

            // Assert
            Assert.Equal(2, result.Count());
        }

        private POStringLocalizer CreatePOLocalizer(string file)
        {
            var options = new LocalizationOptions();
            options.ResourcesPath = "POFiles";
            var localizationOptions = new Mock<IOptions<LocalizationOptions>>();
            localizationOptions.Setup(o => o.Value).Returns(options);

            return new POStringLocalizer(
                file,
                typeof(POStringLocalizerTest).GetTypeInfo().Assembly.GetName().Name,
                localizationOptions.Object);
        }
    }
}

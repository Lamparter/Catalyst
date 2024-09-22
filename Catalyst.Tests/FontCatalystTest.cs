using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catalyst;
using SkiaSharp;

namespace Catalyst.Tests
{
    [TestClass]
    public class FontCatalystTests
    {
        private readonly string _testOutputPath = Path.Combine(Path.GetTempPath(), "fontfile.ttf");

        public readonly List<string> svgFilePaths = new() { "..\\..\\..\\Resources\\TestResource-ED53.svg", "..\\..\\..\\Resources\\TestResource-ED54.svg", "..\\..\\..\\Resources\\TestResource-ED55.svg", "..\\..\\..\\Resources\\TestResource-ED56.svg", "..\\..\\..\\Resources\\TestResource-ED57.svg", "..\\..\\..\\Resources\\TestResource-ED58.svg", "..\\..\\..\\Resources\\TestResource-ED59.svg", "..\\..\\..\\Resources\\TestResource-ED5A.svg" };

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testOutputPath))
            {
                File.Delete(_testOutputPath);
            }
        }

        [TestMethod]
        public void ConvertSvgToFont_ValidInput_CreatesFontFile()
        {
            var fontCatalyst = new FontCatalyst();

            fontCatalyst.ConvertSvgToFont(svgFilePaths, _testOutputPath);

            Assert.IsTrue(File.Exists(_testOutputPath), "The font file was not created.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertSvgToFont_NullSvgFilePaths_ThrowsArgumentNullException()
        {
            var fontCatalyst = new FontCatalyst();

            fontCatalyst.ConvertSvgToFont(null, _testOutputPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertSvgToFont_EmptyOutputFontPath_ThrowsArgumentNullException()
        {
            var fontCatalyst = new FontCatalyst();

            fontCatalyst.ConvertSvgToFont(svgFilePaths, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ConvertSvgToFont_InvalidSvgFilePath_ThrowsFileNotFoundException()
        {
            // Arrange
            var svgFilePaths = new List<string> { "path/to/invalid/svg.svg" };
            var fontCatalyst = new FontCatalyst();

            fontCatalyst.ConvertSvgToFont(svgFilePaths, _testOutputPath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using SixLabors.Fonts;
using SkiaSharp;
using Svg.Skia;

namespace Catalyst
{
    public class FontCatalyst
    {
        public void ConvertSvgToFont(IEnumerable<string> svgFilePaths, string outputFontPath)
        {
            if (svgFilePaths == null)
                throw new ArgumentNullException(nameof(svgFilePaths));
            if (string.IsNullOrEmpty(outputFontPath))
                throw new ArgumentNullException(nameof(outputFontPath));

            var glyphs = new List<Glyph>();

            foreach (var svgFilePath in svgFilePaths)
            {
                if (!File.Exists(svgFilePath))
                    throw new FileNotFoundException($"SVG file not found: {svgFilePath}");

                using var svgStream = File.OpenRead(svgFilePath);
                var svgDocument = new SKSvg();
                var picture = svgDocument.Load(svgStream) ?? throw new InvalidOperationException($"Failed to load SVG file: {svgFilePath}");

                // Assuming each SVG represents a single glyph
                var glyph = new Glyph(picture.CullRect.Width, picture.CullRect.Height, picture);
                glyphs.Add(glyph);
            }

            SaveFont(glyphs, outputFontPath);
        }

        private void SaveFont(List<Glyph> glyphs, string outputFontPath)
        {
            using var fontStream = File.Create(outputFontPath);
            using var writer = new BinaryWriter(fontStream);

            // Write TTF header
            TTFWriter.WriteHeader(writer, glyphs.Count);

            // Write table directory
            TTFWriter.WriteTableDirectory(writer, glyphs.Count);

            // Write glyph data
            TTFWriter.WriteGlyphData(writer, glyphs);

            // Write additional tables
            TTFWriter.WriteHeadTable(writer);
            TTFWriter.WriteHheaTable(writer);
            TTFWriter.WriteMaxpTable(writer, glyphs.Count);
            TTFWriter.WriteCmapTable(writer);
            TTFWriter.WriteNameTable(writer);
            TTFWriter.WriteOS2Table(writer);
            TTFWriter.WritePostTable(writer);
            TTFWriter.WriteLocaTable(writer, glyphs.Count);
        }
    }
}

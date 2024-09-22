using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;

namespace Catalyst
{
    public static class TTFWriter
    {
        public static void WriteHeader(BinaryWriter writer, int numGlyphs)
        {
            // Write the TTF header
            writer.Write((uint)0x00010000); // sfnt version
            writer.Write((ushort)9); // numTables
            writer.Write((ushort)(16 * (int)Math.Floor(Math.Log(9) / Math.Log(2)))); // searchRange
            writer.Write((ushort)(int)Math.Log(9) / Math.Log(2)); // entrySelector
            writer.Write((ushort)(9 * 16 - (16 * (int)Math.Floor(Math.Log(9) / Math.Log(2))))); // rangeShift
        }

        public static void WriteTableDirectory(BinaryWriter writer, int numGlyphs)
        {
            // Write the table directory
            WriteTableRecord(writer, "cmap", 0, 0);
            WriteTableRecord(writer, "glyf", 0, 0);
            WriteTableRecord(writer, "head", 0, 0);
            WriteTableRecord(writer, "hhea", 0, 0);
            WriteTableRecord(writer, "maxp", 0, 0);
            WriteTableRecord(writer, "name", 0, 0);
            WriteTableRecord(writer, "OS/2", 0, 0);
            WriteTableRecord(writer, "post", 0, 0);
            WriteTableRecord(writer, "loca", 0, 0);
        }

        public static void WriteTableRecord(BinaryWriter writer, string tag, uint offset, uint length)
        {
            writer.Write(System.Text.Encoding.ASCII.GetBytes(tag));
            writer.Write((uint)0); // checksum
            writer.Write(offset);
            writer.Write(length);
        }

        public static void WriteGlyphData(BinaryWriter writer, List<Glyph> glyphs)
        {
            // Write glyph data
            foreach (var glyph in glyphs)
            {
                var bitmap = RenderGlyphToBitmap(glyph);
                var data = bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                writer.Write(data.Length);
                writer.Write(data);
            }
        }

        public static void WriteHeadTable(BinaryWriter writer)
        {
            // Write the 'head' table (simplified)
            writer.Write((uint)0x00010000); // version
            writer.Write((uint)0x00010000); // fontRevision
            writer.Write((uint)0); // checkSumAdjustment
            writer.Write((uint)0x5F0F3CF5); // magicNumber
            writer.Write((ushort)0x0003); // flags
            writer.Write((ushort)2048); // unitsPerEm
            writer.Write((long)0); // created
            writer.Write((long)0); // modified
            writer.Write((short)0); // xMin
            writer.Write((short)0); // yMin
            writer.Write((short)0); // xMax
            writer.Write((short)0); // yMax
            writer.Write((ushort)0); // macStyle
            writer.Write((ushort)0); // lowestRecPPEM
            writer.Write((short)2); // fontDirectionHint
            writer.Write((short)0); // indexToLocFormat
            writer.Write((short)0); // glyphDataFormat
        }

        public static void WriteHheaTable(BinaryWriter writer)
        {
            // Write the 'hhea' table (simplified)
            writer.Write((uint)0x00010000); // version
            writer.Write((short)0); // ascender
            writer.Write((short)0); // descender
            writer.Write((short)0); // lineGap
            writer.Write((ushort)0); // advanceWidthMax
            writer.Write((short)0); // minLeftSideBearing
            writer.Write((short)0); // minRightSideBearing
            writer.Write((short)0); // xMaxExtent
            writer.Write((short)0); // caretSlopeRise
            writer.Write((short)0); // caretSlopeRun
            writer.Write((short)0); // caretOffset
            writer.Write((short)0); // reserved1
            writer.Write((short)0); // reserved2
            writer.Write((short)0); // reserved3
            writer.Write((short)0); // reserved4
            writer.Write((short)0); // reserved5
            writer.Write((short)0); // metricDataFormat
            writer.Write((ushort)1); // numberOfHMetrics
        }

        public static void WriteMaxpTable(BinaryWriter writer, int numGlyphs)
        {
            // Write the 'maxp' table (simplified)
            writer.Write((uint)0x00010000); // version
            writer.Write((ushort)numGlyphs); // numGlyphs
        }

        public static void WriteCmapTable(BinaryWriter writer)
        {
            // Write the 'cmap' table (simplified)
            writer.Write((ushort)0); // version
            writer.Write((ushort)1); // numTables
            writer.Write((ushort)3); // platformID
            writer.Write((ushort)1); // encodingID
            writer.Write((uint)12); // offset
            writer.Write((ushort)4); // format
            writer.Write((ushort)0); // length
            writer.Write((ushort)0); // language
            writer.Write((ushort)0); // segCountX2
            writer.Write((ushort)0); // searchRange
            writer.Write((ushort)0); // entrySelector
            writer.Write((ushort)0); // rangeShift
        }

        public static void WriteNameTable(BinaryWriter writer)
        {
            // Write the 'name' table (simplified)
            writer.Write((ushort)0); // format
            writer.Write((ushort)1); // count
            writer.Write((ushort)6); // stringOffset
            writer.Write((ushort)1); // platformID
            writer.Write((ushort)0); // encodingID
            writer.Write((ushort)0); // languageID
            writer.Write((ushort)1); // nameID
            writer.Write((ushort)0); // length
            writer.Write((ushort)0); // offset
        }

        public static void WriteOS2Table(BinaryWriter writer)
        {
            // Write the 'OS/2' table (simplified)
            writer.Write((ushort)0); // version
            writer.Write((short)0); // xAvgCharWidth
            writer.Write((ushort)0); // usWeightClass
            writer.Write((ushort)0); // usWidthClass
            writer.Write((short)0); // fsType
            writer.Write((short)0); // ySubscriptXSize
            writer.Write((short)0); // ySubscriptYSize
            writer.Write((short)0); // ySubscriptXOffset
            writer.Write((short)0); // ySubscriptYOffset
            writer.Write((short)0); // ySuperscriptXSize
            writer.Write((short)0); // ySuperscriptYSize
            writer.Write((short)0); // ySuperscriptXOffset
            writer.Write((short)0); // ySuperscriptYOffset
            writer.Write((short)0); // yStrikeoutSize
            writer.Write((short)0); // yStrikeoutPosition
            writer.Write((short)0); // sFamilyClass
            writer.Write(new byte[10]); // panose
            writer.Write((uint)0); // ulUnicodeRange1
            writer.Write((uint)0); // ulUnicodeRange2
            writer.Write((uint)0); // ulUnicodeRange3
            writer.Write((uint)0); // ulUnicodeRange4
            writer.Write((uint)0); // achVendID
            writer.Write((ushort)0); // fsSelection
            writer.Write((ushort)0); // usFirstCharIndex
            writer.Write((ushort)0); // usLastCharIndex
            writer.Write((short)0); // sTypoAscender
            writer.Write((short)0); // sTypoDescender
            writer.Write((short)0); // sTypoLineGap
            writer.Write((ushort)0); // usWinAscent
            writer.Write((ushort)0); // usWinDescent
            writer.Write((uint)0); // ulCodePageRange1
            writer.Write((uint)0); // ulCodePageRange2
            writer.Write((short)0); // sxHeight
            writer.Write((short)0); // sCapHeight
            writer.Write((ushort)0); // usDefaultChar
            writer.Write((ushort)0); // usBreakChar
            writer.Write((ushort)0); // usMaxContext
        }

        public static void WritePostTable(BinaryWriter writer)
        {
            // Write the 'post' table (simplified)
            writer.Write((uint)0x00030000); // version
            writer.Write((uint)0); // italicAngle
            writer.Write((short)0); // underlinePosition
            writer.Write((short)0); // underlineThickness
            writer.Write((uint)0); // isFixedPitch
            writer.Write((uint)0); // minMemType42
            writer.Write((uint)0); // maxMemType42
            writer.Write((uint)0); // minMemType1
            writer.Write((uint)0); // maxMemType1
        }

        public static void WriteLocaTable(BinaryWriter writer, int numGlyphs)
        {
            // Write the 'loca' table (simplified)
            for (int i = 0; i <= numGlyphs; i++)
            {
                writer.Write((ushort)0); // glyph offset
            }
        }

        private static SKBitmap RenderGlyphToBitmap(Glyph glyph)
        {
            using var surface = SKSurface.Create(new SKImageInfo((int)glyph.Width, (int)glyph.Height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(glyph.Picture);
            var image = surface.Snapshot();
            return SKBitmap.FromImage(image);
        }
    }
}

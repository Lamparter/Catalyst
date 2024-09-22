using SkiaSharp;

namespace Catalyst
{
    public class Glyph
    {
        public float Width { get; }
        public float Height { get; }
        public SKPicture Picture { get; }

        public Glyph(float width, float height, SKPicture picture)
        {
            Width = width;
            Height = height;
            Picture = picture;
        }
    }
}

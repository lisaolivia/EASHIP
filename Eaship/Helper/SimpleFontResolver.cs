using PdfSharp.Fonts;
using System.IO;

public class SimpleFontResolver : IFontResolver
{
    private readonly byte[] _fontData;

    public SimpleFontResolver()
    {
        var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "segoeui.ttf");
        _fontData = File.ReadAllBytes(fontPath);
    }

    public byte[] GetFont(string faceName)
    {
        return _fontData;
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo("DefaultFont");
    }
}

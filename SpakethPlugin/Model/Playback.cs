namespace SpakethPlugin.Model;

public class Playback
{
    public string? Name;
    public string Text = "";
    public uint Duration;
    public uint? Image;

    public string GetName(string fallback)
    {
        return Name ?? fallback;
    }

    public uint GetImage(uint fallback)
    {
        return Image ?? fallback;
    }
}

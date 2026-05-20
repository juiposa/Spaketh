using Newtonsoft.Json;

namespace Spaketh.Model;

public class Voiceline
{
    [JsonProperty(PropertyName = "file")]
    public string File = "";
    [JsonProperty(PropertyName = "duration")]
    public uint Duration = 0;
    [JsonProperty(PropertyName = "name")]
    public string Name = "";
    [JsonProperty(PropertyName = "transcript")]
    public uint Transcript = 0;

    public string Text = "";

}

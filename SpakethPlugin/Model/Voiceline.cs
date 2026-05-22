using Newtonsoft.Json;
using SpakethPlugin.Model;

namespace Spaketh.Model;

public class Voiceline
{
    [JsonProperty(PropertyName = "sound_file")]
    public string SoundFile = "";

    [JsonProperty(PropertyName = "image_id")]
    public uint? ImageId;
    
    [JsonProperty(PropertyName = "duration")]
    public uint Duration = 0;
    
    [JsonProperty(PropertyName = "name")]
    public IntlText? Name;
    
    [JsonProperty(PropertyName = "intercept")]
    public GameText Intercept = new GameText();
    
    [JsonProperty(PropertyName = "text")]
    public IntlText? Text;
    
    [JsonProperty(PropertyName = "game_text")]
    public GameText? GameText;

}

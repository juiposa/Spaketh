using Newtonsoft.Json;

namespace SpakethPlugin.Model;

public class GameText
{
    [JsonProperty(PropertyName = "battletext_id")]
    public uint? BattletextId;

    [JsonProperty(PropertyName = "textbox_line")]
    public string? TextboxLine;
}

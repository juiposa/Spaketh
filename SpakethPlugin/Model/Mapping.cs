using Newtonsoft.Json;

namespace Spaketh.Model;

public class Mapping
{
    [JsonProperty(propertyName: "original")]
    public uint Original = 0;
    [JsonProperty(propertyName: "replacement")]
    public uint Replacement = 0;
}

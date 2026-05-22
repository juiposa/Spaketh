using System;
using Newtonsoft.Json;
using SpakethPlugin.Handlers;

namespace SpakethPlugin.Model;

public class IntlText
{
    [JsonProperty(PropertyName = "en")]
    private string english = "";
    
    [JsonProperty(PropertyName = "ja")]
    private string japanese = "";
    
    [JsonProperty(PropertyName = "de")]
    private string german = "";
    
    [JsonProperty(PropertyName = "fr")]
    private string french = "";
    
    public string Get()
    {
        switch (Language.GetTextLanguage())
        {
            case "en":
                return english;
            case "ja":
                return japanese;
            case "de":
                return german;
            case "fr":
                return french;
        }
        throw new Exception("Language not found");
    }
}

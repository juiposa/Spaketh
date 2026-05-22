using System;
using Newtonsoft.Json;
using Spaketh.Model;

namespace SpakethPlugin.Model;

public class Playscript
{
    [JsonProperty(propertyName: "id")]
    public Guid Id;
    
    [JsonProperty(propertyName: "notes")]
    public String Notes = "";

    [JsonProperty(propertyName: "name")]
    public IntlText Name = new IntlText();
    
    [JsonProperty(propertyName: "supported_languages")]
    public string[] SupportedLanguages= [];

    [JsonProperty(propertyName: "instance_ids")]
    public uint[] InstanceIds = [];

    [JsonProperty(propertyName: "lines")]
    public Voiceline[] Lines = [];
}

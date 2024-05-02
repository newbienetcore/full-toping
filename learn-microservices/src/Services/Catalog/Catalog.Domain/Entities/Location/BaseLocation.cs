using Newtonsoft.Json;
using SharedKernel.Domain;

public class BaseLocation : CoreEntity
{
    [System.ComponentModel.DataAnnotations.Key]
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public LocationType Type { get; set; }
    
    [JsonIgnore]
    public bool IsDeleted { get; set; }
}
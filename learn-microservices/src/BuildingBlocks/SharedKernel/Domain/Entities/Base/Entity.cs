using Newtonsoft.Json;

namespace SharedKernel.Domain;

public class Entity<TKey> : IEntity<TKey>
{
    [System.ComponentModel.DataAnnotations.Key]
    public TKey Id { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}

public class Entity : Entity<Guid>
{
    
}
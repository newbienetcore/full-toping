namespace SharedKernel.Domain;

public interface IEntity<TKey>
{
    TKey Id { get; set; }

    bool IsDeleted { get; set; }
}
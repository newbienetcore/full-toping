namespace SharedKernel.Domain;

public interface IDateTracking
{
    DateTime CreatedDate { get; set; }
    
    DateTime? LastModifiedDate { get; set; }
}
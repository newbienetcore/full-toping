namespace SharedKernel.Domain;

public interface IUserTracking
{
    Guid CreatedBy { get; set; }

    Guid? LastModifiedBy { get; set; }
}
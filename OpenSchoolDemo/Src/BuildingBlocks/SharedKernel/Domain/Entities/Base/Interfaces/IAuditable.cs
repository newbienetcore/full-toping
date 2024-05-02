namespace SharedKernel.Domain;

public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete
{
    
}
namespace SharedKernel.Domain
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }

        Guid CreatedBy { get; set; }

        DateTime? LastModifiedDate { get; set; }

        Guid? LastModifiedBy { get; set; }

        DateTime? DeletedDate { get; set; }

        Guid? DeletedBy { get; set; }
    }
}

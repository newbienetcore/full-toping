using Contracts.Domains;

namespace Catalog.API.Entities;

public class Brand : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    
    #region [REFERENCE PROPERTIES]
   
    public ICollection<Product> Products { get; set; }
    
    #endregion [REFERENCE PROPERTIES]
}
using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.Category)]
public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Alias  { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public string FileName { get; set; }
    public int OrderNumber { get; set; }
    public bool Status { get; set; }
    public string Path { get; set; }
    
    #region Relationships

    public Guid? ParentId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Category Parent { get; set; }
    
    public ICollection<ProductCategory>? ProductCategories { get; set; }
    
    public ICollection<CategoryDiscount> CategoryDiscounts { get; set; }
    
    #endregion
}
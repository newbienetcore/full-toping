using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.Domain;

namespace Catalog.Api.Entities;

public class Product : EntityAuditBase
{
    [Required]
    [Column(TypeName = "varchar(50)")]
    public string No { get; set; }
    
    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string Name { get; set; }
    
    [Column(TypeName = "nvarchar(250)")]
    public string Summary { get; set; }
    
    [Column(TypeName = "text")]
    public string Description { get; set; }
    
    [Column(TypeName = "decimal(12,2)")]
    public decimal Price { get; set; }
}
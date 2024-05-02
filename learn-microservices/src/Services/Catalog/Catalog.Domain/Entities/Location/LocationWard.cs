using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;

namespace Catalog.Domain.Entities;

[Table(TableName.Wards)]
public class LocationWard : BaseLocation
{
    #region Relationships
    
    public long DistrictId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual LocationDistrict District { get; set; }
    
    #endregion
}
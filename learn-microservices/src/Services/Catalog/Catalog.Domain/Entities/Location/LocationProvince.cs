using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;

namespace Catalog.Domain.Entities;

[Table(TableName.Provinces)]
public class LocationProvince : BaseLocation
{
    #region Relationships
    

    #endregion
    
    #region Navigations
    
    public ICollection<LocationDistrict> Districts { get; set; }
    
    #endregion
}
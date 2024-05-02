using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;

namespace Catalog.Domain.Entities;

[Table(TableName.Districts)]
public class LocationDistrict : BaseLocation
{
    #region Relationships

    public long ProvinceId { get; set; }

    #endregion

    #region Navigations

    public virtual LocationProvince Province { get; set; }

    public ICollection<LocationWard> Wards { get; set; }

    #endregion
}
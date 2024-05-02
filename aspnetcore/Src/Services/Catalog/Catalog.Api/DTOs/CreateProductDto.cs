using System.ComponentModel.DataAnnotations;
using Catalog.Api.DTOs;

namespace Catalog.Api.Dtos;

public class CreateProductDto : CreateOrUpdateProductDto
{
    [Required]
    public string No { get; set; }
}
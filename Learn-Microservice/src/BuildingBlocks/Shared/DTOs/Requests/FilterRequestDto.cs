namespace Shared.DTOs.Requests;

public abstract class FilterRequestDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Keyword { get; set; } = string.Empty;
}
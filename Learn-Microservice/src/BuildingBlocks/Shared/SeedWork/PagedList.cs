namespace Shared.SeedWork;

public class PagedList<T> : List<T>
{
    private MetaData _MetaData { get;}

    public PagedList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
    {
        _MetaData = new MetaData()
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
        AddRange(items);
    }
    
    public MetaData GetMetaData() => _MetaData;
}
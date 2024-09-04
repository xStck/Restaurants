namespace Restaurants.Application.Common;

public class PageResult<T>
{
    public PageResult(IEnumerable<T> items, int totalItemsCount, int pageSize, int pageNubmer)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        ItemsFrom = pageSize * (pageNubmer - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }

    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    public IEnumerable<T> Items { get; set; }
}
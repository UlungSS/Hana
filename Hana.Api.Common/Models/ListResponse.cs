namespace Hana.Models;


public class ListResponse<T>
{
    public ListResponse()
    {
    }
    
    public ListResponse(ICollection<T> items)
    {
        Items = items;
        Count = items.Count;
    }

    public ICollection<T> Items { get; set; } = default!;
    
    public long Count { get; set; }
}
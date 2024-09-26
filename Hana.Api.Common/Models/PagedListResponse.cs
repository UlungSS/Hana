using Hana.Common.Models;

namespace Hana.Models;

public record PagedListResponse<T>: LinkedResource
{
    public PagedListResponse()
    {
    }
    
    public PagedListResponse(Page<T> page)
    {
        Items = page.Items;
        TotalCount = page.TotalCount;
    }

    public ICollection<T> Items { get; set; } = default!;
    public long TotalCount { get; set; }
}
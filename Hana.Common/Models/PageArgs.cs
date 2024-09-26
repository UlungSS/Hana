namespace Hana.Common.Models;

public record PageArgs
{
    public static PageArgs All => new() { Offset = null, Limit = null };

    public static PageArgs FromPage(int? page, int? pageSize) => new() { Offset = page * pageSize, Limit = pageSize };

    public static PageArgs FromRange(int? offset, int? limit) => new() { Offset = offset, Limit = limit };

    public static PageArgs From(int? page, int? pageSize, int? offset, int? limit)
    {
        if (page != null && pageSize != null)
            return FromPage(page, pageSize);

        if (offset != null && limit != null)
            return FromRange(offset, limit);

        return FromPage(0, 100);
    }

    public int? Offset { get; set; }

    public int? Limit { get; set; }

    public int? Page => Offset.HasValue && Limit.HasValue ? Offset / Limit : null;

    public int? PageSize => Limit;

    public PageArgs Next() => this with { Offset = Page + 1 };
}
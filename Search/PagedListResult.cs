namespace WebAPI5.Search
{
    public class PagedListResult<TEntity>
    {
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public long Count { get; set; }
        public IEnumerable<TEntity> Entities { get; set; } = Enumerable.Empty<TEntity>();
    }
}

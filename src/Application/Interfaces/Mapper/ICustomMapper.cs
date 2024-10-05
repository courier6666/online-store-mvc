using Store.Domain.PagedLists;

namespace Store.Application.Interfaces.Mapper
{
    public interface ICustomMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        void MapToExisting<TSource, TDestination>(TSource source, ref TDestination destination);
        IEnumerable<TDestination> MapEnumerable<TSource, TDestination>(IEnumerable<TSource> source);
        PagedList<TDestination> MapPagedList<TSource, TDestination>(PagedList<TSource> source);
    }
}

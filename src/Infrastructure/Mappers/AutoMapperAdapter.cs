using AutoMapper;
using Store.Application.Interfaces.Mapper;
using Store.Domain.PagedLists;

namespace Store.Infrastructure.Mappers
{
    /// <summary>
    /// Used for adapting AutoMapper to ICustomMapper interface.
    /// </summary>
    public class AutoMapperAdapter : ICustomMapper
    {
        /// <summary>
        /// AutoMapper IMapper interface.
        /// </summary>
        private readonly IMapper _mapper;
        public AutoMapperAdapter(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Maps values of properties from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">Mapped from</typeparam>
        /// <typeparam name="TDestination">Mapped to</typeparam>
        /// <param name="source">Instance with values to map from</param>
        /// <returns>Instance of type <typeparamref name="TDestination"/></returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// Maps enumerable with <typeparamref name="TSource"/> to enumerable with <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">Mapped from</typeparam>
        /// <typeparam name="TDestination">Mapped to</typeparam>
        /// <param name="source">Instance with values to map from</param>
        /// <returns>IEnumerable of type <typeparamref name="TDestination"/></returns>
        public IEnumerable<TDestination> MapEnumerable<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return source.Select(s => _mapper.Map<TSource, TDestination>(s));
        }
        /// <summary>
        /// Maps paged list with <typeparamref name="TSource"/> to paged list with <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TSource">Mapped from</typeparam>
        /// <typeparam name="TDestination">Mapped to</typeparam>
        /// <param name="source">Instance with values to map from</param>
        /// <returns>PagedList of type <typeparamref name="TDestination"/></returns>
        public PagedList<TDestination> MapPagedList<TSource, TDestination>(PagedList<TSource> source)
        {
            return new PagedList<TDestination>(source.Select(s => _mapper.Map<TSource, TDestination>(s)).ToList(),
                source.TotalCount,
                source.Page,
                source.PageSize);
        }
        /// <summary>
        /// Maps values of properties from <typeparamref name="TSource"/> to existing instance of type <typeparamref name="TDestination"/>
        /// </summary>
        /// <typeparam name="TSource">Mapped from</typeparam>
        /// <typeparam name="TDestination">Mapped to</typeparam>
        /// <param name="source">Instance with values to map from</param>
        /// <param name="destination">Existing instance of <typeparamref name="TDestination"/> to map to.</param>
        public void MapToExisting<TSource, TDestination>(TSource source, ref TDestination destination)
        {
            _mapper.Map(source, destination);
        }
    }
}

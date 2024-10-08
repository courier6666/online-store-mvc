﻿using Microsoft.EntityFrameworkCore;
using Store.Domain.PagedLists;

namespace Store.Persistence.Main.PagedListCreatorNm
{
    public static class PagedListCreator
    {
        public static async Task<PagedList<T>> CreateAsync<T>(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}

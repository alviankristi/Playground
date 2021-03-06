﻿using System.Linq;

namespace PG.Common.Extensions
{
    public static class Queryable
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            int totalCount = query.Count();
            IQueryable<T> collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new PagedList<T>(collection, pageIndex, pageSize, totalCount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Manager
{
    public static class EFExtensions
    {
        public static IQueryable<T> SetPage<T>(this IQueryable<T> query, PageFilter page)
        {
            if (page == null || page.PageSize == int.MaxValue) return query;

            if (page.RecordCount == 0)
            {
                page.RecordCount = query.Count();
            }
            return query.Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize);
        }

        public static IQueryable<T> GetCheckBaseQuery<T>(this IQueryable<T> query, CheckLogFilter filter) where T : VCheckBase
        {
            if (filter.CheckType.HasValue)
            {
                query = query.Where(e => e.CheckType == filter.CheckType.Value);
            }

            if (filter.InfoID.HasValue && filter.InfoID.Value > 0)
            {
                query = query.Where(e => e.InfoID == filter.InfoID.Value);
            }

            if (filter.HasCheck.HasValue)
            {
                if (filter.HasCheck.Value)
                {
                    query = query.Where(e => e.Result.HasValue);
                }
                else
                {
                    query = query.Where(e => e.Result == null);
                }
            }

            if (filter.UserID.HasValue && filter.UserID.Value > 0)
            {
                query = query.Where(e => e.UserID == filter.UserID.Value);
            }

            if (filter.Result.HasValue)
            {
                query = query.Where(e => e.Result == filter.Result.Value);
            }
            return query;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAPI_Biblos1.DTOS;


namespace WAPI_Biblos1.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina).
                Take(paginacionDTO.RecordsPorPagina);
        }
    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.DTOS
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private int cantidadMaximaRegistrosPorPagina = 50;
        public int RecordsPorPagina
        {
            get 
            { 
                return recordsPorPagina; 
            }
            set
            {
                recordsPorPagina = value > cantidadMaximaRegistrosPorPagina ? cantidadMaximaRegistrosPorPagina : value;
            }
        }

    }
}

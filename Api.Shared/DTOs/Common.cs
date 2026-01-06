using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs
{

    /// <summary>
    /// DTO para parámetros de paginación
    /// </summary>
    public class PaginationParamsDTO
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        /// <summary>
        /// Calcula cuántos registros saltar
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Cantidad de registros a tomar
        /// </summary>
        public int Take => PageSize;
    }
}

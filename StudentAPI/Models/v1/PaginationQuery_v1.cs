using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Pagination
{
    /// <summary>
    /// This object represent pagination info
    /// </summary>
    public class PaginationQuery_v1
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

    }
}

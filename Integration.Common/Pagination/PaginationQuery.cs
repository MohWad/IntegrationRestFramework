using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Pagination
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 5;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

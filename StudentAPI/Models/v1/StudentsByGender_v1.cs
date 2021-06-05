using Integration.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Models.v1
{
    public class StudentsByGender_v1
    {
        public string Gender { get; set; }
        public PaginationQuery_v1 Pagination { get; set; }
    }
}

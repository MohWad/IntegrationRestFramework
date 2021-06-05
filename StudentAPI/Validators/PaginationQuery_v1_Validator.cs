using FluentValidation;
using Integration.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Validators
{
    public class PaginationQuery_v1_Validator : AbstractValidator<PaginationQuery_v1>
    {
        public PaginationQuery_v1_Validator()
        {
            this.CascadeMode = CascadeMode.Stop;

            RuleFor(s => s.PageNumber).NotEmpty().WithMessage("101-PageNumber field is required")
                                      .NotEqual(0).WithMessage("101-PageNumber field is required");
            RuleFor(s => s.PageSize).NotEmpty().WithMessage("101-PageSize field is required")
                                    .NotEqual(0).WithMessage("101-PageSize field is required");
        }
    }
}

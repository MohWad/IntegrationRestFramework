using FluentValidation;
using StudentAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Validators
{
    public class GetStudentsByGenderRequestValidator : AbstractValidator<StudentsByGender_v1>
    {
        public GetStudentsByGenderRequestValidator()
        {
            RuleFor(s => s.Gender).NotEmpty().WithMessage("101-Gender field is required");
            //.Must(ValidateClientOwnesEntity).WithMessage("103-Invalid entity type")
        }

        //private bool ValidateClientOwnesEntity(string entity)
        //{
        //    var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

        //    return _repo.ValidateClientOwnesEntity(username, entity);
        //}
    }
}

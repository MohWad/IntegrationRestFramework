using FluentValidation;
using SampleAPI.Services;
using StudentAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Validators
{
    public class NewStudent_v1_Validator : AbstractValidator<NewStudent_v1>
    {
        private readonly IStudentService _studentService;

        public NewStudent_v1_Validator(IStudentService studentService)
        {
            _studentService = studentService;

            // this option will stop FluentValidator from other validators if the first one failed and will ensure that only one validation
            // message will return per propety at a time
            this.CascadeMode = CascadeMode.Stop;

            RuleFor(s => s.NationalId).NotEmpty().WithMessage("101-NationalId field is required")
                .Must(ValidateNationalId).WithMessage("105-NationalId is invalid");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("102-FirstName field is required");
            RuleFor(s => s.FamilyName).NotEmpty().WithMessage("103-FamilyName field is required");
            RuleFor(s => s.Gender).NotEmpty().WithMessage("104-Gender field is required");
        }

        private bool ValidateNationalId(string nationalId)
        {
           return  _studentService.CheckStudentNationalId(nationalId);
        }
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Dtos.StudentDtos
{
    public class StudentCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int GroupId { get; set; }
        public IFormFile File { get; set; }
    }

    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().MaximumLength(35).MinimumLength(5);

            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(100).MinimumLength(5);

            RuleFor(x => x.BirthDate.Date)
                .LessThanOrEqualTo(DateTime.Now.AddYears(-15).Date)
                    .WithMessage("Student's age must be greator or equal than 15");

        }
    }
}

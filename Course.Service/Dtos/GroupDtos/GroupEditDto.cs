using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Dtos.GroupDtos
{
    public class GroupEditDto
    {
        public string No { get; set; }

        public byte Limit { get; set; }
    }

    public class GroupEditDtoValidator : AbstractValidator<GroupEditDto>
    {
        public GroupEditDtoValidator()
        {
            RuleFor(x => x.No).NotEmpty().MaximumLength(5).MinimumLength(4);
            RuleFor(x => (int)x.Limit).NotNull().InclusiveBetween(5, 18);
        }
    }
}

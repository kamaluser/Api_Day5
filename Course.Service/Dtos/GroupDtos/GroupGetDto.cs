using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Dtos.GroupDtos
{
    public class GroupGetDto
    {
        public int Id { get; set; }
        public string No { get; set; }
        public byte Limit { get; set; }
    }
}

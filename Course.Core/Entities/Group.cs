using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Entities
{
    public class Group:AuditEntity
    {
        public string? No { get; set; }
        public byte Limit { get; set; }
        public List<Student> Students { get; set; }
    }
}

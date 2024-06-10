using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Entities
{
    public class Student:AuditEntity
    {
        public int GroupId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Group Group { get; set; }
        public string Photo { get; set; }
    }
}

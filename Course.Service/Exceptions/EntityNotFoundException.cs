using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        public EntityNotFoundException()
        {
            
        }
    }
}

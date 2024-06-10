using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Exceptions
{
    public class GroupLimitException:Exception
    {
        public GroupLimitException(string message) : base(message)
        {

        }
        public GroupLimitException()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class IllegalOperationException : Exception
    {
        public IllegalOperationException()
        {
        }

        public IllegalOperationException(string message) : base(message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Exceptions
{
    public class NullIdException : Exception
    {
        public NullIdException() : base()
        {
        }
        public NullIdException(string message) : base(message)
        {
        }
    }
}

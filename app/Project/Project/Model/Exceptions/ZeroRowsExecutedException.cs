using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Exceptions
{
    public class ZeroRowsExecutedException : Exception
    {
        public ZeroRowsExecutedException() : base()
        {
        }
        public ZeroRowsExecutedException(string message) : base(message)
        {
        }
    }
}

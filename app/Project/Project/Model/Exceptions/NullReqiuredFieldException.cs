using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Exceptions
{
    public class NullReqiuredFieldException : Exception
    {
        public string Field { get; private set; } = "";
        public NullReqiuredFieldException() : base()
        {
        }
        public NullReqiuredFieldException(string field) : base()
        {
            Field = field;
        }
        public NullReqiuredFieldException(string field, string message) : base(message)
        {
            Field = field;
        }
    }
}

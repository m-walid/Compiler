using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class InvalidExpectedToken : Exception
    {
        private string msg;
        public InvalidExpectedToken(string s)
        {
            msg = s;

        }
        public override string Message
        {
            get { return msg; }
        }
    }
}

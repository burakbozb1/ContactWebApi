using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Exceptions
{
    public class ServerSideException : Exception
    {
        public ServerSideException(string message) : base(message)
        {

        }
    }
}

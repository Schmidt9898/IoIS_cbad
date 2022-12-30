using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Common
{
    public enum RequestState
    {
        Successful = 0,
        Unauthorized = 1,
        Error = 2,
        NotFound = 3,
        AlreadyExists = 4,
    }
}

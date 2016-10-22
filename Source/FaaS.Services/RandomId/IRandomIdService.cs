using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Services.RandomId
{
    interface IRandomIdService
    {
        string GenerateRandomString(int entropyBits);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.HashService
{
    public interface IHashService
    {
        string ConvertToHash(string rawData);
    }
}

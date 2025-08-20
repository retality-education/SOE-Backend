using Application.Interfaces.RestoreCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RestorePasswordEmail
{
    public class RestoreCodeProvider : IRestoreCodeProvider
    {
        public string GenerateRestoreCode()
        {
            return "zxc";
        }
    }
}

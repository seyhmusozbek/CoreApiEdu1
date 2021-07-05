using CoreApiEdu1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO loginDTO);
        Task<string> CreateToken();

    }
}

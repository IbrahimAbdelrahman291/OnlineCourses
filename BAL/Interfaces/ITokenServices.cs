using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ITokenServices
    {
        Task<string> CreateTokenAsyync(AppUser user,UserManager<AppUser> userManager);
    }
}

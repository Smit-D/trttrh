using FirstTask.Entities.Models;
using FirstTask.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> RegisterAsync(RegisterVM model);
        User Login(LoginVM model);
        Task<bool> LogoutAsync();
    }
}

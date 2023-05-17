using FirstTask.Entities.Data;
using FirstTask.Entities.Models;
using FirstTask.Entities.ViewModels;
using FirstTask.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace FirstTask.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly FirstTaskDBContext _context;
        public UserRepository(FirstTaskDBContext context) : base(context)
        {
            _context = context;
        }

        public User Login(LoginVM model)
        {
            var FindUser = _context.Users.FirstOrDefault(user => user.Email == model.Email && user.Password == BCrypt.Net.BCrypt.HashPassword(model.Password));
            if (FindUser != null)
            {
                return FindUser;
            }
            else
            {
                User user = new User();
                return user;
            }
        }

        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(RegisterVM model)
        {
            try
            {
                var EmptyUser = new User();
                var GetUser = model.GetUserDetials(EmptyUser);
                if (GetUser != null)
                {
                    if (model.Avtar != null)
                    {
                        GetUser.Avtar = HelperRepository.ConvertImageToBase64(model.Avtar);
                    }
                    await _context.Users.AddAsync(GetUser);
                    return true;
                }
                return false;
            }
            catch
            {
                /*return new User()
                {
                    UserId = -2
                };*/
                return false;
            }
        }
    }
}

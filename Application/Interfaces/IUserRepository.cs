using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterNewUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);

    }
}

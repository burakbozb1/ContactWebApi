using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Core.Services
{
    public interface IUserService : IService<User>
    {
        Task<UserDto> SignUpAsync(UserSignUpDto user);
        Task<UserLoginResponseDto> LoginAsnyc(UserLoginDto lUser);
        Task<User> ChangePasswordAsync(long id,UserPasswordDto userInformations);
    }
}

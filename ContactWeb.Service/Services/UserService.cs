using AutoMapper;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using ContactWeb.Core.Repositories;
using ContactWeb.Core.Services;
using ContactWeb.Core.UnitOfWorks;
using ContactWeb.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(IGenericRepository<User> repository, IUnitOfWork unitOfWork, IGenericRepository<User> userRepository, IMapper mapper, IConfiguration configuration) : base(repository, unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<User> ChangePasswordAsync(long userId, UserPasswordDto userInformations)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.Password = userInformations.NewPassword;
                _userRepository.Update(user);
                await _unitOfWork.CommitAsync();
                return user;

            }
            throw new NotFoundException("User not found");
        }

        public async Task<UserLoginResponseDto> LoginAsnyc(UserLoginDto lUser)
        {
            var user = await _userRepository.Where(x => x.Email == lUser.Email && x.Password == lUser.Password).AsNoTracking().SingleOrDefaultAsync();
            if (user != null)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryDays"].ToString()));
                var issuer = _configuration["JwtIssuer"];
                var audier = _configuration["JwtAudience"];
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var token = new JwtSecurityToken(issuer, audier, claims, null, expiry, creds);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                UserLoginResponseDto loggedUser = _mapper.Map<UserLoginResponseDto>(user);
                loggedUser.Token = "Bearer " + tokenStr;
                return loggedUser;
            }
            throw new NotFoundException("User not found");
        }

        public async Task<UserDto> SignUpAsync(UserSignUpDto user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.CreatedDate = DateTime.Now;
            newUser.UpdatedDate = DateTime.Now;
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            var mappedUser = _mapper.Map<UserDto>(newUser);
            return mappedUser;
        }
    }
}

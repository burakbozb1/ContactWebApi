using AutoMapper;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using ContactWeb.Core.Services;
using ContactWeb.Service.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactWeb.Api.Controllers
{
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserSignUpDto user)
        {
            var result = await _userService.SignUpAsync(user);
            if (result!=null)
            {
                _logger.LogCritical($"{result.Email} user added.");
                return CreateActionResult(CustomResponseDto<UserDto>.Success(201, result));
            }
            throw new ServerSideException("Sign up failed. Try again later.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto lUser)
        {
            var result = await _userService.LoginAsnyc(lUser);
            _logger.LogCritical($"{result.Email} user logged in.");
            return CreateActionResult(CustomResponseDto<UserLoginResponseDto>.Success(200, result));
        }

        [Authorize]
        [HttpPut("passwordchange")]
        public async Task<IActionResult> PasswordChange(UserPasswordDto userInformations)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userService.ChangePasswordAsync(userId, userInformations);
            if (result!=null)
            {
                _logger.LogCritical($"{result.Id} user changed his/her password.");
                var response = CustomResponseDto<string>.Success(200, "Password changed");
                return CreateActionResult(response);
            }
            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(200, "Failed"));
        }
    }
}

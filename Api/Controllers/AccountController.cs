using Api.Services;
using Application.DTOs.Account;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _authenticationService.LoginAsync(request));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            return Ok(await _authenticationService.RegisterAsync(request));
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            return Ok(await _authenticationService.ChangePasswordAsync(request));
        }
        //[Authorize(Roles = "Admin")]
        //[HttpGet("{id}", Name = "GetUserById")]
        //public async Task<User> GetUserById(int id)
        //{
        //    return await _userRepository.GetUserByIdAsync(id);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet("{id}", Name = "GetUserDetailsById")]
        //public async Task<UserViewModel> GetUserDetailsById(int id)
        //{
        //    return Ok(await _authenticationService.GetUserDetailsById(id));
        //}

    }
}
            


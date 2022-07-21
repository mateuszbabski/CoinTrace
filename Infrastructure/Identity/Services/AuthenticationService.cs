using Application.DTOs.Account;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ICurrentUserService _userService;

        public AuthenticationService(IUserRepository userRepository,
            JWTSettings jwtSettings, 
            IMapper mapper, 
            IPasswordHasher<User> passwordHasher,
            ICurrentUserService userService)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        public async Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                return new AuthenticationResponse { 
                    IsSuccess = false, 
                    Errors = new[] { "Email or password incorrect" } 
                };

            var verifyPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if(verifyPassword == PasswordVerificationResult.Failed)
                return new AuthenticationResponse { 
                    IsSuccess = false, 
                    Errors = new[] { "Email or password incorrect" } 
                };

            return await GenerateAuthenticationResponseForUserAsync(user);
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            var isEmailInUse = await _userRepository.GetUserByEmailAsync(request.Email);
            if (isEmailInUse != null)
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Errors = new[] { "Email is already taken" }
                };
                
                    
            var newUser = new User()
            {
                FirstName = request.FirstName,
                PhoneNumber = request?.PhoneNumber,
                Email = request.Email,
                Password = request.Password,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, request.Password);
            newUser.PasswordHash = hashedPassword;
            newUser.Role = Roles.Basic.ToString();

            var user = _mapper.Map<User>(newUser);
            await _userRepository.RegisterNewUserAsync(user);

            //return await GenerateAuthenticationResponseForUserAsync(user);
            return new AuthenticationResponse 
            { 
                IsSuccess = true, 
                Id = user.Id, 
                Email = user.Email,
                FirstName = user.FirstName
            };
        }


        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var currentUser = _userRepository.GetUserByIdAsync(_userService.UserId);
            var user = _mapper.Map<User>(currentUser.Result);

            if (request.OldPassword != user.Password || request.NewPassword != request.ConfirmNewPassword)
                return new ChangePasswordResponse
                {
                    IsSuccess = false,
                    Errors = "Old password is incorrect or new password is not confirmed"
                };

            user.Password = request.NewPassword;
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            var changedPasswordUser = _mapper.Map<User>(request);

            await _userRepository.UpdateUserAsync(changedPasswordUser);

            return new ChangePasswordResponse
            {
                IsSuccess = true,
                Password = request.NewPassword
            };
        }
        private Task<AuthenticationResponse> GenerateAuthenticationResponseForUserAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName}"),
                new Claim(ClaimTypes.Email, $"{user.Email}"),
                new Claim(ClaimTypes.Role, $"{user.Role}")
            };

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtSettings.DurationInDays);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return Task.FromResult(new AuthenticationResponse
            {
                IsSuccess = true,
                JWTToken = tokenHandler.WriteToken(token),
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email
            });
        }
    }
}









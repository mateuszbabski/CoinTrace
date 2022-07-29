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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        public AuthenticationService(IUserRepository userRepository,
            JWTSettings jwtSettings, 
            IMapper mapper,
            ICurrentUserService userService)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _mapper = mapper;
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

            if(!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return new AuthenticationResponse { 
                    IsSuccess = false, 
                    Errors = new[] { "Email or password incorrect" } 
                };

            var token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                JWTToken = token
            };
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

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    
            var newUser = new User()
            {
                FirstName = request.FirstName,
                PhoneNumber = request?.PhoneNumber,
                Email = request.Email,
                Password = request.Password,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Roles.Basic.ToString()
            };

            var user = _mapper.Map<User>(newUser);
            await _userRepository.RegisterNewUserAsync(user);

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

            CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.Password = request.NewPassword;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var changedPasswordUser = _mapper.Map<User>(request);

            await _userRepository.UpdateUserAsync(changedPasswordUser);

            return new ChangePasswordResponse
            {
                IsSuccess = true,
                Password = request.NewPassword
            };
        }

        private string GenerateJwtToken(User user)
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

            return tokenHandler.WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }



    }
}









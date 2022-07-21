//using Application.DTOs.Account;
//using Application.DTOs.Email;
//using Application.Exceptions;
//using Application.Interfaces;
//using Application.Wrappers;
//using Domain.Entities;
//using Domain.Enums;
//using Domain.Settings;
//using Infrastructure.Identity.Model;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Identity.Services
//{
//    public class AccountServices : IAccountServices
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly JWTSettings _jwtSettings;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
//        private readonly IEmailService _emailService;

//        public AccountServices(UserManager<ApplicationUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            JWTSettings jwtSettings,
//            SignInManager<ApplicationUser> signInManager,
//            IPasswordHasher<ApplicationUser> passwordHasher,
//            IEmailService emailService)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _jwtSettings = jwtSettings;
//            _signInManager = signInManager;
//            _passwordHasher = passwordHasher;
//            _emailService = emailService;
//        }

//        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
//        {
//            var user = await _userManager.FindByEmailAsync(request.Email);
//            if (user == null)
//                throw new ApiException("Wrong email");

//            var result = await _signInManager.PasswordSignInAsync(user.Email, request.Password, false, lockoutOnFailure: false);
//            if (!result.Succeeded)
//                throw new ApiException("Invalid password");

//            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(user);
//            AuthenticationResponse response = new AuthenticationResponse();

//            response.Id = user.Id;
//            response.JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//            response.Email = user.Email;
//            response.FirstName = user.FirstName;

//            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
//            response.Roles = rolesList.ToList();

//            var refreshToken = GenerateRefreshToken();
//            response.RefreshToken = refreshToken.Token;

//            return new Response<AuthenticationResponse>(response, $"Authenticated {user.Email}");

//        }

//        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
//        {
//            var user = new User()
//            {
//                Email = request.Email,
//                FirstName = request.FirstName,
//                Password = request.Password
//            };
//            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
//            user.HashedPassword = hashedPassword;

//            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
//            if (userWithSameEmail != null)
//                throw new ApiException("Email is already taken");

//            var result = await _userManager.CreateAsync(user, request.Password);
//            if (result.Succeeded)
//            {
//                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
//            }
                

//            //var checkEmail = await _userManager.FindByEmailAsync(user.Email);
//            //if (checkEmail == null)
//            //    throw new ApiException($"{result.Errors}");

//            return new Response<string>(user.Id, message: $"User id: {user.Id} registered.");

//        }



//        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser user)
//        {
//            var jwtHandler = new JwtSecurityTokenHandler();
//            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Name, $"{user.FirstName}"),
//                new Claim(ClaimTypes.Email, $"{user.Email}"),
                
//            };

//            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var expires = DateTime.Now.AddDays(_jwtSettings.DurationInMinutes);


//            var tokenDescriptor = new JwtSecurityToken(_jwtSettings.Issuer,
//                _jwtSettings.Audience,
//                claims,
//                expires: expires,
//                signingCredentials: cred);

//            return tokenDescriptor;
//        }

//        private string RandomTokenString()
//        {
//            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
//            var randomBytes = new byte[40];
//            rngCryptoServiceProvider.GetBytes(randomBytes);
//            return BitConverter.ToString(randomBytes).Replace("-", "");
//        }


//        private RefreshToken GenerateRefreshToken()
//        {
//            return new RefreshToken
//            {
//                Token = RandomTokenString(),
//                Expires = DateTime.UtcNow.AddDays(7),
//                Created = DateTime.UtcNow
//            };
//        }

//        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
//        {
//            var account = await _userManager.FindByEmailAsync(model.Email);
//            if (account == null) return;

//            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
//            var route = "api/account/reset-password/";
//            var _endpointUri = new Uri(string.Concat($"{origin}/", route));
//            var emailRequest = new EmailRequest()
//            {
//                Body = $"Your reset token is = {code}",
//                To = model.Email,
//                Subject = "Reset password"
//            };

//            await _emailService.SendAsync(emailRequest);
//        }

//        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
//        {
//            var account = await _userManager.FindByEmailAsync(model.Email);
//            if (account == null) throw new ApiException($"Email not found");

//            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
//            if (!result.Succeeded)
//                throw new ApiException($"Error while reseting password");

//            return new Response<string>(model.Email, message: $"Password resetted");
//        }
//    }
//}

﻿using Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
        Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
        Task<ChangePasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<ChangePasswordResponse> ResetPasswordAsync(ResetPasswordRequest request, string token);

    }
}


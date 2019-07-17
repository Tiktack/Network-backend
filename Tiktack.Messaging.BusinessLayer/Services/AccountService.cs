﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tiktack.Common.Core.Infrastructure.Exceptions;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> LoginWithCredentials(string login, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(login, password, false, false);

            if (!result.Succeeded)
                throw new ApplicationException("INVALID_LOGIN_ATTEMPT");

            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == login);
            return GenerateJwtToken(appUser);
        }

        public async Task<string> Register(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) throw new ApplicationException(result.Errors.First().Description);

            await _signInManager.SignInAsync(user, false);
            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            if (user == null)
                throw new AppException("User should be not equal null",
                    ExceptionEventType.InvalidParameter);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                    user.Id),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid()
                        .ToString()),
                new Claim(ClaimTypes.Email,
                    user.Email),
                new Claim(ClaimTypes.Name,
                    user.UserName),
                new Claim("avatar",
                    user.Avatar?.Url ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
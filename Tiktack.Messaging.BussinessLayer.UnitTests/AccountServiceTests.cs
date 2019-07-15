using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tiktack.Common.Core.Infrastructure.Exceptions;
using Tiktack.Messaging.BusinessLayer.Services;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Entities.UserProperties;
using Xunit;
using Xunit.Abstractions;

namespace Tiktack.Messaging.BusinessLayer.UnitTests
{
    public class AccountServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IAccountService _accountService;
        private readonly ApplicationUser _testUser;
        private readonly IConfiguration _configuration;

        public AccountServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testUser = new ApplicationUser
            {
                Id = "1",
                Email = "aleh.khantsevich@gmail.com",
                Avatar = new Avatar(),
                UserName = "Aleh Khantsevich"
            };

            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();


            _accountService = new AccountService(_configuration);
        }

        [Fact(DisplayName = "GenerateJwtToken() should throw AppException if user equal null")]
        public void GenerateTokenShouldThrowExceptionIfUserEqualNull()
        {
            //Arrange
            Func<ApplicationUser, string> action = _accountService.GenerateJwtToken;

            //Act

            //Assert
            Assert.Throws<AppException>(() => action(null));
        }

        [Fact(DisplayName = "GenerateJwtToken() should generate valid JWT token")]
        public void ValidJwtToken()
        {
            //Arrange

            Func<ApplicationUser, string> action = _accountService.GenerateJwtToken;

            var validationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtIssuer"],
                    ValidAudience = _configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };

            var handler = new JwtSecurityTokenHandler();

            //Act

            //Assert
            Assert.NotNull(handler.ValidateToken(action(_testUser), validationParameters,
                out var validatedToken));
            Assert.NotNull(validatedToken);
            Assert.Throws<ArgumentException>(() => handler.ValidateToken("random string", validationParameters, out var validatedToken2));
        }
    }
}

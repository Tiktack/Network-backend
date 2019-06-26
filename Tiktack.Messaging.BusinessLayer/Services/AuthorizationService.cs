using Auth0.AuthenticationApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public class AuthorizationService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;

        public AuthorizationService(UnitOfWork unitOfWork, IMapper mapper, IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<Tuple<UserInfoDBLayer, string>> Authenticate(string username, string password)
        {
            var user = await _unitOfWork.Users.GetAll().SingleOrDefaultAsync(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt accessToken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return new Tuple<UserInfoDBLayer, string>(user, tokenString);
        }

        public async Task<Tuple<UserInfoDBLayer, string>> GenerateToken(string userIdentifier, string accessToken)
        {
            var user = await _unitOfWork.Users.GetAll().FirstOrDefaultAsync(x => x.UserIdentifier == userIdentifier);
            if (user == null)
            {
                var apiClient = new AuthenticationApiClient(new Uri("https://dev-vvcaaesb.eu.auth0.com/userinfo"));

                var userInfo = await apiClient.GetUserInfoAsync(accessToken);

                var mappedUser = _mapper.Map<UserInfoDBLayer>(userInfo);

                user = await _userProvider.Create(mappedUser);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return new Tuple<UserInfoDBLayer, string>(user, tokenString);
        }
    }
}

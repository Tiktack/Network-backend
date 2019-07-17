using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.WebApi.DTOs;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserController(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            ;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDialogsDTO>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<UserDialogsDTO[]>(users);
        }

        [HttpGet]
        [Authorize]
        public async Task<ApplicationUser> UserDetails() => await _userManager.GetUserAsync(User);
    }
}
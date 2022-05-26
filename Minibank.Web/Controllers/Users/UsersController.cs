using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<UserDto> GetUser(string userId)
        {
            var model = await _userService.GetUser(userId);

            return new UserDto()
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }


        [HttpGet]
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return users.Select(i => new UserDto()
            {
                Id = i.Id,
                Login = i.Login,
                Email = i.Email
            }).ToList();
        }

        [HttpPost]
        public Task Create(UserCreateDto model)
        {
            return _userService.CreateUser(new User()
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("{userId}")]
        public Task Update(string userId, UserUpdateDto model)
        {
            return _userService.UpdateUser(new User()
            {
                Id = userId,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("{userId}")]
        public Task Delete(string userId)
        {
            return _userService.DeleteUser(userId);
        }
    }
}

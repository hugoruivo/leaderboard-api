using LeaderboardAPI.Models;
using LeaderboardAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LeaderboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() =>
            Unauthorized();

        [HttpPost]
        public ActionResult<CustomApiResponse> Create(User user)
        {
            //Check if empty username
            if (null != user.UserName && "" != user.UserName.Trim())
            {
                User existingUser = _userService.GetByUsername(user.UserName);

                //Check if username is registered / exists
                if (null != existingUser && null != existingUser.Id)
                {
                    //Exists
                    return BadRequest(new CustomApiResponse(true, "Username already exists"));
                }
                _userService.Create(user);

                return StatusCode(201, new CustomApiResponse(false, "", user));
            }
            return BadRequest(new CustomApiResponse(true, "Username is required"));
        }
    }
}

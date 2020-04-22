﻿using LeaderboardAPI.Models;
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
            _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            //For the badrequest: Could return other thing as result
            //maybe a json formatted object or so, but will keep this simple

            //Check if empty username
            if (null != user.UserName && "" != user.UserName.Trim())
            {
                User existingUser = _userService.GetByUsername(user.UserName);

                //Check if username is registered / exists
                if (null != existingUser && null != existingUser.Id)
                {
                    //Exists
                    return BadRequest("Username already exists");
                }
                _userService.Create(user);

                return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
            }
            return BadRequest("Username is required");
        }
    }
}

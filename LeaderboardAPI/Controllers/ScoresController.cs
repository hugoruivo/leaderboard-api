using LeaderboardAPI.Models;
using LeaderboardAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LeaderboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ScoreService _scoreService;

        public ScoresController(UserService userService, ScoreService scoreService)
        {
            _userService = userService;
            _scoreService = scoreService;
        }

        [HttpGet]
        public ActionResult<CustomApiResponse> Get(int page, int numItems = 2)
        {
            return new CustomApiResponse(false, "", _scoreService.Get(page, numItems));
        }

        [HttpPost]
        public ActionResult<CustomApiResponse> Create(Score score)
        {
            //Check if empty username
            if (null != score.UserName && "" != score.UserName.Trim())
            {
                User existingUser = _userService.GetByUsername(score.UserName);
                //Check if username is registered / exists
                if (null != existingUser && null != existingUser.Id)
                {
                    //It Exists, we can try to insert the score
                    if(0 < score.UserScore)
                    {
                        //User exists and has a score: need to check if already in the DB to update or not
                        Score existingScore = _scoreService.GetByUsername(score.UserName);
                        if (null != existingScore && null != existingScore.Id)
                        {
                            //User has an existing score,
                            //will only update if new score is higher than existing one
                            if(existingScore.UserScore >= score.UserScore)
                            {
                                return BadRequest(new CustomApiResponse(true, "Score is lower or equal than existing high score"));
                            }
                            //New score is higher than existing one, update it
                            existingScore.UserScore = score.UserScore;
                            _scoreService.Update(existingScore.Id, existingScore);
                            return StatusCode(201, new CustomApiResponse(false, "", existingScore));
                        }
                        //User has no existing score, will create it
                        _scoreService.Create(score);
                        return StatusCode(201, new CustomApiResponse(false, "", score));
                    }
                    return BadRequest(new CustomApiResponse(true, "Score is required"));
                }
                return BadRequest(new CustomApiResponse(true, "Username is not registered"));
            }
            return BadRequest(new CustomApiResponse(true, "Username is required"));
        }
    }
}

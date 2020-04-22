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
        public ActionResult<ScorePage> Get(int page)
        {
            return _scoreService.Get(page);
        }

        [HttpGet("{id:length(24)}", Name = "GetUserScore")]
        public ActionResult<Score> Get(string id)
        {
            var score = _scoreService.Get(id);

            if (score == null)
            {
                return NotFound();
            }

            return score;
        }

        [HttpPost]
        public ActionResult<Score> Create(Score score)
        {
            //Could return other thing as result
            //maybe a json formatted object or so, but will keep this simple

            //Check if empty username
            if (null != score.UserName && "" != score.UserName.Trim())
            {
                User existingUser = _userService.GetByUsername(score.UserName);
                //Check if username is registered / exists
                if (null != existingUser && null != existingUser.Id)
                {
                    //It Exists, we can try to insert the score
                    if(0 <= score.UserScore)
                    {
                        //User exists and has a score: need to check if already in the DB to update or not
                        Score existingScore = _scoreService.GetByUsername(score.UserName);
                        if (null != existingScore && null != existingScore.Id)
                        {
                            //User has an existing score,
                            //will only update if new score is higher than existing one
                            if(existingScore.UserScore >= score.UserScore)
                            {
                                return BadRequest("Score is lower or equal than existing high score");
                            }
                            //New score is higher than existing one, update it
                            existingScore.UserScore = score.UserScore;
                            _scoreService.Update(existingScore.Id, existingScore);
                            return CreatedAtRoute("GetUserScore", new { id = existingScore.Id.ToString() }, existingScore);
                        }
                        //User has no existing score, will create it
                        _scoreService.Create(score);
                        return CreatedAtRoute("GetUserScore", new { id = score.Id.ToString() }, score);
                    }
                    return BadRequest("Score is required");
                }
                return BadRequest("Username is not registered");
            }
            return BadRequest("Username is required");
        }
    }
}

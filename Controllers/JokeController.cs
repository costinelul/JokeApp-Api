using Microsoft.AspNetCore.Mvc;
using server.Services.JokeService;
namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokeController : ControllerBase
    {
        private readonly IJokeService _jokeService;
        private readonly Authorization _validateService;
        private string GetUserJwt()
        {
            var jwt = Request.Cookies["jwt"];
            if (jwt is null) return " ";
            return jwt;
        }
        public JokeController(IJokeService jokeService, Authorization validateService)
        {
            _jokeService = jokeService;
            _validateService = validateService;
        }

        [HttpPost("post")]
        public async Task<ActionResult<Joke>> Post(JokeDto dto)
        {
            int userId = _validateService.ValidateUser(GetUserJwt());
            if (userId > 0)
            {
                await _jokeService.CreateJoke(userId, dto);
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Joke>>> GetAll()
        {
            return await _jokeService.GetAllJokes();
        }

        [HttpPut("updateJoke/{id}")]
        public async Task<ActionResult<Joke>> UpdateJoke(int id, JokeDto dto)
        {
            if (_validateService.ValidateUser(GetUserJwt()) > 0)
            {
                var result = await _jokeService.UpdateJoke(id, dto);
                if (result is null)
                    return NotFound("Joke not found! :(");
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPut("updateJoke/laugh/{jokeId}")]
        public async Task<ActionResult<int>> SendLaugh(int jokeId)
        {
            var laugherId = _validateService.ValidateUser(GetUserJwt());
            if (laugherId > 0)
            {
                var result = await _jokeService.Laugh(jokeId, laugherId);
                if (result == -1) return NotFound("Joke not found :(");
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet("has-user-laughed/{jokeId}")]

        public async Task<ActionResult<bool>> HasUserLaughed(int jokeId)
        {
            var userId = _validateService.ValidateUser(GetUserJwt());
            var response = await _jokeService.HasLaughed(jokeId, userId);
            return response;
        }

        [HttpDelete("deleteJoke/{id}")]
        public async Task<ActionResult<Joke>> DeleteJoke(int id)
        {
            if (_validateService.ValidateUser(GetUserJwt()) > 0)
            {
                var result = await _jokeService.DeleteJoke(id);
                if (result is null)
                    return NotFound("Joke not found! :(");
                return Ok();
            }
            return Unauthorized();
        }
    }
}
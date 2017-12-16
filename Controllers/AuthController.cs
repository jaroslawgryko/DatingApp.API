using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                ModelState.AddModelError("username", "Alredy exists");

            // validate request
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);


            var useToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var userToCreate = await _repo.Register(useToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}
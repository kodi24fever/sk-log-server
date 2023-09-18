using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;



namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;




        [HttpGet]
        public async Task<ActionResult> Get()
        {

            return Ok("Hello");

        }


        // Adding post method to api
        [HttpPost]

        public async Task<ActionResult> Post([FromBody] UserRegisterDto dto)
        {


            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();



            string? username = dto.UserName;
            string? email = dto.Email;
            string? password = dto.Password;



            return Ok(username);

        }

    }
}
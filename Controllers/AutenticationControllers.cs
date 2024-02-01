using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealEstateApi.Enumeratori;
using RealEstateApi.Models;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Models.RequestModelsDTO;
using RealEstateApi.Repositories.Interfaces;
using Serilog;
using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticationControllers:Controller
    {
        private readonly ILogger<AutenticationControllers> _logger;   
        private readonly IAutentication _userRepo;
        public AutenticationControllers(ILogger<AutenticationControllers> logger, IAutentication userRepo) 
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var autenticationResult = await _userRepo.Login(login);
            if(autenticationResult == null)
            {
                _logger.LogInformation("Wrong credential or account not found, please, register");
                return NotFound("Wrong credential or account not found, please, register");
            }

            return Ok(autenticationResult);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<UserDTO>> Logout(LoginDTO login)
        {
            var autenticationResult = await _userRepo.Logout(login);
            if(autenticationResult == null)
            {
                _logger.LogInformation("Logout attempt failed");
                return BadRequest("Logout attempt failed");
            }

            return Ok(autenticationResult);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RequestRegisterDTO registerDTO)
        {
            if (registerDTO.DataAccount.Role != RoleEnum.OWNER.ToString() && registerDTO.DataAccount.Role != RoleEnum.TENANT.ToString())
            {
                return BadRequest("Invalid Role");
            }

            var autenticationResult = await _userRepo.Register(registerDTO);

            if(autenticationResult == null)
            {
                _logger.LogInformation("Registration failed");
                return BadRequest("Registration failed");
            }

            return Ok(autenticationResult);
        }

        [HttpPost("getToken")]
        public async Task<ActionResult<string>> getToken(LoginDTO userDto)
        {
            var token = await _userRepo.Login(userDto);
            
            if (token == null)
            {
                return Unauthorized("Username or password is incorrect.");
            }

            return Ok(token);
        }
    }
}

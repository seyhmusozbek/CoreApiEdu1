using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiEdu1.Entities;
using CoreApiEdu1.Models;
using CoreApiEdu1.Services;

namespace CoreApiEdu1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<AppUser> userManager, ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost(Name = "Register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Kayıt işlemi denendi{userDTO.UserName}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<AppUser>(userDTO);
                var result = await _userManager.CreateAsync(user,userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted(StatusCode(201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Invalid post attempt in {nameof(Register)}");
                return BadRequest(new { success = false, product = userDTO });
            }
        }

        [HttpPost(Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            _logger.LogInformation($"Giriş işlemi denendi{userDTO.UserName}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if(!await _authManager.ValidateUser(userDTO))
                {
                    return Unauthorized(userDTO);
                }
              

                return Accepted(new { Token=await _authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Invalig login attempt in {nameof(Login)}");
                return BadRequest(new { success = false, product = userDTO });
            }
        }
    }
}

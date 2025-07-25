using BAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.DTOs;

namespace OnlineCourses.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenServices _token;

        public AccountController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager, ITokenServices token)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _token = token;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model) 
        {
            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password) || string.IsNullOrEmpty(model.email))
            {
                return BadRequest("Username, password and email are required.");
            }
            var existingUser = await _userManager.FindByEmailAsync(model.email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }
            var userName = model.username.Trim().Replace(" ","");
            var newUser = new AppUser() 
            {
                UserName = userName,
                Email = model.email,
                PhoneNumber = model.PhoneNumber,
                DisplayName = model.DisplayName,
            };
            if (!await _roleManager.RoleExistsAsync("Student"))
            {
                var RoleResult = await _roleManager.CreateAsync(new IdentityRole("Student"));
                if (!RoleResult.Succeeded)
                {
                    return BadRequest("Failed to create role");
                }
            }
            var CreateUserResult = await _userManager.CreateAsync(newUser,model.password);
            if (!CreateUserResult.Succeeded)
            {
                var errors = string.Join(" | ", CreateUserResult.Errors.Select(e => e.Description));
                return BadRequest($"Can't Creating new user with user name : {model.username}\n{errors}");
            }
            var AddToRoleResult = await _userManager.AddToRoleAsync(newUser, "Student");
            if (!AddToRoleResult.Succeeded)
            {
                return BadRequest($"Can't Adding user to role : {model.username}");
            }
            var userDTO = new UserDTO
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
                Token = await _token.CreateTokenAsync(newUser,_userManager),
            };
            return Ok(userDTO);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            if (string.IsNullOrEmpty(model.emailOrPassword) || string.IsNullOrEmpty(model.password))
            {
                return BadRequest("Username and password are required.");
            }
            var user = await _userManager.FindByNameAsync(model.emailOrPassword);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.emailOrPassword);
                if (user == null) 
                {
                    return Unauthorized("Invalid username or email");
                }
            }
            var result = await _userManager.CheckPasswordAsync(user, model.password);
            if (!result)
            {
                return Unauthorized("Incorrect password");
            }
            var userDTO = new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await _token.CreateTokenAsync(user, _userManager),
            };
            return Ok(userDTO);
        }
    }
}

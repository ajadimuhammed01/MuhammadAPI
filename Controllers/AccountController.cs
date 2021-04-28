using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using techHowdy.API.Models;
using System.Linq;
using System.Text;
using techHowdy.API.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace techHowdy.API.Controllers
{
     [Route("api/[controller]")]
    public class AccountController: Controller
    {
        private  readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;

        private readonly AppSettings _appSettings;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _appSettings = appSettings.Value;
        }
        //www.example.com/api/account/register
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel formdata){
            //will hold all errors related to registration
            List<string> errorList = new List<string>();
            
            var user = new IdentityUser
            {
                Email = formdata.Email,
                UserName = formdata.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            
            };

            var result = await _userManager.CreateAsync(user, formdata.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                //Sending Confirmation Email
                return Ok(new {username = user.UserName, email = user.Email, status=1, message ="Registration Successful" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    errorList.Add(error.Description);
                }
            }

            return BadRequest(new JsonResult(errorList));

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel formdata) 
        { 
           //Get User from Database
           var user = await _userManager.FindByNameAsync(formdata.UserName);

           var roles = await _userManager.GetRolesAsync(user);

           var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

           double tokenExpiryTime = Convert.ToDouble (_appSettings.ExpireTime);

           if (user != null && await _userManager.CheckPasswordAsync(user, formdata.Password))
           {
               //Confirmation of email
               var tokenHandler = new JwtSecurityTokenHandler();

               var  tokenDescriptor = new SecurityTokenDescriptor
               {
                  Subject = new ClaimsIdentity(new Claim[]
                  {
                      new Claim(JwtRegisteredClaimNames.Sub, formdata.UserName),
                      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                      new Claim(ClaimTypes.NameIdentifier, user.Id),
                      new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                      new Claim("Logged", DateTime.Now.ToString()),
                  }), 
                  
                  SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                  Issuer = _appSettings.Site,
                  Audience = _appSettings.Audience,
                  Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
               };

               //Generate Token
               var token = tokenHandler.CreateToken(tokenDescriptor);

               return Ok(new {token = tokenHandler.WriteToken(token), expiration = token.ValidTo, username = user.UserName, password = user.PasswordHash,
               userRole = roles.FirstOrDefault()               
                });
           }

           ModelState.AddModelError("", "Username/Password was not found");
           
          return Unauthorized();

         //return Unauthorized (new {LoginError= "Please Check the Login Credentials - Invalid Username/Password was entered"});
        }
    } 
}
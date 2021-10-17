using DatingApp.API.Data;
using DatingApp.API.Entities;
using DatingApp.API.Entities.Models;
using DatingApp.API.Interfaces;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(AppUserRequest appUserRequest)
        {
           
            if (await UserExists(appUserRequest.Username))
            {
                return BadRequest("Username is already taken");
            }


            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = appUserRequest.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(appUserRequest.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserModel
            { 
                Username = user.UserName,
                Token =  _tokenService.CreateToken(user)
            
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(LoginUser loginUser)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginUser.Username);

            if (user == null) return Unauthorized("Invalid login attempt");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password");
            }


            return new UserModel
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)

            };
        }

        #region Private methods

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        #endregion

    }
}

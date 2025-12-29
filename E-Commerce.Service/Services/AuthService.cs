using E_Commerce.Domian.Entites.IdentityModule;
using E_Commerce.Service.Abstraction.Interfaces;
using E_Commerce.Shared.Common;
using E_Commerce.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Implementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Result<UserDTO>.Fail(Error.InvalidCrendentials("Invaild caredentials"));
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
            {
                return Result<UserDTO>.Fail(Error.InvalidCrendentials("Invaild caredentials"));
            }
            var userDTO = new UserDTO
            {
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await GenterateTokenAsync(user)
            };

            return Result<UserDTO>.Ok(userDTO);
        }


        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                var userDTO = new UserDTO
                {
                    Email = user.Email!,
                    DisplayName = user.DisplayName,
                    Token = await GenterateTokenAsync(user)
                };
                return Result<UserDTO>.Ok(userDTO);
            }
            var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
            return Result<UserDTO>.Fail(errors);

        }

        public async Task<Result<bool>> CheckEmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)

            {
                return Result<bool>.Ok(false);
            }
            return Result<bool>.Ok(true);
        }
        public async Task<Result<UserDTO>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<UserDTO>.Fail(Error.NotFound("User not found"));
            }
            var userDTO = new UserDTO
            {
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await GenterateTokenAsync(user)
            };
            return Result<UserDTO>.Ok(userDTO);
        }

        private async Task<string> GenterateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
               {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTOptions:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenDescriptor);
        }
    }
}

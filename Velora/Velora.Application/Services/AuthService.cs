using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Velora.Application.InputModel;
using Velora.Application.Services.Interface;
using Velora.Application.ViewModel;
using Velora.Domain.ModelAggregate.User;

namespace Velora.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private AppUser AppUser;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            AppUser = new();
        }

        public async Task<IEnumerable<IdentityError>> WorkspaceRegister(WorkspaceRegister workspaceRegister)
        {
            AppUser.Firstname = workspaceRegister.Firstname;
            AppUser.Lastname = workspaceRegister.Lastname;
            AppUser.Email = workspaceRegister.Email;
            AppUser.DateOfBirth = workspaceRegister.DateOfBirth;
            AppUser.UserName = workspaceRegister.Email;

            var result = await _userManager.CreateAsync(AppUser, workspaceRegister.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(AppUser, $"{workspaceRegister.Role.ToUpper()}");
            }

            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            AppUser.Firstname = register.Firstname;
            AppUser.Lastname = register.Lastname;
            AppUser.Email = register.Email;
            AppUser.DateOfBirth = register.DateOfBirth;
            AppUser.UserName = register.Email;

            var result = await _userManager.CreateAsync(AppUser, register.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(AppUser, "CUSTOMER");
            }

            return result.Errors;
        }

        public async Task<object> Login(Login login)
        {
            AppUser = await _userManager.FindByEmailAsync(login.Email);

            if (AppUser == null) return "Invalid Email Address";

            var result = await _userManager.CheckPasswordAsync(AppUser, login.Password);

            if (result)
            {
                var token = await GenerateToken();

                LoginResponse loginResponse = new LoginResponse
                {
                    UserId = AppUser.Id,
                    Token = token
                };

                return loginResponse;
            }
            else
            {
                return "Invalid Password";
            }
        }

        private async Task<string> GenerateToken()
        {
            //1.Security Key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            //2.Signin Credentials
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //3.Gets Roles of User
            var roles = await _userManager.GetRolesAsync(AppUser);

            //4.Get or Create Claim or Add Roles as Claims
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,AppUser.Email)
            }.Union(roleClaims).ToList();

            //5.Configure Token
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]))
                );

            //6.Create Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> IsUserExists(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null) return false;

            return true;
        }
    }
}

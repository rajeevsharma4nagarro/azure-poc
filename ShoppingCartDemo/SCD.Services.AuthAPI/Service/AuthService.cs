using Microsoft.AspNetCore.Identity;
using SCD.Services.AuthAPI.Data;
using SCD.Services.AuthAPI.Models;
using SCD.Services.AuthAPI.Models.Dto;
using SCD.Services.AuthAPI.Service.IService;

namespace SCD.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator) { 
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = ""};
            }

            var roles = await _userManager.GetRolesAsync(user);
            UserDto userDto = new() { 
                Email = user.NormalizedEmail, 
                FullName = user.FullName,
                //Id = user.Id,
                //PhoneNumber = user.PhoneNumber,
                //Address = user.Address,
                //City = user.City,
                //State = user.State,
                //Zip = user.Zip
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = _jwtTokenGenerator.GenerateToken(user, roles.ToList())
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                FullName = registrationRequestDto.FullName,
                Email = registrationRequestDto.Email,
                Address = registrationRequestDto.Address,
                City = registrationRequestDto.City,
                State = registrationRequestDto.State,
                Zip = registrationRequestDto.Zip,
                NormalizedEmail = registrationRequestDto.Email.ToUpper()
            };

            try
            {
                var result = _userManager.CreateAsync(user, registrationRequestDto.Password);
                if(result.Result.Succeeded)
                {
                    var roleName = registrationRequestDto.RoleName.ToString();
                    var newuser = _db.ApplicationUsers.First(u => u.Email == registrationRequestDto.Email);
                    if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    }
                    await _userManager.AddToRoleAsync(newuser, roleName);

                    return "";
                }
                else
                {
                    return result.Result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
               
            }
            return "Error encountered";
        }
    }
}

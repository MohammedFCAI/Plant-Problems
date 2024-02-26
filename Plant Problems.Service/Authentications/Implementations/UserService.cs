using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Plant_Problems.Data.Models;
using Plant_Problems.Data.Models.Authentications;
using Plant_Problems.Data.Models.Configurations;
using Plant_Problems.Infrastructure.Interfaces;
using Plant_Problems.Service.Authentications.Interfaces;
using Plant_Problems.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Plant_Problems.Service.Authentications.Implementations
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailService _emailService;
		private readonly ILogger<UserService> _logger;
		private readonly IConfiguration _configuration;

		public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration)
		{
			_userManager = userManager;
			_logger = logger;
			_unitOfWork = unitOfWork;
			_emailService = emailService;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<ApplicationUser>> ChangePassword(string username, string oldPassword, string newPassword, string confirmNewPassword)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user is null)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "Invalid username!" };

			if (newPassword != confirmNewPassword)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "New Password Must Match with Confirm New Password!" };

			var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					_logger.LogError($"Error: {error.Code}, Description: {error.Description}");

				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = $"{string.Join(", ", result.Errors.Select(e => e.Description))}" };

			}
			return new ServiceResponse<ApplicationUser> { Entities = user, Success = true, Message = "Password Changed Successfully." };
		}

		public async Task<ServiceResponse<ApplicationUser>> CreateUser(Register model)
		{
			var emailExist = await _userManager.FindByEmailAsync(model.Email);
			var usernameExist = await _userManager.FindByNameAsync(model.Username);


			if (emailExist is not null)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "Email Already Exist!" };


			else if (usernameExist is not null)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "Username Already Exist!" };

			var user = new ApplicationUser
			{
				UserName = model.Username,
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			if (model.Password != model.ConfirmPassword)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "Passwords do not match!" };

			var result = await _userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					_logger.LogError($"Error: {error.Code}, Description: {error.Description}");

				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = $"{string.Join(", ", result.Errors.Select(e => e.Description))}" };

			}

			return new ServiceResponse<ApplicationUser> { Entities = user, Success = true, Message = "User Added Successfully." };
		}

		public async Task<ServiceResponse<ApplicationUser>> SignIn(Login model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "Invalid username or password!" };


			return new ServiceResponse<ApplicationUser> { Entities = user, Success = true, Message = "User successfully logged in" };
		}

		public void Detach(ApplicationUser user)
		{
			_unitOfWork.UserRepository.Detach(user);
		}

		public async Task<ServiceResponse<ApplicationUser>> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return new ServiceResponse<ApplicationUser> { Entities = null, Success = false, Message = "User is not found!" };
			return new ServiceResponse<ApplicationUser> { Entities = user, Success = true, Message = "User is found" };
		}

		public async Task<ServiceResponse<List<ApplicationUser>>> GetUsersList()
		{
			var users = await _userManager.Users.ToListAsync();
			if (users == null || !users.Any())
				return new ServiceResponse<List<ApplicationUser>>() { Entities = null, Success = true, Message = "No users found!" };

			return new ServiceResponse<List<ApplicationUser>>() { Entities = users, Success = true, Message = "Users found" };
		}

		public async Task<ServiceResponse<ApplicationUser>> DeleteUser(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
				return new ServiceResponse<ApplicationUser>() { Entities = null, Success = false, Message = "User id is not found!" };

			await _userManager.DeleteAsync(user);
			return new ServiceResponse<ApplicationUser>() { Entities = user, Success = true, Message = "User deleted." };
		}

		public async Task<ServiceResponse<ApplicationUser>> UpdateUser(ApplicationUser appUser)
		{
			var user = await _userManager.FindByIdAsync(appUser.Id);

			if (user == null)
				return new ServiceResponse<ApplicationUser>() { Entities = null, Success = false, Message = "User id is not found!" };

			await _userManager.UpdateAsync(user);
			return new ServiceResponse<ApplicationUser>() { Entities = user, Success = true, Message = "User updated." };
		}


		public async Task<ServiceResponse<string>> ForgetPassword(ForgetPassword request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				return new ServiceResponse<string>() { Entities = "", Success = false, Message = "User email is not found!" };

			string newPassword = GenerateRandomPassword(12);

			// Set the new password for the user
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

			if (!result.Succeeded)
				return new ServiceResponse<string>() { Entities = "", Success = false, Message = "Password reset failed. Please try again." };

			var content = $"Hello {user.UserName}, your new password is {newPassword}";
			var message = new Message(user.Email!, "Forget password", content!);

			await _emailService.SendEmailAsync(message);
			return new ServiceResponse<string>() { Entities = "", Success = true, Message = "Password reset successfully. Check your email for the new password and change it as you want." };
		}


		public async Task<ServiceResponse<TokensResponse>> LoginWithJwt(Login model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
				return new ServiceResponse<TokensResponse>() { Entities = null, Success = false, Message = "Error in Email or Password!" };

			var userClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var userRoles = await _userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
			{
				userClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var jwtToken = GetToken(userClaims); // access token


			var tokens = await GetJwtTokn(user);


			return new ServiceResponse<TokensResponse>()
			{
				Entities = new TokensResponse()
				{
					AccessToken = tokens.Entities.AccessToken.Token,
					AccessTokenExpireOn = tokens.Entities.AccessToken.ExpireTokenDate,
					RefreshToken = tokens.Entities.RefreshToken.Token,
					RefreshTokenExpireOn = tokens.Entities.RefreshToken.ExpireTokenDate
				},
				Success = tokens.Success,
				Message = tokens.Message
			};
		}

		public async Task<ServiceResponse<LoginResponse>> RenewAccessToken(LoginResponse tokens)
		{
			try
			{
				var accessToken = tokens.AccessToken;
				var refreshToken = tokens.RefreshToken;
				var principal = GetClaimsPrincipal(accessToken.Token);

				var user = await _userManager.FindByNameAsync(principal.Identity.Name);

				if (refreshToken.Token != user.RefreshToken && refreshToken.ExpireTokenDate <= DateTime.Now)
					return new ServiceResponse<LoginResponse>
					{
						Entities = null,
						Success = false,
						Message = $"Token invalid or expired"
					};


				var response = await GetJwtTokn(user);
				return response;
			}
			catch (Exception ex)
			{
				return new ServiceResponse<LoginResponse>() { Message = ex.Message };
			}
		}

		//// Private Methods.
		private static string GenerateRandomPassword(int length)
		{
			const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%_";

			Random random = new Random();
			char[] password = new char[length];

			for (int i = 0; i < length; i++)
			{
				password[i] = validChars[random.Next(validChars.Length)];
			}

			return new string(password);
		}


		private JwtSecurityToken GetToken(List<Claim> userClaim)
		{
			var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
			_ = int.TryParse(_configuration["JWT:TokenValidityInDays"], out int tokenValidityInDays);
			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: DateTime.Now.AddMinutes(tokenValidityInDays),
				claims: userClaim,
				signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
				);
			return token;
		}

		private string GenerateRefreshToken()
		{
			var randomNumber = new Byte[64];
			var range = RandomNumberGenerator.Create();
			range.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}


		private async Task<ServiceResponse<LoginResponse>> GetJwtTokn(ApplicationUser user)
		{
			var userClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var userRoles = await _userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
			{
				userClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var jwtToken = GetToken(userClaims); // access token
			var refreshToken = GenerateRefreshToken();
			_ = int.TryParse(_configuration["JWT:RefrshTokenValidity"], out int refrshTokenValidity);

			user.RefreshToken = refreshToken;
			user.RefreshTokenExpire = DateTime.UtcNow.AddDays(refrshTokenValidity);
			await _userManager.UpdateAsync(user);

			return new ServiceResponse<LoginResponse>
			{
				Entities = new LoginResponse()
				{
					AccessToken = new JwtToken()
					{
						Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
						ExpireTokenDate = jwtToken.ValidTo
					},
					RefreshToken = new JwtToken()
					{
						Token = user.RefreshToken,
						ExpireTokenDate = (DateTime)user.RefreshTokenExpire
					}
				},
				Success = true,
				Message = "Tokens"
			};
		}


		private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
		{
			var tokenValidateionParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(accessToken, tokenValidateionParameters, out SecurityToken
				 securityToken);

			return principal;
		}
	}
}

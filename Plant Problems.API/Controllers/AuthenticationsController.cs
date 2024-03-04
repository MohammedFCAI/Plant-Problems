namespace Plant_Problems.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationsController : AppControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public AuthenticationsController(IMediator mediator, UserManager<ApplicationUser> userManager, IUserService userService, IEmailService emailService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] NewRegisterRequest user)
        {
            var registeredUser = new Register() { Username = user.UserName, Email = user.Email, ConfirmPassword = user.ConfirmPassword, Password = user.Password };

            var userManager = await _userService.CreateUser(registeredUser);
            if (userManager.Success)
            {
                var appUser = userManager.Entities;

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                var confirmationLink = Url.Action(nameof(_emailService.ConfirmEmail), "Authentications", new { token, email = user.Email }, Request.Scheme);

                var message = new Message(appUser.Email!, "confirmation email Link", $"Please confirm your email. Click on this link to confirm your email.\n{confirmationLink!}");
                try
                {
                    await _emailService.SendEmailAsync(message);
                    return Ok($"User created successfully. We sent a varification code on your email. Please confirm your email.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"{ex.Message}. Please check your email.");
                }

            }
            else
                return BadRequest(userManager.Message);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] Login login)
        {
            var userResponse = await _userService.LoginWithJwt(login);
            if (!userResponse.Success)
                return NotFound(userResponse.Message);
            return Ok(new
            {
                tokens = userResponse.Entities
            });
        }


        [HttpPost("forget-password/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var requset = await _mediator.Send(new ForgetPasswordRequest(email));
            return NewResult(requset);
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangeUserPasswordRequest user)
        {
            var requset = await _mediator.Send(user);
            return NewResult(requset);
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromForm] UserRoleRequest user)
        {
            var requset = await _mediator.Send(user);
            return NewResult(requset);
        }




        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] LoginResponse tokens)
        {
            var userResponse = await _userService.RenewAccessToken(tokens);
            if (!userResponse.Success)
                return BadRequest(userResponse.Message);

            return Ok(new { tokens = userResponse.Entities });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("confirm-email")]
        public async Task<string> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await _userManager.ConfirmEmailAsync(user, token);

            return "Email Verified Successfully";
        }

        //[HttpPost("send-email")]
        private async Task<IActionResult> SendEmail([FromForm] SendEmailRequest email)
        {
            var requset = await _mediator.Send(email);
            return NewResult(requset);
        }
    }
}

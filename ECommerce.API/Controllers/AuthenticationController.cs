using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationServices authenticationServices;

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            this.authenticationServices = authenticationServices;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.LoginAsync(loginDto, ct));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.RegisterAsync(registerDto, ct));
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.CheckEmailAsync(email, ct));
        }
        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetCurrentUser(email, ct));
        }
        [HttpGet("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDto>> GetUserAddress([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetUserAddressAsync(email, ct));
        }
        [HttpPost("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDto>> UpdateUserAddress([FromQuery] string email, [FromBody] AddressDto addressDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.UpdateUserAddressAsync(addressDto, email, ct));
        }
    }
}  
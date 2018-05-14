using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PG.Api.DtoModels;
using PG.Api.Identity;
using PG.BLL;
using PG.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PG.Api.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private UserManager _userManager;
        public UserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<UserManager>();
            private set => _userManager = value;
        }
        
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        private readonly IUserProfileService _userProfileService;

        public AccountController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public AccountController(UserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        
        [Route("Register"), HttpPost]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                IsActive = true
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var profile = new UserProfile
                {
                    AppUser = user,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                _userProfileService.Create(profile);
            }
            else
            {
                return GetErrorResult(result);
            }

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            return Ok(new { userId = user.Id, confirmCode = HttpUtility.UrlEncode(code) });
        }
        
        [Route("Confirm", Name = "ConfirmEmail"), HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return BadRequest("Email could not be confirmed.");
            }
            
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var passwordToken = await UserManager.GeneratePasswordResetTokenAsync(userId);
                return Json(new { code = passwordToken });
            }

            return BadRequest("Email could not be confirmed.");

        }
        
        [Authorize]
        [Route("{userId}/ConfirmEmailUrl"), HttpGet]
        public async Task<IHttpActionResult> GetConfirmEmailUrl(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var url = Url.Link("ConfirmEmail", new {userId, code = HttpUtility.UrlEncode(code)});

            return Ok(new { url });
        }
        
        [Authorize]
        [Route("{userId}/Activate"), HttpPost]
        public async Task<IHttpActionResult> ActivateUser(int userId)
        {
            return await SetActiveStatus(userId, true);
        }
        
        [Authorize]
        [Route("{userId}/Deactivate"), HttpPost]
        public async Task<IHttpActionResult> DeactivateUser(int userId)
        {
            return await SetActiveStatus(userId, false);
        }

        [Route("{email}/ResetPasswordUrl"), HttpGet]
        public async Task<IHttpActionResult> GetResetPasswordUrl(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var userId = user?.Id ?? 1;

            var code = await UserManager.GeneratePasswordResetTokenAsync(userId);

            var url = Url.Link("ResetPassword", null);
            var data = new ResetPasswordDto
            {
                Email = email,
                Code = HttpUtility.UrlEncode(code),
                Password = "new-password",
                ConfirmPassword = "confirm-password"
            };

            return Ok(new {url, data});
        }

        [Route("ResetPassword", Name = "ResetPassword"), HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize]
        [Route("{userId}/Delete"), HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private async Task<IHttpActionResult> SetActiveStatus(int userId, bool activeStatus)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();

            if (user.IsActive != activeStatus)
            {
                user.IsActive = activeStatus;
                var result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return GetErrorResult(result);
            }

            return Ok();
        }
    }
}
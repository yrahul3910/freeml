using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MLServer.Application.Interfaces;
using MLServer.Application.ViewModels.v1.UserAccount;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;
using MLServer.Domain.Interfaces;
using MLServer.Infra.CrossCutting.Identity.Models;
using MLServer.Services.Api.Configurations;

namespace MLServer.Services.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/test")]
    [Produces("application/json")]
    public class TestController : ApiController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserAccountAppService _userAccountAppService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DomainNotificationHandler _notifications;
        private readonly IUser _user;
        private readonly AppSettings _appSettings;

        public TestController(
            SignInManager<IdentityUser> signInManager,
            IUserAccountAppService userAccountAppService,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings,
            IUser user,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator)
            : base(notifications, mediator)
        {
            _signInManager = signInManager;
            _userAccountAppService = userAccountAppService;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _user = user;
            _notifications = (DomainNotificationHandler)notifications;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserAccountViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Get()
        {
            var response = _userAccountAppService.GetAll();

            if (IsValidOperation())
            {
                return Ok(response);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserAccountViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Get(Guid id)
        {
            var response = _userAccountAppService.GetById(id);

            if (IsValidOperation())
            {
                return Ok(response);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserAccountViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterNewUserAccountViewModel userAccount)
        {
            if (!ModelState.IsValid || userAccount.Password != userAccount.ConfirmPassword)
            {
                NotifyModelStateErrors();
                return Response(userAccount);
            }

            var response = await _userAccountAppService.Register(userAccount);

            if (IsValidOperation())
            {
                if (!(response is UserAccountViewModel userAccountViewModel))
                    return Response(response);

                var user = new IdentityUser
                {
                    Id = userAccountViewModel.Id.ToString(),
                    UserName = userAccount.Email,
                    Email = userAccount.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, userAccount.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        NotifyError(error.Code, error.Description);
                    }

                    _userAccountAppService.Remove(userAccountViewModel.Id);

                    return Response(userAccount);
                }

                await _signInManager.SignInAsync(user, false);
                var token = await GenerateJwt(userAccount.Email);

                userAccountViewModel.Token = token;

                return Ok(userAccountViewModel);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Put(Guid id, [FromBody] UpdateUserAccountViewModel userAccount)
        {
            userAccount.Id = id;
            _userAccountAppService.Update(userAccount);

            if (IsValidOperation())
            {
                return NoContent();
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(Guid id)
        {
            _userAccountAppService.Remove(id);

            if (IsValidOperation())
            {
                return NoContent();
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        private async Task<string> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidAt,
                Expires = DateTime.UtcNow.AddDays(_appSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
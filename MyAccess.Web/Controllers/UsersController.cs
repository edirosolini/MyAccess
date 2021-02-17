// <copyright file="UsersController.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using MyAccess.Domains.Models;
    using MyAccess.Domains.Services;

    public class UsersController : Controller
    {
        private static string returnUrl;
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult LogIn([FromQuery] string returnUrl)
        {
            var claimsPrincipal = this.User;

            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                UsersController.returnUrl = returnUrl;
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    return this.Redirect($"{returnUrl}?tokenType={claimsPrincipal.FindFirst("TokenType")?.Value}&tokenAccess={claimsPrincipal.FindFirst("TokenAccess")?.Value}&businessName={claimsPrincipal.FindFirst("BusinessName")?.Value}");
                }
            }
            else
            {
                UsersController.returnUrl = returnUrl;
                return this.View();
            }
        }

        [HttpPost]
        public IActionResult LogIn(AuthenticateRequestModel model)
        {
            var authenticate = this.service.Authenticate(model);

            if (authenticate == null)
            {
                return this.View();
            }
            else
            {
                this.GenerateSign(authenticate);
                return this.Redirect($"{returnUrl}?tokenType={authenticate.TokenType}&tokenAccess={authenticate.TokenAccess}&businessName={authenticate.BusinessName}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogOut()
        {
            this.HttpContext.SignOutAsync();
            return this.RedirectToAction("LogIn");
        }

        public IActionResult AccessDenied() => this.Forbid();

        private void GenerateSign(AuthenticateResponseModel model)
        {
            var claimsIdentity = new ClaimsIdentity(
                new[]
                {
                    new Claim("Id", model.Id.ToString()),
                    new Claim("BusinessName", $"{model.BusinessName}"),
                    new Claim("TokenType", model.TokenType),
                    new Claim("TokenAccess", model.TokenAccess),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(480),
                    IsPersistent = true,
                    AllowRefresh = true,
                });
        }
    }
}

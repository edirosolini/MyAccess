// <copyright file="UsersController.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.WebApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;
    using MyAccess.Domains.Services;

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private static string returnUrl;
        private readonly IUserService service;
        private readonly IUserItemService userItemService;

        public UsersController(IUserService service, IUserItemService userItemService)
        {
            this.service = service;
            this.userItemService = userItemService;
        }

        [HttpGet]
        [AllowAnonymous]
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
                    return this.Redirect($"{returnUrl}?tokenType={claimsPrincipal.FindFirst("TokenType")?.Value}&tokenAccess={claimsPrincipal.FindFirst("TokenAccess")?.Value}");
                }
            }
            else
            {
                UsersController.returnUrl = returnUrl;
                return this.View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn(AuthenticateRequest model)
        {
            if (this.ModelState.IsValid)
            {
                var authenticate = this.service.Authenticate(model);

                if (authenticate == null)
                {
                    this.ModelState.AddModelError("authenticate", "User or Password invalid");
                    return this.View();
                }
                else
                {
                    this.GenerateSign(authenticate);
                    return this.Redirect($"{returnUrl}?businessName={authenticate.BusinessName}&tokenType={authenticate.TokenType}&tokenAccess={authenticate.TokenAccess}");
                }
            }
            else
            {
                return this.View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogOut()
        {
            this.HttpContext.SignOutAsync();
            return this.RedirectToAction("LogIn");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => this.View();

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                this.ModelState.AddModelError("forgotPassword", "The username is empty");
                return this.View();
            }

            var authenticate = this.service.GetByUsername(username);

            if (authenticate == null)
            {
                this.ModelState.AddModelError("forgotPassword", "The username is invalid");
                return this.View();
            }

            this.service.ForgotPassword(username, $"{authenticate.TokenType} {authenticate.TokenAccess}");
            return this.RedirectToAction("LogIn");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => this.View();

        [HttpGet]
        public IActionResult Index()
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            var list = this.service.Get(0, 0);
            return this.View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(UserEntity entity)
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            try
            {
                if (this.ModelState.IsValid)
                {
                    this.service.Insert(entity);
                    return this.RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
            }

            return this.View(entity);
        }

        [HttpGet]
        public IActionResult Update(Guid? id)
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            if (id == null)
            {
                return this.BadRequest();
            }
            else
            {
                var user = this.service.Get((Guid)id);
                return this.View(user);
            }
        }

        [HttpPost]
        public IActionResult Update(UserEntity entity)
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            try
            {
                if (this.ModelState.IsValid)
                {
                    this.service.Update(entity);
                    return this.RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
            }

            return this.View(entity);
        }

        [HttpGet]
        public IActionResult Remove(Guid? id)
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            if (id == null)
            {
                return this.BadRequest();
            }
            else
            {
                var user = this.service.Get((Guid)id);
                return this.View(user);
            }
        }

        [HttpPost]
        public IActionResult Remove(UserEntity entity)
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            try
            {
                this.service.Delete(entity.Id);
                return this.RedirectToAction("Index");
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
            }

            var user = this.service.Get(entity.Id);
            return this.View(user);
        }

        private void GenerateSign(AuthenticateResponse model)
        {
            var claimsIdentity = new ClaimsIdentity(
                new[]
                {
                    new Claim("Id", $"{model.Id}"),
                    new Claim("BusinessName", $"{model.BusinessName}"),
                    new Claim("TokenType", $"{model.TokenType}"),
                    new Claim("TokenAccess", $"{model.TokenAccess}"),
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

        private bool ValidAccess()
        {
            var claimsPrincipal = this.User;
            var userId = Guid.Parse(claimsPrincipal.FindFirst("Id").Value);
            var records = this.userItemService.GetByUserId(userId).Records;
            var entity = records.Where(x => x.Item.Key == "MyAccess-Menus-Users").FirstOrDefault();

            return entity != null;
        }
    }
}

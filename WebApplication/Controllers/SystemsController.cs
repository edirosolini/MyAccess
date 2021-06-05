// <copyright file="SystemsController.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.WebApplication.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Services;

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class SystemsController : Controller
    {
        private readonly ISystemService service;
        private readonly IUserItemService userItemService;

        public SystemsController(ISystemService service, IUserItemService userItemService)
        {
            this.service = service;
            this.userItemService = userItemService;
        }

        public IActionResult Index()
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            var list = this.service.Get(0, 0);
            return this.View(list);
        }

        public IActionResult Create()
        {
            if (!this.ValidAccess())
            {
                return this.RedirectToAction("AccessDenied", "Users");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(SystemEntity entity)
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
                var system = this.service.Get((Guid)id);
                return this.View(system);
            }
        }

        [HttpPost]
        public IActionResult Update(SystemEntity entity)
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
                var system = this.service.Get((Guid)id);
                return this.View(system);
            }
        }

        [HttpPost]
        public IActionResult Remove(SystemEntity entity)
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

            var system = this.service.Get(entity.Id);
            return this.View(system);
        }

        private bool ValidAccess()
        {
            var claimsPrincipal = this.User;
            var userId = Guid.Parse(claimsPrincipal.FindFirst("Id").Value);
            var records = this.userItemService.GetByUserId(userId).Records;
            var entity = records.Where(x => x.Item.Key == "MyAccess-Menus-Systems").FirstOrDefault();

            return entity != null;
        }
    }
}

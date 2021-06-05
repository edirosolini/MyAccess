// <copyright file="UsersController.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.WebApplication.APIControllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;
    using MyAccess.Domains.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("/api/[action]")]
        [AllowAnonymous]
        public ActionResult<AuthenticateResponse> Authenticate([FromForm] AuthenticateRequest request)
        {
            var authenticate = this.service.Authenticate(request);

            if (authenticate == null)
            {
                return this.Unauthorized();
            }

            return this.Ok(authenticate);
        }
    }
}

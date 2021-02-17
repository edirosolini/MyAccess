// <copyright file="UserService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using MyAccess.Commons;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Services;

    public class UserService : IUserService
    {
        private readonly IUserDao dao;

        public UserService(IUserDao dao)
        {
            this.dao = dao;
        }

        public AuthenticateResponseModel Authenticate(AuthenticateRequestModel model)
        {
            var users = this.dao.GetList();
            var user = this.dao.GetList().SingleOrDefault(x => x.EmailAddress == model.Username && PasswordHash.ValidatePassword(model.Password, x.Password) && x.Active);

            // return null if user not found
            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticateResponseModel(user, token);
        }

        public ResponseModel ChangePassword(Guid id, ChangePasswordModel model)
        {
            var user = this.dao.GetList().SingleOrDefault(x => x.Id == id && PasswordHash.ValidatePassword(model.CurrentPassword, x.Password) && x.Active);

            // return null if user not found
            if (user != null)
            {
                user.Password = PasswordHash.CreateHash(model.NewPassword);

                if (this.dao.Update(user))
                {
                    return new ResponseModel() { Status = true, Menssage = "Updated password" };
                }
                else
                {
                    return new ResponseModel() { Status = false, Menssage = "Not Updated password" };
                }
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "User or password not match" };
            }
        }

        public ResponseModel Delete(Guid id)
        {
            var entity = this.dao.GetById(id);
            entity.Active = false;

            if (this.dao.Delete(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Deleted user" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Deleted user" };
            }
        }

        public object GetById(Guid id) => this.dao.GetById(id);

        public object GetList(int since, int limit)
        {
            var data = this.dao.GetList();

            if (limit == 0)
            {
                return new
                {
                    recordsQuantity = data.Count(),
                    records = data
                        .OrderBy(x => x.EmailAddress)
                        .ToList()
                        .Skip(since),
                };
            }
            else
            {
                return new
                {
                    recordsQuantity = data.Count(),
                    records = data
                        .OrderBy(x => x.EmailAddress)
                        .ToList()
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ResponseModel Insert(UserEntity entity)
        {
            entity.Password = PasswordHash.CreateHash("12345678");

            if (this.dao.Insert(entity) > 0)
            {
                return new ResponseModel() { Status = true, Menssage = "Inserted user" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Inserted user" };
            }
        }

        public ResponseModel Update(UserEntity entity)
        {
            var currentPassword = this.dao.GetById(entity.Id).Password;

            entity.Password = currentPassword;

            if (this.dao.Update(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Updated user" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated user" };
            }
        }

        private static string GenerateJwtToken(UserEntity user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, string.Empty),
                new Claim("Id", user.Id.ToString()),
            };

            var payload = new JwtPayload(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1));

            var token = new JwtSecurityToken(
                    header,
                    payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

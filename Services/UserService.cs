// <copyright file="UserService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using MyAccess.Commons;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;
    using MyAccess.Domains.Services;
    using RestSharp;

    public class UserService : IUserService
    {
        private readonly IUserDao dao;
        private readonly IMapper mapper;

        public UserService(IUserDao dao, IMapper mapper)
        {
            this.dao = dao;
            this.mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var users = this.dao.GetList();
            var user = this.dao.GetList().SingleOrDefault(x => x.EmailAddress == model.Username && PasswordHash.ValidatePassword(model.Password, x.Password) && x.Active);

            // return null if user not found
            if (user == null)
            {
                return null;
            }

            return this.mapper.Map<AuthenticateResponse>(user);
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
            var entity = this.dao.Get(id);
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

        public UserEntity Get(Guid id) => this.dao.Get(id);

        public ListModel<UserEntity> Get(int since, int limit)
        {
            var data = this.dao.GetList();

            if (limit == 0)
            {
                return new ListModel<UserEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.EmailAddress)
                        .Skip(since),
                };
            }
            else
            {
                return new ListModel<UserEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.EmailAddress)
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ResponseModel Insert(UserEntity entity)
        {
            entity.Password = PasswordHash.CreateHash(entity.Password);
            this.dao.Insert(entity);
            return new ResponseModel() { Status = true, Menssage = "Inserted user" };
        }

        public ResponseModel Update(UserEntity entity)
        {
            if (this.dao.Update(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Updated user" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated user" };
            }
        }

        public ResponseModel ForgotPassword(string username, string authorization)
        {
            var entity = this.dao.GetList().Where(x => x.EmailAddress == username).FirstOrDefault();

            var newPassword = RandomString.Generate(12);

            entity.Password = PasswordHash.CreateHash(newPassword);

            if (this.dao.Update(entity))
            {
                var client = new RestClient($"{Environment.GetEnvironmentVariable("URI_MyMail")}/api/Mail")
                {
                    Timeout = -1,
                };

                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", $"{authorization}");
                request.AddParameter("Source.Name", "MyAccess");
                request.AddParameter("Recipients[0].lastName", entity.LastName);
                request.AddParameter("Recipients[0].firstName", entity.FirstName);
                request.AddParameter("Recipients[0].emailAddress", entity.EmailAddress);
                request.AddParameter("Notification.Subject", "MyAccess - Forgot Password");
                request.AddParameter("Notification.Body", $"A password reset was performed, your new password is: '{newPassword}'");
                IRestResponse response = client.Execute(request);

                return new ResponseModel() { Status = true, Menssage = "Updated password" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated password" };
            }
        }

        public AuthenticateResponse GetByUsername(string username)
        {
            var entity = this.dao.GetList().Where(u => u.EmailAddress == username)?.FirstOrDefault();
            if (entity == null)
            {
                return null;
            }

            return this.mapper.Map<AuthenticateResponse>(entity);
        }
    }
}

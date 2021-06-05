// <copyright file="UserItemService.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using System;
    using System.Linq;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Models;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Services;

    public class UserItemService : IUserItemService
    {
        private readonly IUserItemDao dao;
        private readonly IUserService userService;
        private readonly IItemService itemService;

        public UserItemService(IUserItemDao dao, IUserService userService, IItemService itemService)
        {
            this.dao = dao;
            this.userService = userService;
            this.itemService = itemService;
        }

        public ResponseModel Delete(Guid id)
        {
            var entity = this.dao.Get(id);
            entity.Active = false;

            if (this.dao.Delete(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Deleted item" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Deleted item" };
            }
        }

        public UserItemEntity Get(Guid id)
        {
            var entity = this.dao.Get(id);
            entity.User = this.userService.Get(entity.UserId);
            entity.Item = this.itemService.Get(entity.ItemId);
            return entity;
        }

        public ListModel<UserItemEntity> Get(int since, int limit)
        {
            var data = from ui in this.dao.GetList()
                       join u in this.userService.Get(0, 0).Records on ui.UserId equals u.Id
                       join i in this.itemService.Get(0, 0).Records on ui.ItemId equals i.Id
                       select new UserItemEntity
                       {
                           Id = ui.Id,
                           Active = ui.Active,
                           UserId = ui.UserId,
                           User = u,
                           ItemId = ui.ItemId,
                           Item = i,
                       };

            if (limit == 0)
            {
                return new ListModel<UserItemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .Skip(since),
                };
            }
            else
            {
                return new ListModel<UserItemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ListModel<UserItemEntity> GetByUserId(Guid userId)
        {
            var data = from ui in this.dao.GetList()
                       join u in this.userService.Get(0, 0).Records on ui.UserId equals u.Id
                       join i in this.itemService.Get(0, 0).Records on ui.ItemId equals i.Id
                       where ui.UserId == userId
                       select new UserItemEntity
                       {
                           Id = ui.Id,
                           Active = ui.Active,
                           UserId = ui.UserId,
                           User = u,
                           ItemId = ui.ItemId,
                           Item = i,
                       };

            return new ListModel<UserItemEntity>
            {
                RecordsQuantity = data.Count(),
                Records = data,
            };
        }

        public ResponseModel Insert(UserItemEntity entity)
        {
            this.dao.Insert(entity);
            return new ResponseModel() { Status = true, Menssage = "Inserted item" };
        }

        public ResponseModel Update(UserItemEntity entity)
        {
            if (this.dao.Update(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Updated item" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated item" };
            }
        }
    }
}

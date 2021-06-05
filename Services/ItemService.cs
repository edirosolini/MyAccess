// <copyright file="ItemService.cs" company="El Roso">
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

    public class ItemService : IItemService
    {
        private readonly IItemDao dao;
        private readonly ISystemDao systemDao;
        private readonly ITypeDao typeDao;

        public ItemService(IItemDao dao, ISystemDao systemDao, ITypeDao typeDao)
        {
            this.dao = dao;
            this.systemDao = systemDao;
            this.typeDao = typeDao;
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

        public ItemEntity Get(Guid id)
        {
            var entity = this.dao.Get(id);
            entity.System = this.systemDao.Get(entity.SystemId);
            entity.Type = this.typeDao.Get(entity.TypeId);
            return entity;
        }

        public ListModel<ItemEntity> Get(int since, int limit)
        {
            var data = from i in this.dao.GetList()
                    join s in this.systemDao.GetList() on i.SystemId equals s.Id
                    join t in this.typeDao.GetList() on i.TypeId equals t.Id
                    select new ItemEntity
                    {
                        Id = i.Id,
                        Active = i.Active,
                        SystemId = i.SystemId,
                        TypeId = i.TypeId,
                        Name = i.Name,
                        System = s,
                        Type = t,
                    };

            if (limit == 0)
            {
                return new ListModel<ItemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since),
                };
            }
            else
            {
                return new ListModel<ItemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ResponseModel Insert(ItemEntity entity)
        {
            this.dao.Insert(entity);
            return new ResponseModel() { Status = true, Menssage = "Inserted item" };
        }

        public ResponseModel Update(ItemEntity entity)
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

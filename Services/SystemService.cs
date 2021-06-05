// <copyright file="SystemService.cs" company="El Roso">
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

    public class SystemService : ISystemService
    {
        private readonly ISystemDao dao;

        public SystemService(ISystemDao dao)
        {
            this.dao = dao;
        }

        public ResponseModel Delete(Guid id)
        {
            var entity = this.dao.Get(id);
            entity.Active = false;

            if (this.dao.Delete(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Deleted system" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Deleted system" };
            }
        }

        public SystemEntity Get(Guid id) => this.dao.Get(id);

        public ListModel<SystemEntity> Get(int since, int limit)
        {
            var data = this.dao.GetList();

            if (limit == 0)
            {
                return new ListModel<SystemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since),
                };
            }
            else
            {
                return new ListModel<SystemEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ResponseModel Insert(SystemEntity entity)
        {
            this.dao.Insert(entity);
            return new ResponseModel() { Status = true, Menssage = "Inserted system" };
        }

        public ResponseModel Update(SystemEntity entity)
        {
            if (this.dao.Update(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Updated system" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated system" };
            }
        }
    }
}

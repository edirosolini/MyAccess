// <copyright file="TypeService.cs" company="El Roso">
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

    public class TypeService : ITypeService
    {
        private readonly ITypeDao dao;

        public TypeService(ITypeDao dao)
        {
            this.dao = dao;
        }

        public ResponseModel Delete(Guid id)
        {
            var entity = this.dao.Get(id);
            entity.Active = false;

            if (this.dao.Delete(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Deleted type" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Deleted type" };
            }
        }

        public TypeEntity Get(Guid id) => this.dao.Get(id);

        public ListModel<TypeEntity> Get(int since, int limit)
        {
            var data = this.dao.GetList();

            if (limit == 0)
            {
                return new ListModel<TypeEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since),
                };
            }
            else
            {
                return new ListModel<TypeEntity>
                {
                    RecordsQuantity = data.Count(),
                    Records = data
                        .OrderBy(x => x.Name)
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public ResponseModel Insert(TypeEntity entity)
        {
            this.dao.Insert(entity);
            return new ResponseModel() { Status = true, Menssage = "Inserted type" };
        }

        public ResponseModel Update(TypeEntity entity)
        {
            if (this.dao.Update(entity))
            {
                return new ResponseModel() { Status = true, Menssage = "Updated type" };
            }
            else
            {
                return new ResponseModel() { Status = false, Menssage = "Not Updated type" };
            }
        }
    }
}

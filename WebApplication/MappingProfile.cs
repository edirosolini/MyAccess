// <copyright file="MappingProfile.cs" company="El Roso">
// Copyright (c) El Roso. All rights reserved.
// </copyright>

namespace MyAccess.WebApplication
{
    using AutoMapper;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Responses;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<UserEntity, AuthenticateResponse>().ForMember(d => d.BusinessName, o => o.MapFrom(s => $"{s.LastName} {s.FirstName}"));
        }
    }
}

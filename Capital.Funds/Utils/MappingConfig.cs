﻿using AutoMapper;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;

namespace Capital.Funds.Utils
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {

            CreateMap<Users, TenantPersonalInfoDto>();
            CreateMap<TenantPersonalInfoDto, Users>();
            CreateMap<PropertyDetails, PropertyDetails>();
            CreateMap<TenatDetails, TenatDetails>();
            CreateMap<TenantPayments, TenantPayments>();
            CreateMap<ComplaintsDTO, ComplaintsDTO>();
            CreateMap<DDLTenantName,DDLTenantName>();
            CreateMap<DDLPropertyName, DDLPropertyName>();
            CreateMap<DDLUserName, DDLUserName>();
            //CreateMap<Object,ObjDTO>;
        }
    }
}
// ObjDto = Source
// Object = Destinantion
// Object obj = _mapper.map<Object>(ObjDTO)
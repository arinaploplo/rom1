﻿using AutoMapper;
using api3.Dto;
using api3.Models;

namespace api3.Helper
{
    public class MapingControler : Profile
    {
        public MapingControler()
        {
            // Get (de la tabla al dto)
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Inventory, InventoryDto>();
            CreateMap<Store, StoreDto>();

            // Post (del dto a la tabla)
            CreateMap<EmployeeDto, Employee>();
            CreateMap<InventoryDto, Inventory>();
            CreateMap<StoreDto, Store>();




        }


    }
}

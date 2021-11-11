using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreApiEdu1.Entities;
using CoreApiEdu1.Models;

namespace CoreApiEdu1.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, AddProductDTO>().ReverseMap();
            CreateMap<Production, AddProductionDTO>().ReverseMap();
            CreateMap<MStop, AddStopDTO>().ReverseMap();
            CreateMap<ChosenOrder, AddChosenOrderDTO>().ReverseMap();
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<Plan, AddPlanDTO>().ReverseMap();
            CreateMap<StockReserve, AddStockReserveDTO>().ReverseMap();
            CreateMap<WHTransfer, AddWHTransferDTO>().ReverseMap();
            CreateMap<CountMaster, CountMasterDTO>().ReverseMap();
            CreateMap<CountDetail, CountDetailsDTO>().ReverseMap();


        }
    }
}

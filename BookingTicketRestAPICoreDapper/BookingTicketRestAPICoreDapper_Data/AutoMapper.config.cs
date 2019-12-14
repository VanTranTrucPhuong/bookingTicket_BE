using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data
{
    public class AutoMapperConfig : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a => a.AddProfile<AutoMapperConfig>());
        }
        public AutoMapperConfig()
        {
            CreateMap<ThanhVienViewModel, ThanhVien>();
            CreateMap<Phim, PhimViewModel>();
            CreateMap<PhimInsert, Phim>().ReverseMap();
        }
    }
}

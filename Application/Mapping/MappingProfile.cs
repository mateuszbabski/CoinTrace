using Application.DTOs.Account;
using Application.DTOs.Budget;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, User>().ReverseMap();
            //CreateMap<User, UserViewModel>().ReverseMap();

            CreateMap<ChangePasswordRequest, User>()
                .ForMember(x => x.Password, c => c.MapFrom(w => w.NewPassword));

            CreateMap<CreateBudgetRequest, Budget>();
                //.ForMember(x => x.CreatedById, c => c.MapFrom(w => w.UserId));

            CreateMap<Budget, BudgetViewModel>().ReverseMap();

        }
    }
}

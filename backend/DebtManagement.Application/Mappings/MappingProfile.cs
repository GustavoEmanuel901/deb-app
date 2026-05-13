using AutoMapper;
using DebtManagement.Application.DTOs;
using DebtManagement.Domain.Entities;

namespace DebtManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Installment, InstallmentDetailDto>();
            CreateMap<CreateDebtDto, Debt>();
        }
    }
}
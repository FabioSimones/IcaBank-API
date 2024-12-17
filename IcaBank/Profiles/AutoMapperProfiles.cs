using AutoMapper;
using IcaBank.Models;
using IcaBank.Models.DTOs;

namespace IcaBank.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<RegisterNewAccountDTO, Account>();
            CreateMap<UpdateAccountDTO, Account>();
            CreateMap<Account, GetAccountDTO>();
            CreateMap<TransactionRequestDTO, Transaction>();
        }
        
    }
}

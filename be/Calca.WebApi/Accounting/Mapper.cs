using AutoMapper;
using Calca.Domain.Accounting;
using Calca.WebApi.Accounting.Dto;
using System.Linq;

namespace Calca.WebApi.Accounting
{
    public static class Mapper
    {
        private static readonly IMapper InternalMapper;

        static Mapper()
        {
            InternalMapper = new MapperConfiguration(m =>
            {
                m.CreateMap<Ledger, LedgerDto>(MemberList.Destination);
                m.CreateMap<LedgerMember, LedgerMemberDto>(MemberList.Destination);
                //m.CreateMap<Ledger, LedgerApiDto>(MemberList.Destination);
                //m.CreateMap<LedgerMember, MemberApiDto>(MemberList.Destination);
                //m.CreateMap<Operation, OperationApiDto>(MemberList.Destination)
                //    .ForMember(x => x.From, x => x.MapFrom(op => op.From.Select(m => m.Id).ToList()))
                //    .ForMember(x => x.To, x => x.MapFrom(op => op.To.Select(m => m.Id).ToList()));

                //m.CreateMap<LedgerCreateUpdateDto, Ledger>(MemberList.Source);
                //m.CreateMap<MemberApiDto, LedgerMember>(MemberList.Source);
                //m.CreateMap<OperationCreateUpdateApiDto, Operation>(MemberList.Source);
            }).CreateMapper();
        }

        public static LedgerDto Map(Ledger ledger)
        {
            return InternalMapper.Map<Ledger, LedgerDto>(ledger);
        }

        //public static Ledger Map(LedgerCreateUpdateDto ledger)
        //{
        //    return InternalMapper.Map<LedgerCreateUpdateDto, Ledger>(ledger);
        //}
    }
}

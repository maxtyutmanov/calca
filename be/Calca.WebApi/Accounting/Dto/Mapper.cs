using AutoMapper;
using Calca.Domain.Accounting;

namespace Calca.WebApi.Accounting.Dto
{
    public static class Mapper
    {
        private static readonly IMapper InternalMapper;

        static Mapper()
        {
            InternalMapper = new MapperConfiguration(m =>
            {
                m.CreateMap<Ledger, LedgerApiDto>(MemberList.Source);
                m.CreateMap<Member, MemberApiDto>(MemberList.Source);
                m.CreateMap<Operation, OperationApiDto>(MemberList.Source);
            }).CreateMapper();
        }

        public static LedgerApiDto Map(Ledger ledger)
        {
            return InternalMapper.Map<Ledger, LedgerApiDto>(ledger);
        }
    }
}

using AutoMapper;
using Calca.Domain.Accounting;
using Calca.WebApi.Accounting.Dto;
using System.Collections.Generic;
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
                m.CreateMap<Ledger, LedgerDto>();
                m.CreateMap<LedgerMember, LedgerMemberDto>()
                    .ForSourceMember(m => m.LedgerId, o => o.DoNotValidate());
                m.CreateMap<LedgerOperation, LedgerOperationDto>()
                    .ForSourceMember(m => m.LedgerId, o => o.DoNotValidate());
                m.CreateMap<OperationMember, OperationMemberDto>()
                    .ForSourceMember(m => m.OperationId, o => o.DoNotValidate());

                m.CreateMap<LedgerDto, Ledger>();
                m.CreateMap<LedgerMemberDto, LedgerMember>()
                    .ForMember(m => m.LedgerId, o => o.Ignore());
                m.CreateMap<LedgerOperationDto, LedgerOperation>()
                    .ForMember(m => m.LedgerId, o => o.Ignore());
                m.CreateMap<OperationMemberDto, OperationMember>()
                    .ForMember(m => m.OperationId, o => o.Ignore());
            }).CreateMapper();
        }

        public static LedgerDto Map(Ledger ledger)
        {
            return InternalMapper.Map<Ledger, LedgerDto>(ledger);
        }

        public static Ledger Map(LedgerDto ledger)
        {
            return InternalMapper.Map<LedgerDto, Ledger>(ledger);
        }

        public static IReadOnlyList<LedgerOperationDto> Map(IReadOnlyList<LedgerOperation> operations)
        {
            return InternalMapper.Map<IReadOnlyList<LedgerOperation>, IReadOnlyList<LedgerOperationDto>>(operations);
        }

        public static LedgerOperation Map(LedgerOperationDto operation)
        {
            return InternalMapper.Map<LedgerOperationDto, LedgerOperation>(operation);
        }
    }
}

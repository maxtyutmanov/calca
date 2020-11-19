using AutoMapper;
using Calca.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public static class Mapper
    {
        static Mapper()
        {
            var configuration = new MapperConfiguration(m =>
            {
                m.CreateMap<Ledger, LedgerApiDto>();
                m.CreateMap<Member, MemberApiDto>();
                m.CreateMap<Operation, OperationApiDto>();
            });
        }
    }
}

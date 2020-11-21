using AutoMapper;
using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.WebApi.Accounting.Dto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi
{
    public class DtoMapper : IDtoMapper
    {
        private static readonly MapperConfiguration MapConfig;
        private readonly IMapper _internalMapper;

        static DtoMapper()
        {
            MapConfig = new MapperConfiguration(m =>
            {
                // read scenario

                m.CreateMap<Ledger, LedgerReadDto>(MemberList.Destination);
                m.CreateMap<LedgerMember, LedgerMemberDto>(MemberList.Destination)
                    .ForSourceMember(m => m.LedgerId, o => o.DoNotValidate());
                m.CreateMap<LedgerOperation, LedgerOperationDto>(MemberList.Destination)
                    .ForSourceMember(m => m.LedgerId, o => o.DoNotValidate());
                m.CreateMap<OperationMember, OperationMemberDto>(MemberList.Destination)
                    .ForSourceMember(m => m.OperationId, o => o.DoNotValidate());

                // create scenario

                m.CreateMap<LedgerCreateDto, Ledger>()
                    .ForMember(x => x.Version, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.MapFrom<DateTimeUtcNowResolver<LedgerCreateDto, Ledger>>())
                    .ForMember(x => x.CreatorId, o => o.Ignore());
                m.CreateMap<LedgerOperationCreateDto, LedgerOperation>()
                    .ForMember(x => x.LedgerId, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.MapFrom<DateTimeUtcNowResolver<LedgerOperationCreateDto, LedgerOperation>>())
                    .ForMember(x => x.CreatorId, o => o.Ignore());

                // update scenario

                m.CreateMap<LedgerUpdateDto, Ledger>(MemberList.Source);

                // common

                m.CreateMap<LedgerMemberDto, LedgerMember>()
                    .ForMember(m => m.LedgerId, o => o.Ignore());
                m.CreateMap<OperationMemberDto, OperationMember>()
                    .ForMember(m => m.OperationId, o => o.Ignore());
            });
        }

        public DtoMapper(IServiceProvider provider)
        {
            _internalMapper = new Mapper(MapConfig, t => provider.GetService(t));
        }

        public TTarget Map<TSource, TTarget>(TSource source)
        {
            return _internalMapper.Map<TSource, TTarget>(source);
        }
    }

    public class DateTimeUtcNowResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, DateTime>
    {
        private readonly ISystemClock systemClock;

        public DateTimeUtcNowResolver(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public DateTime Resolve(TSource source, TDestination destination, DateTime destMember, ResolutionContext context)
        {
            return systemClock.UtcNow;
        }
    }
}

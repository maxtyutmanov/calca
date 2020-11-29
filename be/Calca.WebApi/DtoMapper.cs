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
                m.CreateMap<LedgerOperation, LedgerOperationReadDto>(MemberList.Destination)
                    .ForSourceMember(m => m.LedgerId, o => o.DoNotValidate());
                m.CreateMap<OperationMember, OperationMemberDto>(MemberList.Destination)
                    .ForSourceMember(m => m.OperationId, o => o.DoNotValidate())
                    .ForMember(m => m.Side, o => o.ConvertUsing<OperationSide>(new OperationSideConverter()));

                // create scenario

                m.CreateMap<LedgerCreateDto, Ledger>()
                    .ForMember(x => x.Version, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.MapFrom<DateTimeUtcNowResolver<LedgerCreateDto, Ledger>>())
                    .ForMember(x => x.CreatorId, o => o.MapFrom<CurrentUserIdResolver<LedgerCreateDto, Ledger>>());
                m.CreateMap<LedgerOperationCreateDto, LedgerOperation>()
                    .ForMember(x => x.LedgerId, o => o.Ignore())
                    .ForMember(x => x.CreatedAt, o => o.MapFrom<DateTimeUtcNowResolver<LedgerOperationCreateDto, LedgerOperation>>())
                    .ForMember(x => x.CreatorId, o => o.MapFrom<CurrentUserIdResolver<LedgerOperationCreateDto, LedgerOperation>>())
                    .ForSourceMember(x => x.LedgerVersion, o => o.DoNotValidate());

                // update scenario

                m.CreateMap<LedgerUpdateDto, Ledger>(MemberList.Source)
                    // not setting version in the current values, should set it to original values
                    .ForMember(x => x.Version, o => o.Ignore());

                // common

                m.CreateMap<LedgerMemberDto, LedgerMember>()
                    .ForMember(m => m.LedgerId, o => o.Ignore());
                m.CreateMap<OperationMemberDto, OperationMember>()
                    .ForMember(m => m.OperationId, o => o.Ignore())
                    .ForMember(m => m.Side, o => o.ConvertUsing<OperationSideDto>(new OperationSideConverter()));
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

        public void Map<TSource, TTarget>(TSource source, TTarget target)
        {
            _internalMapper.Map(source, target);
        }
    }

    public class OperationSideConverter : IValueConverter<OperationSide, OperationSideDto>, IValueConverter<OperationSideDto, OperationSide>
    {
        public OperationSideDto Convert(OperationSide sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case OperationSide.Creditor:
                    return OperationSideDto.Creditor;
                case OperationSide.Debtor:
                    return OperationSideDto.Debtor;
                default:
                    throw new InvalidOperationException($"Unknown value {sourceMember} of enum {nameof(OperationSide)}");
            }
        }

        public OperationSide Convert(OperationSideDto sourceMember, ResolutionContext context)
        {
            switch (sourceMember)
            {
                case OperationSideDto.Creditor:
                    return OperationSide.Creditor;
                case OperationSideDto.Debtor:
                    return OperationSide.Debtor;
                default:
                    throw new InvalidOperationException($"Unknown value {sourceMember} of enum {nameof(OperationSideDto)}");
            }
        }
    }

    public class CurrentUserIdResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, long>
    {
        private readonly ISecurityContext _ctx;

        public CurrentUserIdResolver(ISecurityContext ctx)
        {
            _ctx = ctx;
        }

        public long Resolve(TSource source, TDestination destination, long destMember, ResolutionContext context)
        {
            return _ctx.CurrentUserId;
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

using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.WebApi.Accounting.Dto;
using Calca.WebApi.Authorization;
using Calca.WebApi.Filters;
using Calca.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting
{
    // TODO: validation

    [Authorize]
    [Route("ledgers")]
    [LedgerConcurrencyConflictFilter]
    public class LedgerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDtoMapper _mapper;
        private readonly IAccountingService _accService;
        private readonly IAuthorizationService _authService;

        public LedgerController(IUnitOfWork unitOfWork, IDtoMapper mapper, IAccountingService accService, IAuthorizationService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accService = accService;
            _authService = authService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerView);
            if (!authz.Succeeded)
                return NotFound();

            var ledgerDto = _mapper.Map<Ledger, LedgerReadDto>(ledger);
            return Ok(ledgerDto);
        }

        [HttpGet("created-by-me")]
        public async Task<IActionResult> GetLedgersCreatedByMe(CancellationToken ct)
        {
            var ledgerItems = await _accService.GetLedgersCreatedByUser(User.GetUserId(), ct);
            var dtos = _mapper.Map<IReadOnlyList<LedgerListItem>, IReadOnlyList<LedgerListItemDto>>(ledgerItems);
            return Ok(dtos);
        }

        [HttpGet("with-me")]
        public async Task<IActionResult> GetLedgersByParticipation(CancellationToken ct)
        {
            var ledgerItems = await _accService.GetLedgersWithUserMember(User.GetUserId(), ct);
            var dtos = _mapper.Map<IReadOnlyList<LedgerListItem>, IReadOnlyList<LedgerListItemDto>>(ledgerItems);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]LedgerCreateDto ledgerDto, CancellationToken ct)
        {
            var ledger = _mapper.Map<LedgerCreateDto, Ledger>(ledgerDto);
            var id = await _accService.CreateLedger(ledger, ct);
            await _unitOfWork.Commit(ct);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody]LedgerUpdateDto ledgerDto, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerMetaEdit);
            if (!authz.Succeeded)
                return NotFound();

            _mapper.Map(ledgerDto, ledger);

            await _accService.UpdateLedger(ledger, ledgerDto.Version, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }

        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerView);
            if (!authz.Succeeded)
                return NotFound();

            var operations = await _accService.GetOperations(id, ct);
            var operationsDto = _mapper
                .Map<IReadOnlyList<LedgerOperation>, IReadOnlyList<LedgerOperationReadDto>>(operations);
            return Ok(operationsDto);
        }

        [HttpPost("{id}/operations")]
        public async Task<IActionResult> RegisterOperation(
            long id, 
            [FromBody] LedgerOperationCreateDto operationDto, 
            CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerOperationsEdit);
            if (!authz.Succeeded)
                return NotFound();

            var operation = _mapper.Map<LedgerOperationCreateDto, LedgerOperation>(operationDto);
            operation.LedgerId = id;
            await _accService.RegisterOperation(operation, operationDto.LedgerVersion, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }

        [HttpPost("{ledgerId}/cancellations")]
        public async Task<IActionResult> CancelOperation(
            long ledgerId,
            [FromBody] OperationCancellationDto cancellation,
            CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(ledgerId, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerOperationsEdit);
            if (!authz.Succeeded)
                return NotFound();

            await _accService.CancelOperation(ledgerId, cancellation.OperationId, cancellation.LedgerVersion, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }

        [HttpGet("{ledgerId}/balance-sheet")]
        public async Task<IActionResult> GetBalanceSheet(long ledgerId, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(ledgerId, ct);
            if (ledger == null)
                return NotFound();

            var authz = await _authService.AuthorizeAsync(User, ledger, KnownAuthzPolicies.AllowLedgerView);
            if (!authz.Succeeded)
                return NotFound();

            var bs = await _accService.GetBalanceSheet(ledgerId, ct);
            var bsDto = BalanceSheetDto.FromModel(bs);
            return Ok(bsDto);
        }
    }
}

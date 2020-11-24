using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure;
using Calca.WebApi.Accounting.Dto;
using Calca.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting
{
    // TODO: authentication, validation

    [Route("ledgers")]
    [LedgerConcurrencyConflictFilter]
    public class LedgerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDtoMapper _mapper;
        private readonly IAccountingService _accService;

        public LedgerController(IUnitOfWork unitOfWork, IDtoMapper mapper, IAccountingService accService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accService = accService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();
            var ledgerDto = _mapper.Map<Ledger, LedgerReadDto>(ledger);
            return Ok(ledgerDto);
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
            _mapper.Map(ledgerDto, ledger);

            await _accService.UpdateLedger(ledger, ledgerDto.Version, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }

        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id, CancellationToken ct)
        {
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
            await _accService.CancelOperation(ledgerId, cancellation.OperationId, cancellation.LedgerVersion, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }
    }
}

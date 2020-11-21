using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure;
using Calca.WebApi.Accounting.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting
{
    [Route("ledgers")]
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

        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id, CancellationToken ct)
        {
            var operations = await _accService.GetOperations(id, ct);
            var operationsDto = _mapper
                .Map<IReadOnlyList<LedgerOperation>, IReadOnlyList<LedgerOperationDto>>(operations);
            return Ok(operationsDto);
        }

        [HttpPost("{id}/operations")]
        public async Task<IActionResult> RegisterOperation(
            long id, 
            [FromHeader(Name = "x-ledger-version")] long ledgerVersion,
            [FromBody] LedgerOperationCreateDto operationDto, 
            CancellationToken ct)
        {
            var operation = _mapper.Map<LedgerOperationCreateDto, LedgerOperation>(operationDto);
            operation.LedgerId = id;
            await _accService.RegisterOperation(operation, ledgerVersion, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }
    }
}

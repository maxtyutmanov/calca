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
        private readonly IAccountingService _accService;

        public LedgerController(IUnitOfWork unitOfWork, IAccountingService accService)
        {
            _unitOfWork = unitOfWork;
            _accService = accService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var ledger = await _accService.GetLedger(id, ct);
            if (ledger == null)
                return NotFound();
            var ledgerDto = Mapper.Map(ledger);
            return Ok(ledgerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]LedgerDto ledgerDto, CancellationToken ct)
        {
            var ledger = Mapper.Map(ledgerDto);
            var id = await _accService.CreateLedger(ledger, ct);
            await _unitOfWork.Commit(ct);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}/operations")]
        public async Task<IActionResult> GetOperations(long id, CancellationToken ct)
        {
            var operations = await _accService.GetOperations(id, ct);
            var operationsDto = Mapper.Map(operations);
            return Ok(operationsDto);
        }

        [HttpPost("{id}/operations")]
        public async Task<IActionResult> RegisterOperation(
            long id, 
            [FromHeader(Name = "x-ledger-version")] long ledgerVersion,
            [FromBody] LedgerOperationDto operationDto, 
            CancellationToken ct)
        {
            var operation = Mapper.Map(operationDto);
            operation.LedgerId = id;
            await _accService.RegisterOperation(operation, ledgerVersion, ct);
            await _unitOfWork.Commit(ct);
            return Ok();
        }
    }
}

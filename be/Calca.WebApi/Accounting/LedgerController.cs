﻿using Calca.Domain;
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

        public LedgerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var repo = _unitOfWork.GetLedgerRepository();
            var ledger = await repo.GetById(id, ct);
            throw new NotImplementedException();
        }
    }
}

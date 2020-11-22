using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class LedgerUpdateDto : LedgerCreateDto
    {
        public long Version { get; set; }
    }
}

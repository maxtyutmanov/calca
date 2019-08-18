using Calca.Model;
using Calca.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.Controllers
{
    [Route("/api/trans")]
    public class TranLogController : Controller
    {
        private readonly FileStorage _storage;

        public TranLogController(FileStorage storage)
        {
            _storage = storage;
        }

        [HttpPost]
        public Tran Post([FromBody] Tran tran)
        {
            var modified = _storage.AppendTran(tran);
            return modified;
        }

        [HttpGet("{collectionId}")]
        public List<Tran> GetAll(string collectionId)
        {
            var trans = _storage.ReadAllTrans(collectionId);
            return trans;
        }
    }
}

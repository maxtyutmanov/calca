using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Authorization
{
    public static class KnownAuthzPolicies
    {
        public const string AllowLedgerView = "AllowLedgerView";

        public const string AllowLedgerMetaEdit = "AllowLedgerMetaEdit";

        public const string AllowLedgerOperationsEdit = "AllowLedgerOperationsEdit";
    }
}

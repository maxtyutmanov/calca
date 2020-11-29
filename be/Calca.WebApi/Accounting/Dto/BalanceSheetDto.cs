using Calca.Domain.Accounting;
using Calca.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class BalanceSheetItemDto
    {
        public long UserId { get; set; }
        public decimal Balance { get; set; }
    }

    public class BalanceSheetDto
    {
        public List<BalanceSheetItemDto> Items { get; set; }

        public static BalanceSheetDto FromModel(BalanceSheet bs)
        {
            var items = new List<BalanceSheetItemDto>(bs.Items.Count);
            foreach (var pair in bs.Items)
            {
                var member = pair.Key;
                var balance = pair.Value;
                items.Add(new BalanceSheetItemDto() { UserId = member.UserId, Balance = balance });
            }
            return new BalanceSheetDto() { Items = items };
        }
    }
}

using Calca.Domain.Accounting;

namespace Calca.WebApi.Accounting.Dto
{
    public enum OperationSideDto
    {
        Creditor = 1,
        Debtor = 2
    }

    public class OperationMemberDto
    {
        public long UserId { get; set; }

        // TODO: DTO?

        public OperationSideDto Side { get; set; }
    }
}
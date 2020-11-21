using Calca.Domain.Accounting;

namespace Calca.WebApi.Accounting.Dto
{
    public class OperationMemberDto
    {
        public long UserId { get; set; }

        // TODO: DTO?

        public OperationSide Side { get; set; }
    }
}
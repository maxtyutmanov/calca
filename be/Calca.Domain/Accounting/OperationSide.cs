using Calca.Domain.Errors;

namespace Calca.Domain.Accounting
{
    public enum OperationSide
    {
        Creditor = 1,
        Debtor = 2
    }

    public static class OperationSideExt
    {
        public static OperationSide Reverse(this OperationSide side)
        {
            switch (side)
            {
                case OperationSide.Creditor:
                    return OperationSide.Debtor;
                case OperationSide.Debtor:
                    return OperationSide.Creditor;
                default:
                    throw side.IsUnknown();
            }
        }
    }
}

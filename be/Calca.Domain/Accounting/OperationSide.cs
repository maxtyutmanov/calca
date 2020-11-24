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
            // TODO: more robust
            if (side == OperationSide.Creditor) return OperationSide.Debtor;
            else return OperationSide.Creditor;
        }
    }
}

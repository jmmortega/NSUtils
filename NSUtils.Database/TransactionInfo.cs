using System;

namespace NSUtils.Database
{
    public class TransactionInfo
    {
        public TransactionInfo()
        { }

        public TransactionInfo(int rowAffected)
        {
            RowAffected = rowAffected;
        }

        public TransactionInfo(Exception exception)
        {
            Exception = exception;
        }

        public int RowAffected { get; private set; }
        public Exception Exception { get; private set; }
    }
}

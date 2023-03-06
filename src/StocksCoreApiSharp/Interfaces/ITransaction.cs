using AndreasReitberger.Stocks.Enums;

namespace AndreasReitberger.Stocks.Interfaces
{
    public interface ITransaction
    {
        #region Properties
        Guid Id { get; set; }

        Guid StockId { get; set; }

        TransactionType Type { get; set; }

        DateTimeOffset? DateOfCreation { get; set; }

        double Amount { get; set; }

        double Price { get; set; }

        public double Total { get; }
        #endregion
    }
}

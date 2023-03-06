namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IStockPriceRange
    {
        #region Properties
        Guid Id { get; set; }

        Guid StockId { get; set; }

        DateTimeOffset Date { get; set; }

        double Open { get; set; }

        double Close { get; set; }

        double High { get; set; }

        double Low { get; set; }

        double Volume { get; set; }
        #endregion
    }
}

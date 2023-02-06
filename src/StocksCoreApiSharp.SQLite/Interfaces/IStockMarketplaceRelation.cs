namespace AndreasReitberger.Stocks.SQLite.Interfaces
{
    public interface IStockMarketplaceRelation
    {
        #region Methods
        Guid StockId { get; set; }

        Guid MarketplaceId { get; set; }
        #endregion
    }
}

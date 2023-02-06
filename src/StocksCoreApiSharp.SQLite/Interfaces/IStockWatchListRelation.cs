namespace AndreasReitberger.Stocks.SQLite.Interfaces
{
    public interface IStockWatchListRelation
    {
        #region Methods
        Guid StockId { get; set; }

        Guid WatchListId { get; set; }
        #endregion
    }
}

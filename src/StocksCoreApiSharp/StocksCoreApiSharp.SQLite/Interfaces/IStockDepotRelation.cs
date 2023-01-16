namespace AndreasReitberger.Stocks.SQLite.Interfaces
{
    public interface IStockDepotRelation
    {
        #region Methods
        Guid StockId { get; set; }

        Guid DepotId { get; set; }
        #endregion
    }
}

using System;

namespace AndreasReitberger.Stocks.Models.Events
{
    public class StockChangedEventArgs : EventArgs
    {
        #region Properties
        public Guid? Id { get; set; }
        public Guid? DepotId { get; set; }
        public string? Name { get; set; }
        #endregion
    }
}

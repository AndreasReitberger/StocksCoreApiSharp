using System;

namespace AndreasReitberger.Stocks.Models.Events
{
    public class MarketplaceChangedEventArgs : EventArgs
    {
        #region Properties
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        #endregion
    }
}

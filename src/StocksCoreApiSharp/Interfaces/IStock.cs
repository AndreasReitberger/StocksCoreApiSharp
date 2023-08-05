using AndreasReitberger.Stocks.Enums;

namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IStock
    {
        #region Properties
        Guid Id { get; set; }

        Guid DepotId { get; set; }

        Guid WatchListId { get; set; }

        Guid MarketplaceId { get; set; }

        string Name { get; set; }

        string Symbol { get; set; }

        string ISIN { get; set; }

        string WKN { get; set; }

        string Marketplace { get; set; }

        bool IsRerfresing { get; set; }

        DateTimeOffset? LastRefresh { get; set; }

        double? DividendForecast { get; set; }

        DateTimeOffset? DateOfAGM { get; set; }

        string Currency { get; set; }

        double? CurrentRate { get; set; }

        bool RespectDividendsForEntrancePrice { get; set; }

        double Quantity { get; set; }

        double EntrancePrice { get; set; }

        double TotalCosts { get; set; }

        double TotalDividends { get; set; }

        double Growth { get; set; }

        double GrowthPercentage { get; set; }

        double CurrentWorth { get; set; }

        double Volume { get; set; }

        double? PriceOpen { get; set; }

        double? PriceClose { get; set; }

        double? Change { get; set; }

        ValueChangedIndicator ChangedIndicator { get; set; }

        public bool PositiveGrowth { get; }
        public bool CostEarnBreakPointReached { get; }
        #endregion

        #region Collections
        /*
        ObservableCollection<IStockPriceRange> PriceRanges { get; set; }
        ObservableCollection<ITransaction> Transactions { get; set; }
        ObservableCollection<IDividend> Dividends { get; set; }
        */

        #endregion

        #region Methods

        void Refresh();

        #endregion
    }
}

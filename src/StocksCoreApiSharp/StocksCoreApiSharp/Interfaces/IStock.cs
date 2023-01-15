using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Models;
using AndreasReitberger.Stocks.Models.Additions;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;
using System.Transactions;

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

        DateTime? LastRefresh { get; set; }

        double? DividendForecast { get; set; }

        DateTime? DateOfAGM { get; set; }

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

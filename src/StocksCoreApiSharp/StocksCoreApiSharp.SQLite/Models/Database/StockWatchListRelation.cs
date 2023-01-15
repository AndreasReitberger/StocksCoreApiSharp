using AndreasReitberger.Stocks.SQLite.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.SQLite.Database
{
    [Table(nameof(StockWatchListRelation) + "s")]
    public partial class StockWatchListRelation : ObservableObject, IStockWatchListRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(WatchList))]
        Guid watchListId;
    }
}
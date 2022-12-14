#if SQLite
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace AndreasReitberger.Stocks.Models.Database
{
    [Table(nameof(StockWatchListRelation) + "s")]
    [ObservableObject]
    public partial class StockWatchListRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(WatchList))]
        Guid watchListId;
    }
}
#endif
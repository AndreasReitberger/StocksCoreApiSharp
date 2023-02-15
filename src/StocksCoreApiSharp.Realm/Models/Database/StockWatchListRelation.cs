using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.Stocks.Realm.Database
{
    [Obsolete]
    public partial class StockWatchListRelation : ObservableObject
    {
        [ObservableProperty]
        //[property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        //[property: ForeignKey(typeof(WatchList))]
        Guid watchListId;
    }
}
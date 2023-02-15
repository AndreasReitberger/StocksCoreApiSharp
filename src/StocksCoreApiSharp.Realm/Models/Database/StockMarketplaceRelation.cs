using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.Stocks.Realm.Database
{
    [Obsolete]
    public partial class StockMarketplaceRelation : ObservableObject
    {
        [ObservableProperty]
        //[property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        //[property: ForeignKey(typeof(Marketplace))]
        Guid marketplaceId;
    }
}
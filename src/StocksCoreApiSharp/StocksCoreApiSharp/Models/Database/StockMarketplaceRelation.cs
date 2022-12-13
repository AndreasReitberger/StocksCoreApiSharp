#if SQLite
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.Models.Database
{
    [Table(nameof(StockMarketplaceRelation) + "s")]
    [ObservableObject]
    public partial class StockMarketplaceRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(Marketplace))]
        Guid marketplacetId;
    }
}
#endif
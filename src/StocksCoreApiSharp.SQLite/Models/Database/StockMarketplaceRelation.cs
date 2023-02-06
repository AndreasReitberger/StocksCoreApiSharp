using AndreasReitberger.Stocks.SQLite.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.SQLite.Database
{
    [Table(nameof(StockMarketplaceRelation) + "s")]
    public partial class StockMarketplaceRelation : ObservableObject, IStockMarketplaceRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(Marketplace))]
        Guid marketplaceId;
    }
}
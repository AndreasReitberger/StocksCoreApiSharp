using AndreasReitberger.Stocks.SQLite.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.SQLite.Database
{
    [Table("StockDepot")]
    public partial class StockDepotRelation : ObservableObject, IStockDepotRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(Depot))]
        Guid depotId;
    }
}

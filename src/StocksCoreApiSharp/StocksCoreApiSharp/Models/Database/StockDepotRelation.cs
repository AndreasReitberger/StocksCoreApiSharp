#if SQLite
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.Models.Database
{
    [Table("StockDepot")]
    [ObservableObject]
    public partial class StockDepotRelation
    {
        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        [property: ForeignKey(typeof(Depot))]
        Guid depotId;
    }
}
#endif
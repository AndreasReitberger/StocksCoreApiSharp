using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.Stocks.Realm.Database
{
    [Obsolete]
    public partial class StockDepotRelation : ObservableObject//, IStockDepotRelation
    {
        [ObservableProperty]
        Guid stockId;

        [ObservableProperty]
        Guid depotId;
    }
}

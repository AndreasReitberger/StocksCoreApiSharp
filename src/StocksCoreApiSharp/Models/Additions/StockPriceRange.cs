
using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
namespace AndreasReitberger.Stocks.Models.Additions
{
    [ObservableObject]
    public partial class StockPriceRange : IStockPriceRange
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        Guid stockId = Guid.Empty;

        [ObservableProperty]
        DateTimeOffset date;

        [ObservableProperty]
        double open;

        [ObservableProperty]
        double close;

        [ObservableProperty]
        double high;

        [ObservableProperty]
        double low;

        [ObservableProperty]
        double volume;
        #endregion

        #region Constructor
        public StockPriceRange()
        {
            Id = Guid.NewGuid();
        }
        public StockPriceRange(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Overrides

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

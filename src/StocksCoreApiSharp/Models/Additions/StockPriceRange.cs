
using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
namespace AndreasReitberger.Stocks.Models.Additions
{
    [ObservableObject]
    public partial class StockPriceRange
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        Guid stockId = Guid.Empty;

        [ObservableProperty]
        DateTime date;

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

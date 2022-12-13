
using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif

namespace AndreasReitberger.Stocks.Models.Additions
{
#if SQLite
    [Table(nameof(StockPriceRange) + "s")]
#endif
    [ObservableObject]
    public partial class StockPriceRange
    {
        #region Properties
        [ObservableProperty]
#if SQLite
        [property: PrimaryKey]
#endif
        Guid id = Guid.Empty;

        [ObservableProperty]
#if SQLite
        [property: ForeignKey(typeof(Depot))]
#endif
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

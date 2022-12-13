using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
#if SQLite
using SQLite;
#endif

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Dividend) + "s")]
#endif
    [ObservableObject]
    public partial class Dividend// : BaseModel
    {
        #region Properties
        [ObservableProperty]
#if SQLite
        [property: PrimaryKey]
#endif
        Guid id = Guid.Empty;

        [ObservableProperty]
#if SQLite
        [property: ForeignKey(typeof(Stock))]
#endif
        Guid stockId;

        [ObservableProperty]
        DateTime? dateOfDividend;

        [ObservableProperty]
        double quantity = 0;

        [ObservableProperty]
        double amountOfDividend = 0;

        [ObservableProperty]
        double tax = 0;

        public double Total => AmountOfDividend - Tax;
        public double TaxPercentage => Tax / (AmountOfDividend / 100);
        public double Margin => 100 - TaxPercentage;
        #endregion

        #region Constructor
        public Dividend()
        {         
            Id = Guid.NewGuid();
        }
        public Dividend(Guid id)
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

using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Dividend : ObservableObject, IDividend
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        Guid stockId;

        [ObservableProperty]
        DateTimeOffset? dateOfDividend;

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

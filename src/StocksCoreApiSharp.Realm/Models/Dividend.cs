using AndreasReitberger.Stocks.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class Dividend : RealmObject, IDividend
    {
        #region Properties
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        //[property: ForeignKey(typeof(Stock))]
        public Guid StockId { get; set; }

        public DateTimeOffset? DateOfDividend { get; set; }

        public double Quantity { get; set; } = 0;

        public double AmountOfDividend { get; set; } = 0;

        public double Tax { get; set; } = 0;

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

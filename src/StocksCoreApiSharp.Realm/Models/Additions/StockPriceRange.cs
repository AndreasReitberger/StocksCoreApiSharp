using AndreasReitberger.Stocks.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm.Additions
{
    public partial class StockPriceRange : RealmObject, IStockPriceRange
    {
        #region Properties
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        //[property: ForeignKey(typeof(Depot))]
        public Guid StockId { get; set; } = Guid.Empty;

        public DateTimeOffset Date { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Volume { get; set; }
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

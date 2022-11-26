
using AndreasReitberger.Core.Utilities;
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
    public partial class StockPriceRange : BaseModel
    {
        #region Properties
        Guid id = Guid.Empty;
#if SQLite
        [PrimaryKey]
#endif
        public Guid Id
        {
            get => id;
            set
            {
                if (id == value)
                    return;
                id = value;
                OnPropertyChanged();
            }
        }

        Guid stockId = Guid.Empty;
#if SQLite
        [ForeignKey(typeof(Depot))]
#endif
        public Guid StockId
        {
            get => stockId;
            set
            {
                if (stockId == value)
                    return;
                stockId = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date { get; set; }
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

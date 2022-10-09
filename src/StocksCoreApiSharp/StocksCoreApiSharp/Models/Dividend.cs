using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Dividend) + "s")]
#endif
    public partial class Dividend : BaseModel
    {
        #region Properties
        [JsonProperty(nameof(Id))]
        Guid id = Guid.Empty;
        [JsonIgnore]
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
        
        Guid stockId;
#if SQLite
        [ForeignKey(typeof(Stock))]
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

        DateTime? dateOfCreation = null;
        public DateTime? DateOfDividend
        {
            get => dateOfCreation;
            set
            {
                if (dateOfCreation == value)
                    return;
                dateOfCreation = value;
                OnPropertyChanged();
            }
        }

        double quantity = 0;
        public double Quantity
        {
            get => quantity;
            set
            {
                if (quantity == value)
                    return;
                quantity = value;
                OnPropertyChanged();
            }
        }

        double amountofDividend = 0;
        public double AmountOfDividend
        {
            get => amountofDividend;
            set
            {
                if (amountofDividend == value)
                    return;
                amountofDividend = value;
                OnPropertyChanged();
            }
        }

        double tax = 0;
        public double Tax
        {
            get => tax;
            set
            {
                if (tax == value)
                    return;
                tax = value;
                OnPropertyChanged();
            }
        }

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

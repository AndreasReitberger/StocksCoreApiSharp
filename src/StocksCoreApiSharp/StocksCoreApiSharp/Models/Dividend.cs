using AndreasReitberger.Core.Utilities;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Dividend : BaseModel
    {
        #region Properties
        Guid id = Guid.Empty;
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
        
        Guid? stockId;
        public Guid? StockId
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
    }
}

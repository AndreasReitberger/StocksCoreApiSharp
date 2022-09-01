using AndreasReitberger.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Dividend : BaseModel
    {
        #region Properties
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
        #endregion
    }
}

using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Transaction : BaseModel
    {
        #region Properties

        TransactionType? type = null;
        public TransactionType? Type
        {
            get => type;
            set
            {
                if (type == value)
                    return;
                type = value;
                OnPropertyChanged();
            }
        }

        DateTime? dateOfCreation = null;
        public DateTime? DateOfCreation
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

        double amount = 0;
        public double Amount
        {
            get => amount;
            set
            {
                if (amount == value)
                    return;
                amount = value;
                OnPropertyChanged();
            }
        }

        double price = 0;
        public double Price
        {
            get => price;
            set
            {
                if (price == value)
                    return;
                price = value;
                OnPropertyChanged();
            }
        }

        public double Total => Math.Round(Amount * Price, 2); 
        #endregion

    }
}

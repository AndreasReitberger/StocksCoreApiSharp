﻿using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Transaction : BaseModel
    {
        #region Properties
        [JsonProperty(nameof(Id))]
        Guid id = Guid.Empty;
        [JsonIgnore]
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

        #region Constructor
        public Transaction()
        {
            Id = Guid.NewGuid();
        }
        public Transaction(Guid id)
        {
            Id = id;
        }
        #endregion
    }
}

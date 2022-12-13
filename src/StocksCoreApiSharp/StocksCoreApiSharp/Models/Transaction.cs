using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Transaction) + "s")]
#endif
    [ObservableObject]
    public partial class Transaction
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
        TransactionType? type = null;

        [ObservableProperty]
        DateTime? dateOfCreation = null;

        [ObservableProperty]
        double amount = 0;

        [ObservableProperty]
        double price = 0;

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

        #region Overrides

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

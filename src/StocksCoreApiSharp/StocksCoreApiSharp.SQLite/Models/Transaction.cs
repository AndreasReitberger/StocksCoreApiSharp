using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.SQLite
{

    [Table(nameof(Transaction) + "s")]

    public partial class Transaction : ObservableObject, ITransaction
    {
        #region Properties

        [ObservableProperty]

        [property: PrimaryKey]

        Guid id = Guid.Empty;

        [ObservableProperty]

        [property: ForeignKey(typeof(Stock))]

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

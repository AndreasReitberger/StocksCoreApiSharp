using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Interfaces;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class Transaction : RealmObject, ITransaction
    {
        #region Properties

        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        //[property: ForeignKey(typeof(Stock))]
        public Guid StockId { get; set; }

        public TransactionType Type
        {
            get => (TransactionType)TypeId;
            set { TypeId = (int)value; }
        }
        public int TypeId { get; set; }

        public DateTimeOffset? DateOfCreation { get; set; } = null;

        public double Amount { get; set; } = 0;

        public double Price { get; set; } = 0;

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

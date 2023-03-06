using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Events;
using AndreasReitberger.Stocks.Realm.Additions;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class Stock : RealmObject, IStock
    {
        #region Properties
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        //[ForeignKey(typeof(Depot))]
        public Guid DepotId { get; set; } = Guid.Empty;

        //[ForeignKey(typeof(WatchList))]
        public Guid WatchListId { get; set; } = Guid.Empty;

        //[ForeignKey(typeof(Marketplace))]
        public Guid MarketplaceId { get; set; } = Guid.Empty;

        public string Name { get; set; } = "";

        public string Symbol { get; set; } = "";

        public string ISIN { get; set; } = "";

        public string WKN { get; set; } = "";

        public string Marketplace { get; set; } = "";

        public bool IsRerfresing { get; set; } = false;

        public DateTimeOffset? LastRefresh { get; set; }

        public double? DividendForecast { get; set; }

        public DateTimeOffset? DateOfAGM { get; set; }

        public string Currency { get; set; }

        double? currentRate { get; set; }
        public double? CurrentRate
        {
            get => currentRate;
            set
            {
                currentRate = value;
                OnCurrentRateChanged(value);
            }
        }
        void OnCurrentRateChanged(double? value)
        {
            NotifyListeners();
            UpdateChangedIndicator();
        }

        bool respectDividendsForEntrancePrice { get; set; } = true;
        public bool RespectDividendsForEntrancePrice
        {
            get => respectDividendsForEntrancePrice;
            set
            {
                respectDividendsForEntrancePrice = value;
                OnRespectDividendsForEntrancePriceChanged(value);
            }
        }
        void OnRespectDividendsForEntrancePriceChanged(bool value)
        {
            NotifyListeners();
        }

        public double Quantity { get; set; } = 0;

        double entrancePrice { get; set; } = 0;
        public double EntrancePrice
        {
            get => entrancePrice;
            set
            {
                entrancePrice = value;
                OnEntrancePriceChanged(value);
            }
        }
        void OnEntrancePriceChanged(double value)
        {
            NotifyListeners();
            UpdateChangedIndicator();
        }

        //[NotifyPropertyChangedFor(nameof(PositiveGrowth))]
        double totalCosts { get; set; } = 0;
        public double TotalCosts
        {
            get => totalCosts;
            set
            {
                totalCosts = value;
                OnTotalCostsChanged(value);
            }
        }
        void OnTotalCostsChanged(double value)
        {
            NotifyListeners();
        }

        public double TotalDividends { get; set; } = 0;

        public double Growth { get; set; } = 0;

        public double GrowthPercentage { get; set; } = 0;


        //[NotifyPropertyChangedFor(nameof(PositiveGrowth))]
        double currentWorth { get; set; } = 0;
        public double CurrentWorth
        {
            get => currentWorth;
            set
            {
                currentWorth = value;
                OnCurrentWorthChanged(value);
            }
        }
        void OnCurrentWorthChanged(double value)
        {
            NotifyListeners();
        }

        public double Volume { get; set; } = 0;

        [Ignored]
        double? priceOpen { get; set; } = 0;
        public double? PriceOpen
        {
            get => priceOpen;
            set
            {
                priceOpen = value;
                OnPriceOpenChanged(value);
            }
        }
        void OnPriceOpenChanged(double? value)
        {
            NotifyListeners();
        }

        [Ignored]
        double? priceClose { get; set; } = 0;
        public double? PriceClose
        {
            get => priceClose;
            set
            {
                priceClose = value;
                OnPriceCloseChanged(value);
            }
        }
        void OnPriceCloseChanged(double? value)
        {
            NotifyListeners();
        }

        [Ignored]
        double? change { get; set; } = 0;
        public double? Change
        {
            get => change;
            set
            {
                change = value;
                OnChangeChanged(value);
            }
        }
        void OnChangeChanged(double? value)
        {
            NotifyListeners();
        }


        [Ignored]
        ValueChangedIndicator changedIndicator { get; set; } = ValueChangedIndicator.Unchanged;
        public ValueChangedIndicator ChangedIndicator
        {
            get => changedIndicator;
            set
            {
                changedIndicator = value;
                OnChangedIndicatorChanged(value);
            }
        }
        void OnChangedIndicatorChanged(ValueChangedIndicator value)
        {
            NotifyListeners();
        }

        [Ignored]
        public bool PositiveGrowth => TotalCosts <= CurrentWorth;

        #endregion

        #region Collections

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        public IList<Transaction> Transactions { get; }
        //ObservableCollection<ITransaction> transactions = new();

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        public IList<Dividend> Dividends { get; }
        //ObservableCollection<IDividend> dividends = new();

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        public IList<StockPriceRange> PriceRanges { get; }
        //ObservableCollection<IStockPriceRange> priceRanges = new();
        void OnPriceRangesChanged() => UpdateOpenClosePrices();

        #endregion

        #region Constructor
        public Stock()
        {
            Id = Guid.NewGuid();
            //Dividends.AsRealmCollection().CollectionChanged += Dividends_CollectionChanged;
            //Transactions.AsRealmCollection().CollectionChanged += Transactions_CollectionChanged;
            //PriceRanges.AsRealmCollection().CollectionChanged += PriceRanges_CollectionChanged;
        }

        public Stock(Guid id)
        {
            Id = id;
            //Dividends.AsRealmCollection().CollectionChanged += Dividends_CollectionChanged;
            //Transactions.AsRealmCollection().CollectionChanged += Transactions_CollectionChanged;
            //PriceRanges.AsRealmCollection().CollectionChanged += PriceRanges_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Stock()
        {
            //Dividends.AsRealmCollection().CollectionChanged -= Dividends_CollectionChanged;
            //Transactions.AsRealmCollection().CollectionChanged -= Transactions_CollectionChanged;
            //PriceRanges.AsRealmCollection().CollectionChanged -= PriceRanges_CollectionChanged;
        }
        #endregion

        #region EventHandlers
        public event EventHandler Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(System.IO.ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(Realms.ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }

        public event EventHandler<StockChangedEventArgs> StockChanged;
        protected virtual void OnStockChanged(StockChangedEventArgs e)
        {
            StockChanged?.Invoke(this, e);
        }
        #endregion

        #region Events
        void Transactions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            NotifyListeners();
        }

        void Dividends_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            NotifyListeners();
        }

        void PriceRanges_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPriceRangesChanged();
        }
        #endregion

        #region Methods
        void NotifyListeners()
        {
            OnStockChanged(new()
            {
                Id = Id,
                DepotId = DepotId,
                Name = Name,
            });
        }

        void UpdateChangedIndicator()
        {
            if (CurrentRate == PriceOpen)
            {
                ChangedIndicator = ValueChangedIndicator.Unchanged;
            }
            else
            {
                ChangedIndicator = CurrentRate > PriceOpen ? ValueChangedIndicator.Increased : ValueChangedIndicator.Decreased;
            }
        }

        void UpdateOpenClosePrices()
        {
            StockPriceRange? priceRange = PriceRanges?.LastOrDefault();
            PriceClose = priceRange?.Close ?? CurrentRate;
            PriceOpen = priceRange?.Open ?? CurrentRate;
            // Change since the laste open
            Change = CurrentRate - PriceOpen;
            UpdateChangedIndicator();
        }

        public void Refresh()
        {
            Quantity = CalculateCurrentAmount();
            EntrancePrice = CalculateEntrancePrice();
            // Avoid negative costs, those will be respected in CurrentWorth calculation
            TotalCosts = Math.Clamp(CalculateTotalCosts(), 0, double.PositiveInfinity);
            TotalDividends = CalculateTotalDividends();
            CurrentWorth = CalculateCurrentWorth();

            Growth = CurrentWorth - TotalCosts;
            GrowthPercentage = CalculateGrowthPercentage();
        }

        double CalculateEntrancePrice()
        {
            var bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            //double? price = bought?.Select(transaction => transaction.Total).Sum();
            double? amount = bought?.Select(transaction => transaction.Amount).Sum();

            //double? entrancePrice = price / amount;
            double? entrancePrice = RespectDividendsForEntrancePrice ?
                (CalculateTotalCosts() - CalculateTotalDividends()) / amount :
                CalculateTotalCosts() / amount;

            return entrancePrice ?? 0;

        }

        double CalculateCurrentWorth()
        {
            /*
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            double? amount = bought?.Select(transaction => transaction.Amount).Sum();
            */
            double amount = CalculateCurrentAmount();
            double trading = CalculateTotalCosts();
            //double? entrancePrice = price / amount;
            double? currentWorth = RespectDividendsForEntrancePrice ?
                (amount * CurrentRate + CalculateTotalDividends()) :
                amount * CurrentRate;
            // Indicates a win in trading (sold with profits)
            if (trading < 0)
            {
                currentWorth += Math.Abs(trading);
            }
            return currentWorth ?? 0;

        }

        double CalculateTotalCosts()
        {
            var bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            var sold = Transactions?.Where(transaction => transaction.Type == TransactionType.Sell).ToList();

            double? boughtTotal = bought?.Select(transaction => transaction.Total).Sum();
            double? soldTotal = sold?.Select(transaction => transaction.Total).Sum();
            // Avoid negative returns
            return (boughtTotal - soldTotal) ?? 0;
        }

        double CalculateTotalDividends()
        {
            double? price = Dividends?.Select(transaction => (transaction.AmountOfDividend - transaction.Tax)).Sum();
            return price ?? 0;
        }

        double CalculateCurrentAmount()
        {
            double? currentAmount =
                Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).Select(transaction => transaction.Amount).Sum() -
                Transactions?.Where(transaction => transaction.Type == TransactionType.Sell).Select(transaction => transaction.Amount).Sum();
            return currentAmount ?? 0;
        }

        double CalculateGrowthPercentage()
        {
            double? currentAmount = CalculateCurrentAmount();
            double? currentWorth = CurrentRate * currentAmount + CalculateTotalDividends();

            // Avoid wrong calculations if the stock actually have been sold and no growth calculation is possible then.
            if (currentAmount == 0 || currentWorth == 0) return 0;

            double? growth = currentWorth / CalculateTotalCosts() * 100 - 100;
            return growth ?? 0;
        }

        #endregion

        #region Overrides
        protected override void OnManaged()
        {
            base.OnManaged();
            /*
            Dividends.AsRealmCollection().CollectionChanged += Dividends_CollectionChanged;
            Transactions.AsRealmCollection().CollectionChanged += Transactions_CollectionChanged;
            PriceRanges.AsRealmCollection().CollectionChanged += PriceRanges_CollectionChanged;
            */
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

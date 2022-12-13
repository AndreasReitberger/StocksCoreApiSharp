using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Models.Additions;
using AndreasReitberger.Stocks.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Stock) + "s")]
#endif
    [ObservableObject]
    public partial class Stock// : BaseModel
    {

        #region Properties
        [ObservableProperty]
#if SQLite
        [property: PrimaryKey]
#endif
        Guid id = Guid.Empty;

        [ObservableProperty]
#if SQLite
        [property: ForeignKey(typeof(Depot))]
#endif
        Guid depotId = Guid.Empty;

        [ObservableProperty]
#if SQLite
        [property: ForeignKey(typeof(WatchList))]
#endif
        Guid watchListId = Guid.Empty;

        [ObservableProperty]
#if SQLite
        [property: ForeignKey(typeof(Marketplace))]
#endif
        Guid marketplaceId = Guid.Empty;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        string symbol = "";

        [ObservableProperty]
        string iSIN = "";

        [ObservableProperty]
        string wKN = "";

        [ObservableProperty]
        string marketplace = "";

        [ObservableProperty]
        bool isRerfresing = false;

        [ObservableProperty]
        DateTime? lastRefresh;

        [ObservableProperty]
        double? dividendForecast;

        /*
        [ObservableProperty]
        Dictionary <int, double> dividendHistory;
        */

        [ObservableProperty]
        DateTime? dateOfAGM;

        [ObservableProperty]
        string currency;

        double? currentRate;
        public double? CurrentRate
        {
            get => currentRate;
            set
            {
                if (currentRate == value)
                    return;
                currentRate = value;
                OnPropertyChanged();
                NotifyListeners();
                UpdateChangedIndicator();
            }
        }

        bool respectDividendsForEntrancePrice = true;
        public bool RespectDividendsForEntrancePrice
        {
            get => respectDividendsForEntrancePrice;
            set
            {
                if (respectDividendsForEntrancePrice == value)
                    return;
                respectDividendsForEntrancePrice = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        [ObservableProperty]
        double quantity = 0;

        double entrancePrice = 0;
        public double EntrancePrice
        {
            get => entrancePrice;
            set
            {
                if (entrancePrice == value)
                    return;
                entrancePrice = value;
                OnPropertyChanged();
                NotifyListeners();
                UpdateChangedIndicator();
            }
        }

        double totalCosts = 0;
        public double TotalCosts
        {
            get => totalCosts;
            set
            {
                if (totalCosts == value)
                    return;
                totalCosts = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PositiveGrowth));
                NotifyListeners();
            }
        }

        [ObservableProperty]
        double totalDividends = 0;
        /*
        public double TotalDividends
        {
            get => totalDividends;
            set
            {
                if (totalDividends == value)
                    return;
                totalDividends = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }
        */

        [ObservableProperty]
        double growth = 0;
        /*
        public double Growth
        {
            get => growth;
            set
            {
                if (growth == value)
                    return;
                growth = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }
        */

        [ObservableProperty]
        double growthPercentage = 0;
        /*
        public double GrowthPercentage
        {
            get => growthPercentage;
            set
            {
                if (growthPercentage == value)
                    return;
                growthPercentage = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }
        */

        double currentWorth = 0;
        public double CurrentWorth
        {
            get => currentWorth;
            set
            {
                if (currentWorth == value)
                    return;
                currentWorth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PositiveGrowth));
                NotifyListeners();
            }
        }

        [ObservableProperty]
        double volume = 0;
        /*
        public double Volume
        {
            get => currentWorth;
            set
            {
                if (volume == value)
                    return;
                volume = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }
        */

        double? priceOpen = 0;
#if SQLite
        [Ignore]
#endif
        public double? PriceOpen
        {
            get => priceOpen;
            private set
            {
                if (priceOpen == value)
                    return;
                priceOpen = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        double? priceClose = 0;
#if SQLite
        [Ignore]
#endif
        public double? PriceClose
        {
            get => priceClose;
            private set
            {
                if (priceClose == value)
                    return;
                priceClose = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        double? change = 0;
#if SQLite
        [Ignore]
#endif
        public double? Change
        {
            get => change;
            private set
            {
                if (change == value)
                    return;
                change = value;
                OnPropertyChanged();
            }
        }

        ValueChangedIndicator changedIndicator = ValueChangedIndicator.Unchanged;
#if SQLite
        [Ignore]
#endif
        public ValueChangedIndicator ChangedIndicator
        {
            get => changedIndicator;
            private set
            {
                if (changedIndicator == value)
                    return;
                changedIndicator = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

#if SQLite
        [Ignore]
#endif
        public bool PositiveGrowth
        {
            get
            {
                return TotalCosts <= CurrentWorth;
            }
        }
        #endregion

        #region Collections

        ObservableCollection<Transaction> transactions = new();
#if SQLite
        [OneToMany(CascadeOperations = CascadeOperation.All)]
#endif
        public ObservableCollection<Transaction> Transactions
        {
            get => transactions;
            set
            {
                if (transactions == value)
                    return;
                transactions = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Dividend> dividends = new();
#if SQLite
        [OneToMany(CascadeOperations = CascadeOperation.All)]
#endif
        public ObservableCollection<Dividend> Dividends
        {
            get => dividends;
            set
            {
                if (dividends == value)
                    return;
                dividends = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<StockPriceRange> priceRanges = new();
#if SQLite
        [OneToMany(CascadeOperations = CascadeOperation.All)]
#endif
        public ObservableCollection<StockPriceRange> PriceRanges
        {
            get => priceRanges;
            set
            {
                if (priceRanges == value)
                    return;
                priceRanges = value;
                OnPropertyChanged();
                UpdateOpenClosePrices();
            }
        }
        #endregion

        #region Constructor
        public Stock()
        {
            Id = Guid.NewGuid();
            Dividends.CollectionChanged += Dividends_CollectionChanged;
            Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        public Stock(Guid id)
        {
            Id = id;
            Dividends.CollectionChanged += Dividends_CollectionChanged;
            Transactions.CollectionChanged += Transactions_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Stock()
        {
            Dividends.CollectionChanged -= Dividends_CollectionChanged;
            Transactions.CollectionChanged -= Transactions_CollectionChanged;
        }
        #endregion

        #region EventHandlers
        public event EventHandler Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(ErrorEventArgs e)
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
            var priceRange = PriceRanges?.LastOrDefault();
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
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();

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
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            List<Transaction>? sold = Transactions?.Where(transaction => transaction.Type == TransactionType.Sell).ToList();

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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

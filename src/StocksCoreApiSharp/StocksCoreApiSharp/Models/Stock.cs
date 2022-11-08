using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Stock) + "s")]
#endif
    public partial class Stock : BaseModel
    {

        #region Properties
        //[JsonProperty(nameof(Id))]
        Guid id = Guid.Empty;
        //[JsonIgnore]
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
                NotifyListeners();
            }
        }

        Guid depotId = Guid.Empty;
#if SQLite
        [ForeignKey(typeof(Depot))]
#endif
        public Guid DepotId
        {
            get => depotId;
            set
            {
                if (depotId == value)
                    return;
                depotId = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        Guid watchListId = Guid.Empty;
#if SQLite
        [ForeignKey(typeof(WatchList))]
#endif
        public Guid WatchListId
        {
            get => watchListId;
            set
            {
                if (watchListId == value)
                    return;
                watchListId = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        string symbol = "";
        public string Symbol
        {
            get => symbol;
            set
            {
                if (symbol == value)
                    return;
                symbol = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        string isin = "";
        public string ISIN
        {
            get => isin;
            set
            {
                if (isin == value)
                    return;
                isin = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        string wkn = "";
        public string WKN
        {
            get => wkn;
            set
            {
                if (wkn == value)
                    return;
                wkn = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        string marketplace = "";
        public string Marketplace
        {
            get => marketplace;
            set
            {
                if (marketplace == value)
                    return;
                marketplace = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        bool isRerfresing = false;
        public bool IsRerfresing
        {
            get => isRerfresing;
            set
            {
                if (isRerfresing == value)
                    return;
                isRerfresing = value;
                OnPropertyChanged();
            }
        }

        DateTime? lastRefresh;
        public DateTime? LastRefresh
        {
            get => lastRefresh;
            set
            {
                if (lastRefresh == value)
                    return;
                lastRefresh = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

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
                NotifyListeners();
            }
        }

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

        double totalDividends = 0;
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

        double growth = 0;
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

        double growthPercentage = 0;
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

        double volume = 0;
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

        double priceOpen = 0;
        public double PriceOpen
        {
            get => priceOpen;
            set
            {
                if (priceOpen == value)
                    return;
                priceOpen = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        double priceClose = 0;
        public double PriceClose
        {
            get => priceClose;
            set
            {
                if (priceClose == value)
                    return;
                priceClose = value;
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
            if(trading < 0)
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

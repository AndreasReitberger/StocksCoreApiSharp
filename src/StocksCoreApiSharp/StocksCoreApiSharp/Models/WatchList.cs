using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Models.Database;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.IO;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(WatchList) + "s")]
#endif
    public partial class WatchList : BaseModel
    {
        #region Properties

        Guid id = Guid.Empty;
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

        bool isPrimaryWatchList = false;
        public bool IsPrimaryWatchList
        {
            get => isPrimaryWatchList;
            set
            {
                if (isPrimaryWatchList == value)
                    return;
                isPrimaryWatchList = value;
                OnPropertyChanged();
                NotifyListeners();
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
                NotifyListeners();
            }
        }

        double totalWorth = 0;
        public double TotalWorth
        {
            get => totalWorth;
            set
            {
                if (totalWorth == value)
                    return;
                totalWorth = value;
                OnPropertyChanged();
                NotifyListeners();
            }
        }

        #endregion

        #region Collections
        ObservableCollection<Stock> stocks = new();
#if SQLite
        [ManyToMany(typeof(StockWatchListRelation))]
#endif
        public ObservableCollection<Stock> Stocks
        {
            get => stocks;
            set
            {
                if (stocks == value)
                    return;
                stocks = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public WatchList()
        {
            Id = Guid.NewGuid();
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(Guid id)
        {
            Id = id;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(Guid id, string name)
        {
            Id = id;
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~WatchList()
        {
            Stocks.CollectionChanged -= Stocks_CollectionChanged;
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

        public event EventHandler<WatchListChangedEventArgs> WatchListChanged;
        protected virtual void OnWatchListChanged(WatchListChangedEventArgs e)
        {
            WatchListChanged?.Invoke(this, e);
        }
        #endregion

        #region Events
        void Stocks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateStocks();
            NotifyListeners();
        }
        #endregion

        #region Methods
        void NotifyListeners()
        {
            OnWatchListChanged(new()
            {
                Id = Id,
                Name = Name,
            });
        }
        public void Refresh()
        {
            //UpdateDependencies();
            UpdateStocks();
        }

        void UpdateDependencies()
        {
            TotalWorth = CalculateTotalWorth();
        }

        void UpdateStocks()
        {
            foreach (Stock stock in Stocks)
            {
                stock.RespectDividendsForEntrancePrice = false;
                stock.Refresh();
            }
            UpdateDependencies();
        }

        double CalculateTotalWorth()
        {
            double? worth = Stocks?.Sum(stock => stock.Quantity * stock.CurrentRate);
            return worth ?? 0;
        }
        double CalculateTotalCosts()
        {
            double? costs = Stocks?.Sum(stock => stock.Quantity * stock.EntrancePrice);
            return costs ?? 0;
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

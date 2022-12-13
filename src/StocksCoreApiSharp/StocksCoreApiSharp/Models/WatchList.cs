using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Models.Database;
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
    [Table(nameof(WatchList) + "s")]
#endif
    [ObservableObject]
    public partial class WatchList
    {
        #region Properties

        [ObservableProperty]
#if SQLite
        [property: PrimaryKey]
#endif
        Guid id = Guid.Empty;
        partial void OnIdChanged(Guid value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        string name = "";
        partial void OnNameChanged(string value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        bool isPrimaryWatchList = false;
        partial void OnIsPrimaryWatchListChanged(bool value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        DateTime? lastRefresh;
        partial void OnLastRefreshChanged(DateTime? value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        DateTime? dateOfCreation = null;
        partial void OnDateOfCreationChanged(DateTime? value)
        {
            NotifyListeners();
        }

        #endregion

        #region Collections
        [ObservableProperty]
#if SQLite
        [property: ManyToMany(typeof(StockWatchListRelation))]
#endif
        ObservableCollection<Stock> stocks = new();

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
        #endregion

        #region Overrides

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

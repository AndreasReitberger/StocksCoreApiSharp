using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class WatchList : RealmObject, IWatchList
    {
        #region Properties

        [property: PrimaryKey]
        Guid id { get; set; } = Guid.Empty;
        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                OnIdChanged(value);
            }
        }
        void OnIdChanged(Guid value)
        {
            NotifyListeners();
        }

        [Required]
        string name { get; set; } = "";
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnNameChanged(value);
            }
        }
        void OnNameChanged(string value)
        {
            NotifyListeners();
        }

        bool isPrimaryWatchList { get; set; } = false;
        public bool IsPrimaryWatchList
        {
            get => isPrimaryWatchList;
            set
            {
                isPrimaryWatchList = value;
                OnIsPrimaryWatchListChanged(value);
            }
        }
        void OnIsPrimaryWatchListChanged(bool value)
        {
            NotifyListeners();
        }

        DateTimeOffset? lastRefresh { get; set; }
        public DateTimeOffset? LastRefresh
        {
            get => lastRefresh;
            set
            {
                lastRefresh = value;
                OnLastRefreshChanged(value);
            }
        }
        void OnLastRefreshChanged(DateTimeOffset? value)
        {
            NotifyListeners();
        }

        DateTimeOffset? dateOfCreation { get; set; } = null;
        public DateTimeOffset? DateOfCreation
        {
            get => dateOfCreation;
            set
            {
                dateOfCreation = value;
                OnDateOfCreationChanged(value);
            }
        }
        void OnDateOfCreationChanged(DateTimeOffset? value)
        {
            NotifyListeners();
        }

        #endregion

        #region Collections
        //[property: ManyToMany(typeof(StockWatchListRelation))]
        public IList<Stock> Stocks { get; }
        //ObservableCollection<IStock> stocks = new();

        #endregion

        #region Constructor
        public WatchList()
        {
            Id = Guid.NewGuid();
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(Guid id)
        {
            Id = id;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public WatchList(Guid id, string name)
        {
            Id = id;
            Name = name;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~WatchList()
        {
            Stocks.AsRealmCollection().CollectionChanged -= Stocks_CollectionChanged;
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

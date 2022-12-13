using AndreasReitberger.Stocks.Models.Database;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.ComponentModel;
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
#if SQLite
    [Table(nameof(Marketplace) + "s")]
#endif
    [ObservableObject]
    public partial class Marketplace
    {
        #region Properties

        [ObservableProperty]
#if SQLite
        [property: PrimaryKey]
#endif
        Guid id = Guid.Empty;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        bool isOpen = false;

        [ObservableProperty]
        DateTime? lastRefresh;

        [ObservableProperty]
        DateTime? dateOfCreation = null;

        #endregion

        #region Collections
        [ObservableProperty]
#if SQLite
        [property: ManyToMany(typeof(StockMarketplaceRelation))]
#endif
        ObservableCollection<Stock> stocks = new();
        /*
#if SQLite
        [ManyToMany(typeof(StockMarketplaceRelation))]
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
        */
        #endregion

        #region Constructor
        public Marketplace()
        {
            Id = Guid.NewGuid();
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(Guid id)
        {
            Id = id;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(Guid id, string name)
        {
            Id = id;
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Marketplace()
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

        public event EventHandler<MarketplaceChangedEventArgs> MarketplaceChanged;
        protected virtual void OnMarketplaceChanged(MarketplaceChangedEventArgs e)
        {
            MarketplaceChanged?.Invoke(this, e);
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
            OnMarketplaceChanged(new()
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

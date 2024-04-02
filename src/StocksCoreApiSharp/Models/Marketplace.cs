using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Marketplace : ObservableObject, IMarketplace
    {
        #region Properties

        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        bool isOpen = false;

        [ObservableProperty]
        DateTimeOffset? lastRefresh;

        [ObservableProperty]
        DateTimeOffset? dateOfCreation = null;

        #endregion

        #region Collections
        [ObservableProperty]
        ObservableCollection<Stock> stocks = new();

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

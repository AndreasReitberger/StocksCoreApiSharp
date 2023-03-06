using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class Marketplace : RealmObject, IMarketplace
    {
        #region Properties

        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        public string Name { get; set; } = "";

        public bool IsOpen { get; set; } = false;

        public DateTimeOffset? LastRefresh { get; set; }

        public DateTimeOffset? DateOfCreation { get; set; } = null;

        #endregion

        #region Collections
        //[property: ManyToMany(typeof(StockMarketplaceRelation))]
        public IList<Stock> Stocks { get; }

        #endregion

        #region Constructor
        public Marketplace()
        {
            Id = Guid.NewGuid();
            //Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            //Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(Guid id)
        {
            Id = id;
            //Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Marketplace(Guid id, string name)
        {
            Id = id;
            Name = name;
            //Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Marketplace()
        {
            //Stocks.AsRealmCollection().CollectionChanged -= Stocks_CollectionChanged;
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
        protected override void OnManaged()
        {
            base.OnManaged();
            //Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

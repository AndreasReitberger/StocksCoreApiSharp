using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Realm
{
    public partial class Depot : RealmObject, IDepot
    {
        #region Properties
        
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        public string Name { get; set; }

        public bool IsPrimaryDepot { get; set; } = false;

        // https://www.mongodb.com/docs/realm/sdk/dotnet/model-data/define-object-model/
        bool considerDividendsForGrowthCalculation { get; set; } = false;
        public bool ConsiderDividendsForGrowthCalculation
        {
            get => considerDividendsForGrowthCalculation;
            set
            {
                considerDividendsForGrowthCalculation = value;
                OnConsiderDividendsForGrowthCalculationChanged(value);
            }
        }
        void OnConsiderDividendsForGrowthCalculationChanged(bool value)
        {
            // Do stuff (including calling other methods if needed)
            UpdateStocks();
            NotifyListeners();
        }

        public DateTimeOffset? LastRefresh { get; set; }

        public DateTimeOffset? DateOfCreation { get; set; } = null;

        double totalWorth { get; set; } = 0;
        public double TotalWorth
        {
            get => totalWorth;
            set
            {
                totalWorth = value;
                OnTotalWorthChanged(value);
            }
        }
        void OnTotalWorthChanged(double value)
        {
            // Do stuff (including calling other methods if needed)
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
            NotifyListeners();
        }
        
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
            // Do stuff (including calling other methods if needed)
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
            NotifyListeners();
        }
        
        double costGrowthRatio { get; set; } = 0;
        public double CostGrowthRatio
        {
            get => costGrowthRatio;
            set
            {
                costGrowthRatio = value;
                OnCostGrowthRatioChanged(value);
            }
        }
        void OnCostGrowthRatioChanged(double value)
        {
            // Do stuff (including calling other methods if needed)
            Growth = TotalWorth - TotalCosts;
            NotifyListeners();
        }
     
        public double Growth { get; set; } = 0;

        [Ignored]
        public bool PositiveGrowth => TotalCosts <= TotalWorth;

        [Ignored]
        public ObservableCollection<Dividend> OverallDividends => new(Stocks.SelectMany(stock => stock.Dividends));
        #endregion

        #region Collections
        
        //[property: ManyToMany(typeof(StockDepotRelation))]
        //ObservableCollection<Stock> Stocks { get; set; } = new();
        public IList<Stock> Stocks { get; }
        #endregion

        #region Constructor
        public Depot()
        {
            Id = Guid.NewGuid();
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(Guid id)
        {
            Id = id;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(Guid id, string name)
        {
            Id = id;
            Name = name;
            Stocks.AsRealmCollection().CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Depot()
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
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        protected virtual void OnError(Realms.ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        public event EventHandler<DepotChangedEventArgs> DepotChanged;
        protected virtual void OnDepotChanged(DepotChangedEventArgs e)
        {
            DepotChanged?.Invoke(this, e);
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
            OnDepotChanged(new()
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
            TotalCosts = CalculateTotalCosts();
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
        }

        void UpdateStocks()
        {
            foreach (Stock stock in Stocks)
            {
                stock.RespectDividendsForEntrancePrice = ConsiderDividendsForGrowthCalculation;
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

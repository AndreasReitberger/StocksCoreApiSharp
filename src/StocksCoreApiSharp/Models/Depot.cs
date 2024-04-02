using AndreasReitberger.Stocks.Models.Events;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using AndreasReitberger.Stocks.Interfaces;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Depot : ObservableObject, IDepot
    {
        #region Properties

        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        bool isPrimaryDepot = false;

        [ObservableProperty]
        bool considerDividendsForGrowthCalculation = true;
        partial void OnConsiderDividendsForGrowthCalculationChanged(bool value)
        {
            // Do stuff (including calling other methods if needed)
            UpdateStocks();
            NotifyListeners();
        }

        [ObservableProperty]
        DateTimeOffset? lastRefresh;

        [ObservableProperty]
        DateTimeOffset? dateOfCreation = null;

        [ObservableProperty]
        double totalWorth = 0;
        partial void OnTotalWorthChanged(double value)
        {
            // Do stuff (including calling other methods if needed)
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
            NotifyListeners();
        }

        [ObservableProperty]
        double totalCosts = 0;
        partial void OnTotalCostsChanged(double value)
        {
            // Do stuff (including calling other methods if needed)
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
            NotifyListeners();
        }

        [ObservableProperty]
        double costGrowthRatio = 0;
        partial void OnCostGrowthRatioChanged(double value)
        {
            // Do stuff (including calling other methods if needed)
            Growth = TotalWorth - TotalCosts;
            NotifyListeners();
        }

        [ObservableProperty]
        double growth = 0;

        public bool PositiveGrowth => TotalCosts <= TotalWorth;

        public ObservableCollection<Dividend> OverallDividends => new(Stocks.SelectMany(stock => stock.Dividends));
        #endregion

        #region Collections
        [ObservableProperty]
        ObservableCollection<Stock> stocks = new();
        #endregion

        #region Constructor
        public Depot()
        {
            Id = Guid.NewGuid();
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(Guid id)
        {
            Id = id;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }

        public Depot(Guid id, string name)
        {
            Id = id;
            Name = name;
            Stocks.CollectionChanged += Stocks_CollectionChanged;
        }
        #endregion

        #region Destructor
        ~Depot()
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

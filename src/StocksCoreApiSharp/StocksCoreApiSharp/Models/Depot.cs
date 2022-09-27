using AndreasReitberger.Core.Utilities;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Depot : BaseModel
    {
        #region Properties
        Guid id = Guid.Empty;
        public Guid Id
        {
            get => id;
            set
            {
                if (id == value)
                    return;
                id = value;
                OnPropertyChanged();
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
            }
        }

        bool isPrimaryDepot = false;
        public bool IsPrimaryDepot
        {
            get => isPrimaryDepot;
            set
            {
                if (isPrimaryDepot == value)
                    return;
                isPrimaryDepot = value;
                OnPropertyChanged();
            }
        }

        bool considerDividendsForGrowthCalculation = true;
        public bool ConsiderDividendsForGrowthCalculation
        {
            get => considerDividendsForGrowthCalculation;
            set
            {
                if (considerDividendsForGrowthCalculation == value)
                    return;
                considerDividendsForGrowthCalculation = value;
                OnPropertyChanged();
                UpdateStocks();
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
                CostGrowthRatio = TotalWorth / (TotalCosts / 100);
                //UpdateDependencies();
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
                CostGrowthRatio = TotalWorth / (TotalCosts / 100);
                //UpdateDependencies();
            }
        }

        double costGrowthRatio = 0;
        public double CostGrowthRatio
        {
            get => costGrowthRatio;
            set
            {
                if (costGrowthRatio == value)
                    return;
                costGrowthRatio = value;
                OnPropertyChanged();
                //UpdateDependencies();
            }
        }

        //public double TotalWorth => CalculateTotalWorth();
        //public double TotalCosts => CalculateTotalCosts();
        //public double CostGrowthRatio => TotalWorth / (TotalCosts / 100);

        public ObservableCollection<Dividend> OverallDividends => new(Stocks.SelectMany(stock => stock.Dividends));
        #endregion

        #region Collections
        ObservableCollection<Stock> stocks = new();
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

        #region Events
        void Stocks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateStocks();
        }
        #endregion

        #region Methods

        public void Refresh()
        {
            UpdateDependencies();
        }

        void UpdateDependencies()
        {
            TotalWorth = CalculateTotalWorth();
            TotalCosts = CalculateTotalCosts();
            CostGrowthRatio = TotalWorth / (TotalCosts / 100);
        }

        void UpdateStocks()
        {
            foreach(Stock stock in Stocks)
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
    }
}

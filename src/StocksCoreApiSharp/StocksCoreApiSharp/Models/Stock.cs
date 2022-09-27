using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Stock : BaseModel
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
            }
        }

        //public double Quantity => CalculateCurrentAmount();
        //public double EntrancePrice => CalculateEntrancePrice();
        //public double TotalCosts => CalculateTotalCosts();
        //public double TotalDividends => CalculateTotalDividends();

        //public double Growth => CalculateGrowth(); 
        //public double CurrentWorth => CalculateCurrentWorth(); 
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

        #region Events
        void Transactions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        void Dividends_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }
        #endregion

        #region Methods
        public void Refresh()
        {
            Quantity = CalculateCurrentAmount();
            EntrancePrice = CalculateEntrancePrice();
            TotalCosts = CalculateTotalCosts();
            TotalDividends = CalculateTotalDividends();
            Growth = CalculateGrowth();
            CurrentWorth = CalculateCurrentWorth();
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
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();

            //double? price = bought?.Select(transaction => transaction.Total).Sum();
            double? amount = bought?.Select(transaction => transaction.Amount).Sum();

            //double? entrancePrice = price / amount;
            double? currentWorth = RespectDividendsForEntrancePrice ?
                (amount * CurrentRate + CalculateTotalDividends()) :
                amount * CurrentRate;

            return currentWorth ?? 0;
        
        }

        double CalculateTotalCosts()
        {
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            List<Transaction>? sold = Transactions?.Where(transaction => transaction.Type == TransactionType.Sell).ToList();

            double? boughtTotal = bought?.Select(transaction => transaction.Total).Sum();
            double? soldTotal = sold?.Select(transaction => transaction.Total).Sum();
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
              
        double CalculateGrowth()
        {
            double? currentAmount = CalculateCurrentAmount();
            double? currentWorth = CurrentRate * currentAmount + CalculateTotalDividends();

            double? growth = currentWorth / CalculateTotalCosts() * 100 - 100;
            return growth ?? 0;
        }

        #endregion
    }
}

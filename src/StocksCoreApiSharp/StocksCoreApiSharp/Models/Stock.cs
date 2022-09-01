using AndreasReitberger.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Stock : BaseModel
    {

        #region Properties
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

        public double EntrancePrice => CalculateEntrancePrice();
        public double TotalCosts => CalculateTotalCosts();
        public double TotalDividends => CalculateTotalDividends();

        public double Growth => CalculateGrowth(); 
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

        #region Methods

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

            double? growth = CalculateTotalCosts() / currentWorth * 100;
            return growth ?? 0;
        }

        #endregion
    }
}

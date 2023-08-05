﻿using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.Models.Additions;
using AndreasReitberger.Stocks.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Stock : ObservableObject, IStock
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        Guid depotId = Guid.Empty;

        [ObservableProperty]
        Guid watchListId = Guid.Empty;

        [ObservableProperty]
        Guid marketplaceId = Guid.Empty;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        string symbol = "";

        [ObservableProperty]
        string iSIN = "";

        [ObservableProperty]
        string wKN = "";

        [ObservableProperty]
        string marketplace = "";

        [ObservableProperty]
        bool isRerfresing = false;

        [ObservableProperty]
        DateTimeOffset? lastRefresh;

        [ObservableProperty]
        double? dividendForecast;

        [ObservableProperty]
        DateTimeOffset? dateOfAGM;

        [ObservableProperty]
        string currency;

        [ObservableProperty]
        double? currentRate;
        partial void OnCurrentRateChanged(double? value)
        {
            NotifyListeners();
            UpdateChangedIndicator();
        }

        [ObservableProperty]
        bool respectDividendsForEntrancePrice = true;
        partial void OnRespectDividendsForEntrancePriceChanged(bool value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        double quantity = 0;

        [ObservableProperty]
        double entrancePrice = 0;
        partial void OnEntrancePriceChanged(double value)
        {
            NotifyListeners();
            UpdateChangedIndicator();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PositiveGrowth))]
        double totalCosts = 0;
        partial void OnTotalCostsChanged(double value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        double totalDividends = 0;

        [ObservableProperty]
        double growth = 0;

        [ObservableProperty]
        double growthPercentage = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PositiveGrowth))]
        double currentWorth = 0;

        partial void OnCurrentWorthChanged(double value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        double volume = 0;

        [ObservableProperty]
        double? priceOpen = 0;
        partial void OnPriceOpenChanged(double? value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        double? priceClose = 0;
        partial void OnPriceCloseChanged(double? value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        double? change = 0;
        partial void OnChangeChanged(double? value)
        {
            NotifyListeners();
        }

        [ObservableProperty]
        ValueChangedIndicator changedIndicator = ValueChangedIndicator.Unchanged;
        partial void OnChangedIndicatorChanged(ValueChangedIndicator value)
        {
            NotifyListeners();
        }

        public bool PositiveGrowth => TotalCosts <= CurrentWorth;
        public bool CostEarnBreakPointReached => TotalCosts <= TotalDividends;

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

        ObservableCollection<StockPriceRange> priceRanges = new();
        public ObservableCollection<StockPriceRange> PriceRanges
        {
            get => priceRanges;
            set
            {
                if (priceRanges == value)
                    return;
                priceRanges = value;
                OnPropertyChanged();
                UpdateOpenClosePrices();
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

        public event EventHandler<StockChangedEventArgs> StockChanged;
        protected virtual void OnStockChanged(StockChangedEventArgs e)
        {
            StockChanged?.Invoke(this, e);
        }
        #endregion

        #region Events
        void Transactions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            NotifyListeners();
        }

        void Dividends_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
            NotifyListeners();
        }
        #endregion

        #region Methods
        void NotifyListeners()
        {
            OnStockChanged(new()
            {
                Id = Id,
                DepotId = DepotId,
                Name = Name,
            });
        }

        void UpdateChangedIndicator()
        {
            if (CurrentRate == PriceOpen)
            {
                ChangedIndicator = ValueChangedIndicator.Unchanged;
            }
            else
            {
                ChangedIndicator = CurrentRate > PriceOpen ? ValueChangedIndicator.Increased : ValueChangedIndicator.Decreased;
            }
        }

        void UpdateOpenClosePrices()
        {
            StockPriceRange? priceRange = PriceRanges?.LastOrDefault();
            PriceClose = priceRange?.Close ?? CurrentRate;
            PriceOpen = priceRange?.Open ?? CurrentRate;
            // Change since the laste open
            Change = CurrentRate - PriceOpen;
            UpdateChangedIndicator();
        }

        public void Refresh()
        {
            Quantity = CalculateCurrentAmount();
            EntrancePrice = CalculateEntrancePrice();
            // Avoid negative costs, those will be respected in CurrentWorth calculation
            TotalCosts = Math.Clamp(CalculateTotalCosts(), 0, double.PositiveInfinity);
            TotalDividends = CalculateTotalDividends();
            CurrentWorth = CalculateCurrentWorth();

            Growth = CurrentWorth - TotalCosts;
            GrowthPercentage = CalculateGrowthPercentage();
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
            /*
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            double? amount = bought?.Select(transaction => transaction.Amount).Sum();
            */
            double amount = CalculateCurrentAmount();
            double trading = CalculateTotalCosts();
            //double? entrancePrice = price / amount;
            double? currentWorth = RespectDividendsForEntrancePrice ?
                (amount * CurrentRate + CalculateTotalDividends()) :
                amount * CurrentRate;
            // Indicates a win in trading (sold with profits)
            if (trading < 0)
            {
                currentWorth += Math.Abs(trading);
            }
            return currentWorth ?? 0;

        }

        double CalculateTotalCosts()
        {
            List<Transaction>? bought = Transactions?.Where(transaction => transaction.Type == TransactionType.Buy).ToList();
            List<Transaction>? sold = Transactions?.Where(transaction => transaction.Type == TransactionType.Sell).ToList();

            double? boughtTotal = bought?.Select(transaction => transaction.Total).Sum();
            double? soldTotal = sold?.Select(transaction => transaction.Total).Sum();
            // Avoid negative returns
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

        double CalculateGrowthPercentage()
        {
            double? currentAmount = CalculateCurrentAmount();
            double? currentWorth = CurrentRate * currentAmount + CalculateTotalDividends();

            // Avoid wrong calculations if the stock actually have been sold and no growth calculation is possible then.
            if (currentAmount == 0 || currentWorth == 0) return 0;

            double? growth = currentWorth / CalculateTotalCosts() * 100 - 100;
            return growth ?? 0;
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

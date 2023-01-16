using AndreasReitberger.Stocks.SQLite.Events;
using System;

namespace AndreasReitberger.Stocks.SQLite.Utilities
{
    public partial class DatabaseHandler
    {
        #region Events
        public event EventHandler<DatabaseEventArgs> DataChanged;
        protected virtual void OnDataChanged(DatabaseEventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        public event EventHandler<DatabaseEventArgs> QueryFinished;
        protected virtual void OnQueryFinished(DatabaseEventArgs e)
        {
            QueryFinished?.Invoke(this, e);
        }

        public event EventHandler<DepotsChangedDatabaseEventArgs> DepotsChanged;
        protected virtual void OnDepotsChanged(DepotsChangedDatabaseEventArgs e)
        {
            DepotsChanged?.Invoke(this, e);
        }

        public event EventHandler<WatchListsChangedDatabaseEventArgs> WatchListsChanged;
        protected virtual void OnWatchListsChanged(WatchListsChangedDatabaseEventArgs e)
        {
            WatchListsChanged?.Invoke(this, e);
        }

        public event EventHandler<MarketplacesChangedDatabaseEventArgs> MarketplacesChanged;
        protected virtual void OnMarketplacesChanged(MarketplacesChangedDatabaseEventArgs e)
        {
            MarketplacesChanged?.Invoke(this, e);
        }

        public event EventHandler<StocksChangedDatabaseEventArgs> StocksChanged;
        protected virtual void OnStocksChanged(StocksChangedDatabaseEventArgs e)
        {
            StocksChanged?.Invoke(this, e);
        }

        public event EventHandler<DividendsChangedDatabaseEventArgs> DividendsChanged;
        protected virtual void OnDividendsChanged(DividendsChangedDatabaseEventArgs e)
        {
            DividendsChanged?.Invoke(this, e);
        }

        public event EventHandler<TransactionsChangedDatabaseEventArgs> TransactionsChanged;
        protected virtual void OnTransactionsChanged(TransactionsChangedDatabaseEventArgs e)
        {
            TransactionsChanged?.Invoke(this, e);
        }


        #endregion
    }
}
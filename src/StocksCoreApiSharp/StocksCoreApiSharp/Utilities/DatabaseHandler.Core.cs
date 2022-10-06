#if SQLite
using AndreasReitberger.Stocks.Models;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.Stocks.Utilities
{
    public partial class DatabaseHandler
    {
        #region Methods

        #region Public

        #region Depots
        public async Task<List<Depot>> GetDepotsWithChildrenAsync()
        {
            // To trigger event
            Depots = await DatabaseAsync
                .GetAllWithChildrenAsync<Depot>(recursive: true)
                ;
            return Depots;
        }

        public async Task<Depot> GetDepotWithChildrenAsync(Guid id)
        {
            return await DatabaseAsync
                .GetWithChildrenAsync<Depot>(id, recursive: true)
                ;
        }

        public async Task SetDepotWithChildrenAsync(Depot depot)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(depot, recursive: true)
                ;
        }

        public async Task SetDepotsWithChildrenAsync(List<Depot> depots, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(depots);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(depots);
        }

        public async Task<int> DeleteMaterialAsync(Depot depot)
        {
            return await DatabaseAsync.DeleteAsync<Depot>(depot?.Id);
        }

        #endregion

        #region Stocks
        public async Task<List<Stock>> GetStocksWithChildrenAsync()
        {
            Stocks = await DatabaseAsync
                .GetAllWithChildrenAsync<Stock>(recursive: true)
                ;
            return Stocks;
        }

        public async Task<Stock> GetStockWithChildrenAsync(Guid id)
        {
            return await DatabaseAsync
                .GetWithChildrenAsync<Stock>(id, recursive: true)
                ;
        }

        public async Task SetStocksWithChildrenAsync(List<Stock> stocks, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(stocks);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(stocks);
        }

        public async Task SetStockWithChildrenAsync(Stock stock)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(stock, recursive: true)
                ;
        }

        public async Task<int> DeletePrinterAsync(Stock stock)
        {
            return await DatabaseAsync.DeleteAsync<Stock>(stock?.Id);
        }

        #endregion

        #region Dividends
        public async Task<List<Dividend>> GetDividendsWithChildrenAsync()
        {
            return await DatabaseAsync
                .GetAllWithChildrenAsync<Dividend>(recursive: true)
                ;
        }

        public async Task<Dividend> GetDividendWithChildrenAsync(Guid id)
        {
            return await DatabaseAsync
                .GetWithChildrenAsync<Dividend>(id, recursive: true)
                ;
        }

        public async Task SetDividendWithChildrenAsync(Dividend dividend)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(dividend, recursive: true)
                ;
        }

        public async Task SetDividendsWithChildrenAsync(List<Dividend> dividends, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(dividends);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(dividends);
        }

        public async Task<int> DeleteMaintenanceAsync(Dividend dividend)
        {
            return await DatabaseAsync.DeleteAsync<Dividend>(dividend.Id);
        }

        public async Task<int[]> DeleteDividendsAsync(List<Dividend> dividends)
        {
            Stack<int> results = new();
            for (int i = 0; i < dividends?.Count; i++)
            {
                results.Push(await DatabaseAsync.DeleteAsync<Dividend>(dividends[i]?.Id));
            }
            return results.ToArray();
        }

        #endregion

        #region Transactions
        public async Task<List<Transaction>> GetTransactionsWithChildrenAsync()
        {
            Transactions = await DatabaseAsync
                .GetAllWithChildrenAsync<Transaction>(recursive: true)
                ;
            return Transactions;
        }

        public async Task<Transaction> GetTransactionWithChildrenAsync(Guid id)
        {
            return await DatabaseAsync
                .GetWithChildrenAsync<Transaction>(id, recursive: true)
                ;
        }

        // https://stackoverflow.com/questions/35975235/one-to-many-relationship-in-sqlite-xamarin
        public async Task SetTransactionWithChildrenAsync(Transaction transaction)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(transaction, recursive: true)
                ;
        }

        public async Task SetTransactionsWithChildrenAsync(List<Transaction> transactions, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(transactions);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(transactions);
        }

        public async Task<int> DeleteTransactionAsync(Transaction transaction)
        {
            return await DatabaseAsync
                .DeleteAsync<Transaction>(transaction.Id);
        }

        public async Task<int[]> DeleteCustomersAsync(List<Transaction> transactions)
        {
            Stack<int> results = new();
            for (int i = 0; i < transactions?.Count; i++)
            {
                results.Push(await DatabaseAsync.DeleteAsync<Transaction>(transactions[i]?.Id));
            }
            return results.ToArray();
        }

        #endregion

        #endregion

        #endregion
    }
}
#endif
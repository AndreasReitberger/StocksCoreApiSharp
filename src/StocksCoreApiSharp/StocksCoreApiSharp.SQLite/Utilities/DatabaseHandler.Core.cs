using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.SQLite.Additions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.SQLite.Utilities
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
            // Ensures to recalculate some data
            Depots.ForEach(depot => depot?.Refresh());
            return Depots;
        }

        public async Task<Depot> GetDepotWithChildrenAsync(Guid id)
        {
            Depot depot = await DatabaseAsync
                .GetWithChildrenAsync<Depot>(id, recursive: true)
                ;
            depot.Refresh();
            return depot;
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

        public async Task SetDepotsWithChildrenAsync(ObservableCollection<Depot> depots, bool replaceExisting = true)
        {
            await SetDepotsWithChildrenAsync(depots.ToList(), replaceExisting);
        }

        public async Task<int> DeleteDepotAsync(Depot depot)
        {
            return await DatabaseAsync.DeleteAsync<Depot>(depot?.Id);
        }

#endregion

#region WatchLists
        public async Task<List<WatchList>> GetWatchListsWithChildrenAsync()
        {
            // To trigger event
            WatchLists = await DatabaseAsync
                .GetAllWithChildrenAsync<WatchList>(recursive: true)
                ;
            // Ensures to recalculate some data
            WatchLists.ForEach(depot => depot?.Refresh());
            return WatchLists;
        }

        public async Task<WatchList> GetWatchListWithChildrenAsync(Guid id)
        {
            WatchList watchlist = await DatabaseAsync
                .GetWithChildrenAsync<WatchList>(id, recursive: true)
                ;
            watchlist.Refresh();
            return watchlist;
        }

        public async Task SetWatchListWithChildrenAsync(WatchList watchlist)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(watchlist, recursive: true)
                ;
        }

        public async Task SetWatchListsWithChildrenAsync(List<WatchList> watchLists, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(watchLists);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(watchLists);
        }

        public async Task SetWatchListsWithChildrenAsync(ObservableCollection<WatchList> watchLists, bool replaceExisting = true)
        {
            await SetWatchListsWithChildrenAsync(watchLists.ToList(), replaceExisting);
        }

        public async Task<int> DeleteWatchListAsync(WatchList watchList)
        {
            return await DatabaseAsync.DeleteAsync<WatchList>(watchList?.Id);
        }

#endregion

#region Marketplaces
        public async Task<List<Marketplace>> GetMarketplacesWithChildrenAsync()
        {
            // To trigger event
            Marketplaces = await DatabaseAsync
                .GetAllWithChildrenAsync<Marketplace>(recursive: true)
                ;
            // Ensures to recalculate some data
            Marketplaces.ForEach(depot => depot?.Refresh());
            return Marketplaces;
        }

        public async Task<Marketplace> GetMarketplaceWithChildrenAsync(Guid id)
        {
            Marketplace marketplace = await DatabaseAsync
                .GetWithChildrenAsync<Marketplace>(id, recursive: true)
                ;
            marketplace.Refresh();
            return marketplace;
        }

        public async Task SetMarketplaceWithChildrenAsync(Marketplace marketplace)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(marketplace, recursive: true)
                ;
        }

        public async Task SetMarketplacesWithChildrenAsync(List<Marketplace> marketplaces, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(marketplaces);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(marketplaces);
        }

        public async Task SetMarketplacesWithChildrenAsync(ObservableCollection<Marketplace> marketplaces, bool replaceExisting = true)
        {
            await SetMarketplacesWithChildrenAsync(marketplaces.ToList(), replaceExisting);
        }

        public async Task<int> DeleteMarketplaceAsync(Marketplace marketplace)
        {
            return await DatabaseAsync.DeleteAsync<Marketplace>(marketplace?.Id);
        }

#endregion

#region Stocks
        public async Task<List<Stock>> GetStocksWithChildrenAsync()
        {
            Stocks = await DatabaseAsync
                .GetAllWithChildrenAsync<Stock>(recursive: true)
                ;
            // Ensures to recalculate some data
            Stocks.ForEach(stock => stock.Refresh());
            return Stocks;
        }

        public async Task<Stock> GetStockWithChildrenAsync(Guid id)
        {
            Stock stock = await DatabaseAsync
                .GetWithChildrenAsync<Stock>(id, recursive: true)
                ;
            stock.Refresh();
            return stock;
        }

        //public async Task SetStocksWithChildrenAsync(List<IStock> stocks, bool replaceExisting = true) => await SetStocksWithChildrenAsync(stocks: stocks.Cast<Stock>()?.ToList(), replaceExisting: replaceExisting).ConfigureAwait(false);
        public async Task SetStocksWithChildrenAsync(List<Stock> stocks, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(stocks);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(stocks);
        }

        public async Task SetStocksWithChildrenAsync(ObservableCollection<Stock> stocks, bool replaceExisting = true)
        {
            await SetStocksWithChildrenAsync(stocks.ToList(), replaceExisting);
        }

        public async Task SetStockWithChildrenAsync(Stock stock)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(stock, recursive: true)
                ;
        }

        public async Task<int> DeleteStockAsync(Stock stock)
        {
            return await DatabaseAsync.DeleteAsync<Stock>(stock?.Id);
        }
        public async Task<int[]> DeleteStocksAsync(List<Stock> stocks)
        {
            Stack<int> results = new();
            for (int i = 0; i < stocks?.Count; i++)
            {
                results.Push(await DatabaseAsync.DeleteAsync<Stock>(stocks[i]?.Id));
            }
            return results.ToArray();
        }
        public async Task<int[]> DeleteStocksAsync(ObservableCollection<Stock> stocks)
        {
            return await DeleteStocksAsync(stocks.ToList());
        }
#endregion

#region StockPriceRanges
        public async Task<List<StockPriceRange>> GetStockPriceRangesWithChildrenAsync()
        {
            List<StockPriceRange> stockPriceRanges = await DatabaseAsync
                .GetAllWithChildrenAsync<StockPriceRange>(recursive: true)
                ;
            return stockPriceRanges;
        }

        public async Task<StockPriceRange> GetStockPriceRangeWithChildrenAsync(Guid id)
        {
            StockPriceRange stockPriceRange = await DatabaseAsync
                .GetWithChildrenAsync<StockPriceRange>(id, recursive: true)
                ;
            return stockPriceRange;
        }

        public async Task SetStockPriceRangesWithChildrenAsync(List<StockPriceRange> stockPriceRanges, bool replaceExisting = true)
        {
            if (replaceExisting)
                await DatabaseAsync.InsertOrReplaceAllWithChildrenAsync(stockPriceRanges);
            else
                await DatabaseAsync.InsertAllWithChildrenAsync(stockPriceRanges);
        }

        public async Task SetStockPriceRangesWithChildrenAsync(ObservableCollection<StockPriceRange> stockPriceRanges, bool replaceExisting = true)
        {
            await SetStockPriceRangesWithChildrenAsync(stockPriceRanges.ToList(), replaceExisting);
        }

        public async Task SetStockPriceRangeWithChildrenAsync(StockPriceRange stockPriceRange)
        {
            await DatabaseAsync
                .InsertOrReplaceWithChildrenAsync(stockPriceRange, recursive: true)
                ;
        }

        public async Task<int> DeleteStockPriceRangeAsync(StockPriceRange stockPriceRange)
        {
            return await DatabaseAsync.DeleteAsync<StockPriceRange>(stockPriceRange?.Id);
        }
        public async Task<int[]> DeleteStockPriceRangesAsync(List<StockPriceRange> stockPriceRanges)
        {
            Stack<int> results = new();
            for (int i = 0; i < stockPriceRanges?.Count; i++)
            {
                results.Push(await DatabaseAsync.DeleteAsync<StockPriceRange>(stockPriceRanges[i]?.Id));
            }
            return results.ToArray();
        }
        public async Task<int[]> DeleteStockPriceRangesAsync(ObservableCollection<StockPriceRange> stockPriceRanges)
        {
            return await DeleteStockPriceRangesAsync(stockPriceRanges.ToList());
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

        public async Task<int> DeleteDividendAsync(Dividend dividend)
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

        public async Task<int[]> DeleteTransactionsAsync(List<Transaction> transactions)
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

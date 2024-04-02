using AndreasReitberger.Shared.Core.Utilities;
using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.SQLite;
using AndreasReitberger.Stocks.SQLite.Additions;
using AndreasReitberger.Stocks.SQLite.Utilities;
using Newtonsoft.Json;
using SQLite;

namespace StocksCoreApiSharp.Test.SQLite
{
    public class Tests
    {
        string key = "K4eO9Qq4GTO9D0g0aBPzYGp0KsBoYYyFT9S3SX1VgOg=";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StocksApiTest()
        {
            try
            {
                Depot myDepot = new("My depot")
                {
                    DateOfCreation = DateTime.Now,
                };

                Stock basf = new()
                {
                    Name = "BASF",
                    ISIN = "DE000BASF111",
                    CurrentRate = 41.05,
                };
                basf.Transactions.Add(new Transaction()
                {
                    StockId = basf.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 100,
                    Price = 64.10,
                    Type = TransactionType.Buy,
                });
                basf.Transactions.Add(new Transaction()
                {
                    StockId = basf.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 25,
                    Price = 60.10,
                    Type = TransactionType.Sell,
                });
                basf.Dividends.Add(new Dividend()
                {
                    StockId = basf.Id,
                    DateOfDividend = DateTime.Now,
                    AmountOfDividend = 3400.00,
                    Quantity = 100,
                    Tax = 300,
                });
                basf.Dividends.Add(new Dividend()
                {
                    StockId = basf.Id,
                    DateOfDividend = DateTime.Now.AddYears(-1),
                    AmountOfDividend = 3400.00,
                    Quantity = 100,
                    Tax = 300,
                });

                var total = basf.TotalCosts;
                var entryPrice = basf.EntrancePrice;
                var dividend = basf.TotalDividends;
                var growth = basf.Growth;

                myDepot.Stocks.Add(basf);

                Stock daimler = new()
                {
                    Name = "Mercedes Benz AG",
                    ISIN = "DE0007100000",
                    CurrentRate = 55.60,
                };

                daimler.Transactions.Add(new Transaction()
                {
                    StockId = daimler.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 100,
                    Price = 58.10,
                    Type = TransactionType.Buy,
                });
                daimler.Transactions.Add(new Transaction()
                {
                    StockId = daimler.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 30,
                    Price = 38.10,
                    Type = TransactionType.Buy,
                });
                daimler.Dividends.Add(new Dividend()
                {
                    StockId = daimler.Id,
                    DateOfDividend = DateTime.Now,
                    AmountOfDividend = 5000,
                    Quantity = 100,
                    Tax = 1200,
                });

                myDepot.Stocks.Add(daimler);
                var totalDepotWorth = myDepot.TotalWorth;
                var overallDividends = myDepot.OverallDividends;
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task StocksDatabaseApiTest()
        {
            try
            {
                string databasePath = "testdatabase.db";
                // Start with a clear database
                if (File.Exists(databasePath))
                {
                    File.Delete(databasePath);
                }
                DatabaseHandler.Instance = new DatabaseHandler(databasePath);
                if (DatabaseHandler.Instance.IsInitialized)
                {
                    await DatabaseHandler.Instance.InitTablesAsync();
                    Depot myDepot = new("My depot")
                    {
                        DateOfCreation = DateTime.Now,
                    };

                    Stock basf = new()
                    {
                        DepotId = myDepot.Id,
                        Name = "BASF",
                        ISIN = "DE000BASF111",
                        CurrentRate = 41.05,
                    };
                    basf.Transactions.Add(new Transaction()
                    {
                        StockId = basf.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 100,
                        Price = 64.10,
                        Type = TransactionType.Buy,
                    });
                    basf.Transactions.Add(new Transaction()
                    {
                        StockId = basf.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 25,
                        Price = 60.10,
                        Type = TransactionType.Sell,
                    });
                    basf.Dividends.Add(new Dividend()
                    {
                        StockId = basf.Id,
                        DateOfDividend = DateTime.Now,
                        AmountOfDividend = 3400.00,
                        Quantity = 100,
                        Tax = 300,
                    });
                    basf.Dividends.Add(new Dividend()
                    {
                        StockId = basf.Id,
                        DateOfDividend = DateTime.Now.AddYears(-1),
                        AmountOfDividend = 3400.00,
                        Quantity = 100,
                        Tax = 300,
                    });
                    basf.PriceRanges.Add(new StockPriceRange()
                    {
                        StockId = basf.Id,
                        Date = DateTime.Now,
                        Close = 51.51,
                        Open = 49.98,
                        Volume = 2161516,
                        High = 53.45,
                        Low = 46.46,
                    });
                    basf.PriceRanges.Add(new StockPriceRange()
                    {
                        StockId = basf.Id,
                        Date = DateTime.Now.AddDays(-1),
                        Close = 50.51,
                        Open = 53.56,
                        Volume = 6456321,
                        High = 51.44,
                        Low = 49.41,
                    });

                    var total = basf.TotalCosts;
                    var entryPrice = basf.EntrancePrice;
                    var dividend = basf.TotalDividends;
                    var growth = basf.Growth;

                    myDepot.Stocks.Add(basf);

                    Stock daimler = new()
                    {
                        DepotId = myDepot.Id,
                        Name = "Mercedes Benz AG",
                        ISIN = "DE0007100000",
                        CurrentRate = 55.60,
                    };
                    daimler.Transactions.Add(new Transaction()
                    {
                        StockId = daimler.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 100,
                        Price = 58.10,
                        Type = TransactionType.Buy,
                    });
                    daimler.Transactions.Add(new Transaction()
                    {
                        StockId = daimler.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 30,
                        Price = 38.10,
                        Type = TransactionType.Buy,
                    });
                    daimler.Dividends.Add(new Dividend()
                    {
                        StockId = daimler.Id,
                        DateOfDividend = DateTime.Now,
                        AmountOfDividend = 5000,
                        Quantity = 100,
                        Tax = 1200,
                    });

                    myDepot.Stocks.Add(daimler);
                    var totalDepotWorth = myDepot.TotalWorth;
                    var overallDividends = myDepot.OverallDividends;

                    await DatabaseHandler.Instance.SetStocksWithChildrenAsync(myDepot.Stocks.ToList(), true);
                    await DatabaseHandler.Instance.SetDepotWithChildrenAsync(myDepot);

                    await Task.Delay(20);
                    Depot? dbDepot = await DatabaseHandler.Instance.GetDepotWithChildrenAsync(myDepot.Id);
                    Assert.That(dbDepot is not null);
                    Assert.That(dbDepot?.Stocks.First(stock => stock.Name == "BASF").PriceRanges.Count == 2);

                    string jsonOriginal = JsonConvert.SerializeObject(myDepot, Formatting.Indented);
                    string jsonDatabase = JsonConvert.SerializeObject(dbDepot, Formatting.Indented);

                    //Assert.That(myDepot == dbDepot);
                    // Test WatchList
                    WatchList watchList = new("My Watchlist")
                    {
                        DateOfCreation = DateTime.Now,
                    };
                    basf.WatchListId = watchList.Id;
                    watchList.Stocks.Add(basf);

                    daimler.WatchListId = watchList.Id;
                    watchList.Stocks.Add(daimler);

                    await DatabaseHandler.Instance.SetStocksWithChildrenAsync(watchList.Stocks.ToList(), true);
                    await DatabaseHandler.Instance.SetWatchListWithChildrenAsync(watchList);
                    var loadedWatchLists = await DatabaseHandler.Instance.GetWatchListsWithChildrenAsync();

                    await Task.Delay(20);
                    Assert.That(loadedWatchLists?.Count > 0, "Watchlist.Count was 0 or null");
                    WatchList? list = loadedWatchLists?.FirstOrDefault(l => l.Id == watchList.Id);
                    Assert.That(list is not null);
                    Assert.That(list?.Stocks?.Count == 2, "Stocks.Count was not 2");

                    // Check if the updating works
                    await Task.Delay(20);
                    var stocks = await DatabaseHandler.Instance.GetStocksWithChildrenAsync();
                    Assert.That(stocks?.Count == 2, "Stocks.Count was not 2 after loading from database");

                    await DatabaseHandler.Instance.CloseDatabaseAsync();
                }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void WorthAndGrowthCalculationTest()
        {
            Depot myDepot = new("My depot")
            {
                DateOfCreation = DateTime.Now,
            };
            Stock basf = new()
            {
                DepotId = myDepot.Id,
                Name = "BASF",
                ISIN = "DE000BASF111",
                CurrentRate = 41.05,
            };
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 100,
                Price = 64.10,
                Type = TransactionType.Buy,
            });
            // Sell with win
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 100,
                Price = 104.10,
                Type = TransactionType.Sell,
            });

            myDepot.Stocks.Add(basf);

            double cost = basf.TotalCosts;
            Assert.That(cost >= 0);
            double worth = basf.CurrentWorth;
            Assert.That(worth > 0, Is.True);

            // Sell with loss
            myDepot = new("My depot")
            {
                DateOfCreation = DateTime.Now,
            };
            basf = new()
            {
                DepotId = myDepot.Id,
                Name = "BASF",
                ISIN = "DE000BASF111",
                CurrentRate = 41.05,
            };
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 100,
                Price = 104.10,
                Type = TransactionType.Buy,
            });
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 100,
                Price = 64.10,
                Type = TransactionType.Sell,
            });
            cost = basf.TotalCosts;
            Assert.That(cost >= 0);
            worth = basf.CurrentWorth;
            Assert.That(worth >= 0);
            var growth = basf.Growth;
            Assert.That(growth < 0);

            // Sell with loss
            myDepot = new("My depot")
            {
                DateOfCreation = DateTime.Now,
            };
            basf = new()
            {
                DepotId = myDepot.Id,
                Name = "BASF",
                ISIN = "DE000BASF111",
                CurrentRate = 41.05,
            };
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 100,
                Price = 104.10,
                Type = TransactionType.Buy,
            });
            basf.Transactions.Add(new Transaction()
            {
                StockId = basf.Id,
                DateOfCreation = DateTime.Now,
                Amount = 50,
                Price = 98.10,
                Type = TransactionType.Sell,
            });
            cost = basf.TotalCosts;
            Assert.That(cost >= 0);
            worth = basf.CurrentWorth;
            Assert.That(worth >= 0);
            growth = basf.Growth;
            Assert.That(growth < 0);
        }

        [Test]
        public void WatchListsTest()
        {
            WatchList watchList = new("My Watchlist");
            Stock basf = new()
            {
                DepotId = watchList.Id,
                Name = "BASF",
                ISIN = "DE000BASF111",
                CurrentRate = 41.05,
            };
            watchList.Stocks.Add(basf);
        }

        [Test]
        public void EarnCostPointReachedTest()
        {
            var stock = new Stock()
            {
                Name = "Test Stock",
                CurrentRate = 56.46,
                Transactions = new()
                {
                    new Transaction() { Amount = 100, Price = 40, DateOfCreation = DateTimeOffset.Now, Type = TransactionType.Buy}
                },
                Dividends = new()
                {
                    new Dividend() { AmountOfDividend = 2534.12, Quantity = 100, DateOfDividend = DateTimeOffset.Now, Tax = 531.15 }
                }
            };
            stock.Refresh();
            Assert.That(!stock.CostEarnBreakPointReached);

            stock.Dividends.Add(new Dividend() { AmountOfDividend = 5534.12, Quantity = 100, DateOfDividend = DateTimeOffset.Now, Tax = 1531.15 });
            stock.Refresh();

            Assert.That(stock.CostEarnBreakPointReached);
        }

        [Test]
        public async Task DatabaseEncrytpionTestAsync()
        {
            try
            {
                string databasePath = "testdatabase_secure.db";
                if (File.Exists(databasePath))
                    File.Delete(databasePath);

                using DatabaseHandler handler = await new DatabaseHandler.DatabaseHandlerBuilder()
                    .WithDatabasePath(databasePath)
                    .WithTable(typeof(Stock))
                    .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                    .WithPassphrase(key)
                    .BuildAsync();

                List<TableMapping>? mappings = handler.GetTableMappings();
                Assert.That(mappings?.Count > 0);

                await handler.SetStockWithChildrenAsync(new()
                {
                    Id = Guid.NewGuid(),
                    Name = "BASF",
                    ISIN = "DE000BASF111",
                    CurrentRate = 41.05,
                });
                List<Stock> stocks = await handler.GetStocksWithChildrenAsync();
                Assert.That(stocks?.Count > 0);

                await handler.CloseDatabaseAsync();
                handler.Dispose();

                try
                {
                    using DatabaseHandler handlerUnseure = await new DatabaseHandler.DatabaseHandlerBuilder()
                        .WithDatabasePath(databasePath)
                        .WithTable(typeof(Stock))
                        .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                        //.WithPassphrase(key)
                        .BuildAsync();
                    Assert.Fail("Building without the key should throw an exception");
                }
                catch (Exception) { }

                try
                {
                    using DatabaseHandler handlerUnseure = await new DatabaseHandler.DatabaseHandlerBuilder()
                        .WithDatabasePath(databasePath)
                        // Different key also should throw
                        .WithTable(typeof(Stock))
                        .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                        .WithPassphrase(EncryptionManager.GenerateBase64Key())
                        .BuildAsync();
                    Assert.Fail("Building without the key should throw an exception");
                }
                catch (Exception) { }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        [Test]
        public async Task DatabaseEncrytpionRekeyTestAsync()
        {
            try
            {
                string databasePath = "testdatabase_secure.db";
                if (File.Exists(databasePath))
                    File.Delete(databasePath);

                using DatabaseHandler handler = await new DatabaseHandler.DatabaseHandlerBuilder()
                    .WithDatabasePath(databasePath)
                    .WithTable(typeof(Stock))
                    .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                    .WithPassphrase(key)
                    .BuildAsync();

                List<TableMapping>? mappings = handler.GetTableMappings();
                Assert.That(mappings?.Count > 0);

                await handler.SetStockWithChildrenAsync(new()
                {
                    Id = Guid.NewGuid(),
                    Name = "BASF",
                    ISIN = "DE000BASF111",
                    CurrentRate = 41.05,
                });
                List<Stock> stocks = await handler.GetStocksWithChildrenAsync();
                Assert.That(stocks?.Count > 0);

                // try to rekey
                string newKey = EncryptionManager.GenerateBase64Key();
                await handler.RekeyDatabaseAsync(newKey);

                await handler.CloseDatabaseAsync();
                using DatabaseHandler handler2 = await new DatabaseHandler.DatabaseHandlerBuilder()
                    .WithDatabasePath(databasePath)
                    .WithTable(typeof(Stock))
                    .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                    .WithPassphrase(newKey)
                    .BuildAsync();

                stocks = await handler2.GetStocksWithChildrenAsync();
                Assert.That(stocks?.Count > 0);
                await handler2.CloseDatabaseAsync();
                try
                {
                    using DatabaseHandler handler3 = await new DatabaseHandler.DatabaseHandlerBuilder()
                        .WithDatabasePath(databasePath)
                        .WithTable(typeof(Stock))
                        .WithTables([typeof(Dividend), typeof(Transaction), typeof(StockPriceRange)])
                        .WithPassphrase(key)
                        .BuildAsync();
                    await handler2.CloseDatabaseAsync();
                    Assert.Fail("Should throw on wrong key");
                }
                catch (Exception) { }
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}
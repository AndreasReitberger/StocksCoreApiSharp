using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Models;
using AndreasReitberger.Stocks.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StocksCoreApiSharp.Test
{
    public class Tests
    {
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
                basf.Transactions.Add(new()
                {
                    StockId = basf.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 100,
                    Price = 64.10,
                    Type = TransactionType.Buy,
                });
                basf.Transactions.Add(new()
                {
                    StockId = basf.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 25,
                    Price = 60.10,
                    Type = TransactionType.Sell,
                });
                basf.Dividends.Add(new()
                {
                    StockId = basf.Id,
                    DateOfDividend = DateTime.Now,
                    AmountOfDividend = 3400.00,
                    Quantity = 100,
                    Tax = 300,
                });
                basf.Dividends.Add(new()
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

                daimler.Transactions.Add(new()
                {
                    StockId = daimler.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 100,
                    Price = 58.10,
                    Type = TransactionType.Buy,
                });
                daimler.Transactions.Add(new()
                {
                    StockId = daimler.Id,
                    DateOfCreation = DateTime.Now,
                    Amount = 30,
                    Price = 38.10,
                    Type = TransactionType.Buy,
                });
                daimler.Dividends.Add(new()
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
            catch(Exception exc)
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
                    basf.Transactions.Add(new()
                    {
                        StockId = basf.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 100,
                        Price = 64.10,
                        Type = TransactionType.Buy,
                    });
                    basf.Transactions.Add(new()
                    {
                        StockId = basf.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 25,
                        Price = 60.10,
                        Type = TransactionType.Sell,
                    });
                    basf.Dividends.Add(new()
                    {
                        StockId = basf.Id,
                        DateOfDividend = DateTime.Now,
                        AmountOfDividend = 3400.00,
                        Quantity = 100,
                        Tax = 300,
                    });
                    basf.Dividends.Add(new()
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
                        DepotId = myDepot.Id,
                        Name = "Mercedes Benz AG",
                        ISIN = "DE0007100000",
                        CurrentRate = 55.60,
                    };

                    daimler.Transactions.Add(new()
                    {
                        StockId = daimler.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 100,
                        Price = 58.10,
                        Type = TransactionType.Buy,
                    });
                    daimler.Transactions.Add(new()
                    {
                        StockId = daimler.Id,
                        DateOfCreation = DateTime.Now,
                        Amount = 30,
                        Price = 38.10,
                        Type = TransactionType.Buy,
                    });
                    daimler.Dividends.Add(new()
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

                    Depot? dbDepot = await DatabaseHandler.Instance.GetDepotWithChildrenAsync(myDepot.Id);
                    Assert.IsNotNull(dbDepot);
                    string jsonOriginal = JsonConvert.SerializeObject(myDepot, Formatting.Indented);
                    string jsonDatabase = JsonConvert.SerializeObject(dbDepot, Formatting.Indented);

                    //Assert.IsTrue(myDepot == dbDepot);
                }
            }
            catch(Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}
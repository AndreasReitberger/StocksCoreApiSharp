# StocksCoreApiSharp
This is the core C# library to manage stocks for our [StocksWatch](https://github.com/AndreasReitberger/StocksWatchMauiApp) application built with .NET MAUI.

[![.NET](https://github.com/AndreasReitberger/StocksCoreApiSharp/actions/workflows/dotnet-unittest.yml/badge.svg)]([https://github.com/AndreasReitberger/StocksCoreApiSharp/actions/workflows/dotnet-unittest.yml](https://github.com/AndreasReitberger/StocksCoreApiSharp/actions/workflows/dotnet-unittest.yml))

# Nuget
[![NuGet](https://img.shields.io/nuget/v/StocksCoreApiSharp.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/StocksCoreApiSharp/)
[![NuGet](https://img.shields.io/nuget/dt/StocksCoreApiSharp.svg)](https://www.nuget.org/packages/StocksCoreApiSharp)

# Usage

## Depots & Watchlists
`Depot` and `WatchList` objects can hold `Stock` items.

```cs
// Create a new depot
Depot myDepot = new("My depot")
{
    DateOfCreation = DateTime.Now,
    IsPrimaryDepot = true,
};
myDepot.Stocks.Add(new Stock("Daimler AG"));
```
```cs
// Create a new watchlist
WatchList myList = new("My watchlist")
{
    DateOfCreation = DateTime.Now,
};
myList.Stocks.Add(new Stock("Daimler AG"));
```

## Stocks
The `Stock` object holds all necessary informaton about a stock or ETF.

```cs
// Create a new Stock item
Stock basf = new()
{
    Name = "BASF",
    ISIN = "DE000BASF111",
    CurrentRate = 41.05,
};

// Add transactions to it
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

// Add dividends to it
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
```

Based on the `Transaction` and `Dividend` items, the `Stock` class will automatically calculate the amount of stocks hold, the total worth and growth.

# Database
The library supports SQLite by default. You either can create your own `DatabaseHandler` or use the provided one.

```cs
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
        Assert.IsTrue(loadedWatchLists?.Count > 0);
        WatchList list = loadedWatchLists?.FirstOrDefault(l => l.Id == watchList.Id);
        Assert.IsNotNull(list);
        Assert.IsTrue(list.Stocks?.Count == 2);

        // Check if the updating works
        var stocks = await DatabaseHandler.Instance.GetStocksWithChildrenAsync();
        Assert.IsTrue(stocks?.Count == 2);

        await DatabaseHandler.Instance.CloseDatabaseAsync();
    }
}
catch (Exception exc)
{
    Assert.Fail(exc.Message);
}
```


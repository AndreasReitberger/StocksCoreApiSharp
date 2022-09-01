using AndreasReitberger.Stocks.Enums;
using AndreasReitberger.Stocks.Models;

namespace StocksCoreApiSharp.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StacksApiTest()
        {
            try
            {
                Stock basf = new()
                {
                    Name = "BASF",
                    ISIN = "DE000BASF111",
                    CurrentRate = 41.05,
                };
                basf.Transactions.Add(new()
                {
                    DateOfCreation = DateTime.Now,
                    Amount = 100,
                    Price = 64.10,
                    Type = TransactionType.Buy,
                });
                basf.Transactions.Add(new()
                {
                    DateOfCreation = DateTime.Now,
                    Amount = 25,
                    Price = 60.10,
                    Type = TransactionType.Sell,
                });
                basf.Dividends.Add(new()
                {
                    DateOfDividend = DateTime.Now,
                    AmountOfDividend = 3400.00,
                    Quantity = 100,
                    Tax = 300,
                });
                basf.Dividends.Add(new()
                {
                    DateOfDividend = DateTime.Now.AddYears(-1),
                    AmountOfDividend = 3400.00,
                    Quantity = 100,
                    Tax = 300,
                });

                var total = basf.TotalCosts;
                var entryPrice = basf.EntrancePrice;
                var dividend = basf.TotalDividends;
                var growth = basf.Growth;

            }
            catch(Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
    }
}
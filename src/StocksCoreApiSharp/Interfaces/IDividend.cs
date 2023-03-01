namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IDividend
    {
        #region Propeties

        Guid Id { get; set; }

        Guid StockId { get; set; }

        DateTimeOffset? DateOfDividend { get; set; }

        double Quantity { get; set; }

        double AmountOfDividend { get; set; }

        double Tax { get; set; }

        public double Total { get; }
        public double TaxPercentage { get; }
        public double Margin { get; }
        #endregion
    }
}

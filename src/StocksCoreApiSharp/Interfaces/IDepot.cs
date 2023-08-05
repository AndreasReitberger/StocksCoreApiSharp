namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IDepot
    {
        #region Properties

        Guid Id { get; set; }

        string Name { get; set; }

        bool IsPrimaryDepot { get; set; }

        bool ConsiderDividendsForGrowthCalculation { get; set; }
        DateTimeOffset? LastRefresh { get; set; }

        DateTimeOffset? DateOfCreation { get; set; }

        double TotalWorth { get; set; }

        double TotalCosts { get; set; }

        double CostGrowthRatio { get; set; }

        double Growth { get; set; }

        public bool PositiveGrowth { get; }

        #endregion

        #region Collections
        /*
        ObservableCollection<IDividend> OverallDividends { get; }
        ObservableCollection<IStock> Stocks { get; }
        */
        #endregion

        #region Methods
        public void Refresh();

        #endregion
    }
}

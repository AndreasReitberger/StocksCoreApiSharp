using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IDepot
    {
        #region Properties

        Guid Id { get; set; }

        string Name { get; set; }

        bool IsPrimaryDepot { get; set; }

        bool ConsiderDividendsForGrowthCalculation { get; set; }
        DateTime? LastRefresh { get; set; }

        DateTime? DateOfCreation { get; set; }

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
        void Refresh();
        #endregion
    }
}

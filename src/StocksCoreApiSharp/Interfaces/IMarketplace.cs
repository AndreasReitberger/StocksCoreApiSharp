using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IMarketplace
    {
        #region Properties

        Guid Id { get; set; }

        string Name { get; set; }

        bool IsOpen { get; set; }

        DateTime? LastRefresh { get; set; }

        DateTime? DateOfCreation { get; set; }

        #endregion

        #region Collections
        //ObservableCollection<IStock> Stocks {get;set;}
        #endregion

        #region Methods

        void Refresh();

        #endregion
    }
}

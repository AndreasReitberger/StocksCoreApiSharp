namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IWatchList
    {
        #region Properties
        Guid Id { get; set; }

        string Name { get; set; }

        bool IsPrimaryWatchList { get; set; }

        DateTimeOffset? LastRefresh { get; set; }

        DateTimeOffset? DateOfCreation { get; set; }

        #endregion

        #region Collections
        //ObservableCollection<IStock> Stocks { get; set; }

        #endregion

        #region Methods

        void Refresh();

        #endregion
    }
}

using AndreasReitberger.Stocks.Utilities;

namespace AndreasReitberger.Stocks.SQLite.Utilities
{
    public partial class DatabaseHandler
    {
        #region Instance
        static DatabaseHandler? _instanceLazy;
        public static AsyncLazy<DatabaseHandler> InstanceLazy => new(() =>
        {
            //DatabaseHandler instance;
            lock (Lock)
            {
                _instanceLazy = new DatabaseHandler();
            }
            //await _instanceLazy.InitTablesAsync();
            return _instanceLazy;
        });

        #endregion
    }
}
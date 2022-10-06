#if SQLite
namespace AndreasReitberger.Stocks.Utilities
{
    public partial class DatabaseHandler
    {
        #region Instance
        public static AsyncLazy<DatabaseHandler> InstanceLazy => new(async () =>
        {
            DatabaseHandler instance;
            lock (Lock)
            {
                instance = new DatabaseHandler();
            }
            await instance.InitTablesAsync();
            return instance;
        });

        #endregion
    }
}
#endif
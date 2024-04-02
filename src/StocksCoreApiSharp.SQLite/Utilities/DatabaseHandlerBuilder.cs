namespace AndreasReitberger.Stocks.SQLite.Utilities
{
    public partial class DatabaseHandler
    {
        public class DatabaseHandlerBuilder
        {

            #region Instance
            readonly DatabaseHandler _databaseHandler = new();
            #endregion

            #region Method

            public async Task<DatabaseHandler> BuildAsync()
            {
                if (!string.IsNullOrEmpty(_databaseHandler.DatabasePath))
                {
                    SQLiteConnectionString con =
                        new(_databaseHandler.DatabasePath, true, key: _databaseHandler.Passphrase);
                    _databaseHandler.DatabaseAsync = new(con);
                    _databaseHandler.IsInitialized = true;
                    if (_databaseHandler.Tables?.Count > 0)
                    {
                        await _databaseHandler.CreateTablesAsync(_databaseHandler.Tables);
                    }
                    else
                        await _databaseHandler.InitTablesAsync();
                }
                return _databaseHandler;
            }
            public DatabaseHandler Build()
            {
                if (!string.IsNullOrEmpty(_databaseHandler.DatabasePath))
                {
                    SQLiteConnectionString con =
                        new(_databaseHandler.DatabasePath, true, key: _databaseHandler.Passphrase);
                    _databaseHandler.DatabaseAsync = new(con);
                    _databaseHandler.IsInitialized = true;
                    if (_databaseHandler.Tables?.Count > 0)
                    {
                        _databaseHandler.CreateTables(_databaseHandler.Tables);
                    }
                    else
                        _databaseHandler.InitTables();
                }
                return _databaseHandler;
            }

            public DatabaseHandlerBuilder WithDatabasePath(string databasePath)
            {
                _databaseHandler.DatabasePath = databasePath;
                return this;
            }

            public DatabaseHandlerBuilder WithTable(Type table)
            {
                _databaseHandler.Tables?.Add(table);
                return this;
            }

            public DatabaseHandlerBuilder WithTables(List<Type> tables)
            {
                _databaseHandler.Tables?.AddRange(tables);
                return this;
            }
            public DatabaseHandlerBuilder WithPassphrase(string passphrase)
            {
                _databaseHandler.Passphrase = passphrase;
                return this;
            }

            #endregion
        }
    }
}

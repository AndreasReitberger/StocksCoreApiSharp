using AndreasReitberger.Stocks.Interfaces;
using AndreasReitberger.Stocks.SQLite.Additions;
using AndreasReitberger.Stocks.SQLite.Database;
using AndreasReitberger.Stocks.SQLite.Events;
using AndreasReitberger.Stocks.SQLite.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Diagnostics;

namespace AndreasReitberger.Stocks.SQLite.Utilities
{
    public partial class DatabaseHandler : ObservableObject, IDatabaseHandler
    {
        #region Instance
        static DatabaseHandler? _instance;
        static readonly object Lock = new();
        public static DatabaseHandler? Instance
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new DatabaseHandler();
                }
                return _instance;
            }

            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }

        }
        #endregion

        #region Properties
        [ObservableProperty]
        bool isInitialized = false;

        [ObservableProperty]
        string databasePath = "";

        [ObservableProperty]
        string passphrase = string.Empty;

        [ObservableProperty]
        SQLiteAsyncConnection databaseAsync;

        [ObservableProperty]
        List<Action> delegates = [];

        [ObservableProperty]
        List<Type> tables = [];

        [ObservableProperty]
        List<Type> defaultTables = [
            typeof(Depot),
            typeof(WatchList),
            typeof(Stock),
            typeof(StockPriceRange),
            typeof(StockDepotRelation),
            typeof(StockWatchListRelation),
            typeof(StockDividendAppointment),
            typeof(Marketplace),
            typeof(StockMarketplaceRelation),
            typeof(Dividend),
            typeof(Transaction),
            ];
        #endregion

        #region Collections
        [ObservableProperty]
        List<Depot> depots = [];
        partial void OnDepotsChanged(List<Depot> value)
        {
            OnDepotsChangedEvent(new DepotsChangedDatabaseEventArgs()
            {
                Depots = value,
            });
        }

        [ObservableProperty]
        List<WatchList> watchLists = [];
        partial void OnWatchListsChanged(List<WatchList> value)
        {
            OnWatchListsChangedEvent(new WatchListsChangedDatabaseEventArgs()
            {
                WatchLists = value,
            });
        }

        [ObservableProperty]
        List<Marketplace> marketplaces = [];
        partial void OnMarketplacesChanged(List<Marketplace> value)
        {
            OnMarketplacesChangedEvent(new MarketplacesChangedDatabaseEventArgs()
            {
                Marketplaces = value,
            });
        }

        [ObservableProperty]
        List<Stock> stocks = [];
        partial void OnStocksChanged(List<Stock> value)
        {
            OnStocksChangedEvent(new StocksChangedDatabaseEventArgs()
            {
                Stocks = value,
            });
        }

        [ObservableProperty]
        List<Dividend> dividends = [];
        partial void OnDividendsChanged(List<Dividend> value)
        {
            OnDividendsChangedEvent(new DividendsChangedDatabaseEventArgs()
            {
                Dividends = value,
            });
        }

        [ObservableProperty]
        List<Transaction> transactions = [];
        partial void OnTransactionsChanged(List<Transaction> value)
        {
            OnTransactionsChangedEvent(new TransactionsChangedDatabaseEventArgs()
            {
                Transactions = value,
            });
        }

        #endregion

        #region Constructor
        public DatabaseHandler()
        {

        }
        public DatabaseHandler(string databasePath, bool updateInstance = true, string? passphrase = null)
        {
            // Docs: https://github.com/praeclarum/sqlite-net?tab=readme-ov-file#using-sqlcipher
            // Some examples: https://github.com/praeclarum/sqlite-net/blob/master/tests/SQLite.Tests/SQLCipherTest.cs
            SQLiteConnectionString connection = new(databasePath, true, key: passphrase);
            DatabaseAsync = new SQLiteAsyncConnection(connection);

            InitTables();
            IsInitialized = true;
            if (updateInstance) Instance = this;
        }
        #endregion

        #region Methods

        #region Public

        #region Init
        public void InitTables() => DefaultTables?.ForEach(async type => await DatabaseAsync.CreateTableAsync(type));
        public async Task InitTablesAsync() => DefaultTables?.ForEach(async type => await DatabaseAsync.CreateTableAsync(type));

        public Task<CreateTableResult> CreateTableAsnyc(Type table) => DatabaseAsync.CreateTableAsync(table);
        public void CreateTable(Type table) => DatabaseAsync.CreateTableAsync(table);

        public Task<CreateTablesResult> CreateTablesAsync(List<Type> tables) => DatabaseAsync.CreateTablesAsync(CreateFlags.None, tables?.ToArray());
        public void CreateTables(List<Type> tables) => tables.ForEach(async type => await DatabaseAsync.CreateTableAsync(type, CreateFlags.None));

        #endregion

        #region Delegates
        public async Task UpdateAllDelegatesAsync()
        {
            //var actions = Delegates.Select(task => new Task(task));
            List<Task> tasks = new(Delegates.Select(task => new Task(task)));
            await Task.WhenAll(tasks);
        }
        #endregion

        #region Database
        public void InitDatabase(string databasePath, string? passphrase = null)
        {
            SQLiteConnectionString connection = new(databasePath, true, key: passphrase);
            DatabaseAsync = new SQLiteAsyncConnection(connection);

            InitTables();
            IsInitialized = true;
            Instance = this;
        }

        public async Task InitDatabaseAsync(string databasePath, string? passphrase = null)
        {
            SQLiteConnectionString connection = new(databasePath, true, key: passphrase);
            DatabaseAsync = new SQLiteAsyncConnection(connection);

            await InitTablesAsync();
            IsInitialized = true;
            Instance = this;
        }

        public Task CloseDatabaseAsync() => DatabaseAsync.CloseAsync();

        public List<TableMapping>? GetTableMappings(string databasePath = "")
        {
            if (DatabaseAsync == null && !string.IsNullOrWhiteSpace(databasePath))
            {
                InitDatabase(databasePath);
            }
            return DatabaseAsync?.TableMappings.ToList();
        }

        public Task RebuildAllTableAsync() => InitTablesAsync();

        public async Task DropAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                await DatabaseAsync.DropTableAsync(mapping);
            }
        }

        public async Task TryDropAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                try
                {
                    await DatabaseAsync.DropTableAsync(mapping);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public async Task<bool> TryDropTableAsync(TableMapping mapping)
        {
            try
            {
                int result = await DatabaseAsync.DropTableAsync(mapping);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ClearAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                await DatabaseAsync.DeleteAllAsync(mapping);
            }
        }

        public async Task ClearTableAsync(TableMapping mapping)
        {
            await DatabaseAsync.DeleteAllAsync(mapping);
        }

        public async Task TryClearAllTableAsync()
        {
            foreach (TableMapping mapping in DatabaseAsync.TableMappings)
            {
                try
                {
                    await DatabaseAsync.DeleteAllAsync(mapping);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public Task BackupDatabaseAsync(string targetFolder, string databaseName) => DatabaseAsync.BackupAsync(targetFolder, databaseName);

        public void RekeyDatabase(string newPassword)
        {
            // Bases on: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/encryption?tabs=netcore-cli
            SQLiteConnectionWithLock con = DatabaseAsync.GetConnection();
            SQLiteCommand command = con
                .CreateCommand(
                    "SELECT quote($newPassword);",
                    new Dictionary<string, object>() { { "$newPassword", newPassword } }
                    );
            string quotedNewPassword = command.ExecuteScalar<string>();
            command = con
                .CreateCommand(
                    $"PRAGMA rekey = {quotedNewPassword}"
                    );
            command.ExecuteNonQuery();
        }

        public async Task RekeyDatabaseAsync(string newPassword)
        {
            try
            {
                // Bases on: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/encryption?tabs=netcore-cli
                string quotedNewPassword = await DatabaseAsync
                    .ExecuteScalarAsync<string>(
                        $"SELECT quote('{newPassword}');"
                        );
                await DatabaseAsync.ExecuteAsync($"PRAGMA rekey = {quotedNewPassword}");
            }
            catch (Exception exc)
            {
                OnErrorEvent(new ErrorEventArgs(exc));
            }
        }

        public Task CloseAsync() => DatabaseAsync.CloseAsync();
        public void Close() => DatabaseAsync?.CloseAsync();

        public void Dispose() => Close();

        #endregion

        #region Static

        public static async Task<Tuple<T, TimeSpan?>> StopWatchFunctionAsync<T>(Func<T> action, bool inNewTask = false)
        {
            Stopwatch timer = new();
            timer.Start();
            T result;
            if (inNewTask)
            {
                result = await Task.Run(() =>
                {
                    return action();
                });
            }
            else
            {
                result = action();
            }
            timer.Stop();
            //var t = new Tuple<T, TimeSpan?>(result, timer?.Elapsed);
            return new Tuple<T, TimeSpan?>(result, timer?.Elapsed);
        }

        public static T StopWatchFunction<T>(Func<T> action, out TimeSpan? duration)
        {
            Stopwatch timer = new();
            timer.Start();
            T result = action();

            timer.Stop();
            duration = timer?.Elapsed;

            return result;
        }

        #endregion

        #region Clone
        public object Clone() => MemberwiseClone();

        #endregion

        #endregion

        #endregion
    }
}

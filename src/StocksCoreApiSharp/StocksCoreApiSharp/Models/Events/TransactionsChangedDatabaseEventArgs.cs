#if SQLite
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Stocks.Models.Events
{
    public class TransactionsChangedDatabaseEventArgs : DatabaseEventArgs
    {
#region Properties
        public List<Transaction> Transactions { get; set; } = new();
#endregion

#region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
#endregion
    }
}
#endif
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Stocks.Realm.Events
{
    public class DepotsChangedDatabaseEventArgs : DatabaseEventArgs
    {
        #region Properties
        public List<Depot> Depots { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
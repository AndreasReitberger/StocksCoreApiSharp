using Newtonsoft.Json;

namespace AndreasReitberger.Stocks.Realm.Events
{
    public class MarketplacesChangedDatabaseEventArgs : DatabaseEventArgs
    {
        #region Properties
        public List<Marketplace> Marketplaces { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }
}
#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace AndreasReitberger.Stocks.Models.Database
{
    [Table("StockDepot")]
    public partial class StockDepotRelation
    {
        
        [ForeignKey(typeof(Stock))]
        public Guid StockId { get; set; }

        [ForeignKey(typeof(Depot))]
        public Guid DepotId { get; set; }
    }
}
#endif
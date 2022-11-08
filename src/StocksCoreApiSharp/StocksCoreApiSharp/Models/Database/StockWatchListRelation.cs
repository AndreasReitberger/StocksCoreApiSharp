#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace AndreasReitberger.Stocks.Models.Database
{
    [Table(nameof(StockWatchListRelation) + "s")]
    public partial class StockWatchListRelation
    {
        
        [ForeignKey(typeof(Stock))]
        public Guid StockId { get; set; }

        [ForeignKey(typeof(WatchList))]
        public Guid WatchListId { get; set; }
    }
}
#endif
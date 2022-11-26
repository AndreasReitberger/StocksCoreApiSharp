#if SQLite
using SQLite;
using SQLiteNetExtensions.Attributes;
#endif
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;

namespace AndreasReitberger.Stocks.Models.Additions
{
#if SQLite
    [Table(nameof(StockDividendAppointment) + "s")]
#endif
    public partial class StockDividendAppointment
    {
        #region  Properties
#if SQLite
        [PrimaryKey]
#endif
        public Guid Id { get; set; }
#if SQLite
        [ForeignKey(typeof(Stock))]
#endif
        public Guid StockId { get; set; }
        public string? Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAllDay { get; set; }
        public string? Notes { get; set; }
        [Ignore]
        public TimeZoneInfo? StartTimeZone { get; set; }
        [Ignore]
        public TimeZoneInfo? EndTimeZone { get; set; }
        [Ignore]
        public Color Background { get; set; }
        public string? BackgroundColorCode { get; set; }
        public Guid? RecurrenceId { get; set; }
        public string? RecurrenceRule { get; set; }
        [OneToMany]
        public ObservableCollection<DateTime>? RecurrenceExceptions { get; set; } = new();

        #endregion

        #region Constructor
        public StockDividendAppointment()
        {
            Id = Guid.NewGuid();
        }
        public StockDividendAppointment(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Overrides

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}

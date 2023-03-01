
using AndreasReitberger.Stocks.Interfaces;
using Newtonsoft.Json;
using System.Drawing;

namespace AndreasReitberger.Stocks.Realm.Additions
{
    public partial class StockDividendAppointment : RealmObject, IStockDividendAppointment
    {
        #region  Properties
        [PrimaryKey]
        public Guid Id { get; set; }

        //[property: ForeignKey(typeof(Stock))]
        public Guid StockId { get; set; }

        public string? Name { get; set; }

        public DateTimeOffset From { get; set; }

        public DateTimeOffset To { get; set; }

        public bool IsAllDay { get; set; }

        public string? Notes { get; set; }

        [Ignored]
        public TimeZoneInfo? StartTimeZone { get; set; }

        [Ignored]
        public TimeZoneInfo? EndTimeZone { get; set; }

        [Ignored]
        public Color Background { get; set; }

        public string? BackgroundColorCode { get; set; }

        public Guid? RecurrenceId { get; set; }

        public string? RecurrenceRule { get; set; }

        #endregion

        #region Collections

        //[property: OneToMany]
        public IList<DateTimeOffset>? RecurrenceExceptions { get; }

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

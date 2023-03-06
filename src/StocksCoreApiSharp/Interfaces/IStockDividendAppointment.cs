using System.Collections.ObjectModel;
using System.Drawing;

namespace AndreasReitberger.Stocks.Interfaces
{
    public interface IStockDividendAppointment
    {

        #region  Properties

        Guid Id { get; set; }

        Guid StockId { get; set; }

        string? Name { get; set; }

        DateTimeOffset From { get; set; }

        DateTimeOffset To { get; set; }

        bool IsAllDay { get; set; }

        string? Notes { get; set; }

        TimeZoneInfo? StartTimeZone { get; set; }

        TimeZoneInfo? EndTimeZone { get; set; }

        Color Background { get; set; }

        string? BackgroundColorCode { get; set; }

        Guid? RecurrenceId { get; set; }

        string? RecurrenceRule { get; set; }

        #endregion

        #region Collections

        //ObservableCollection<DateTime>? RecurrenceExceptions { get; set; }
        #endregion
    }
}

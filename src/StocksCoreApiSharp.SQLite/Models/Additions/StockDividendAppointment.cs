
using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;
using System.Drawing;

namespace AndreasReitberger.Stocks.SQLite.Additions
{

    [Table(nameof(StockDividendAppointment) + "s")]
    public partial class StockDividendAppointment : ObservableObject, IStockDividendAppointment
    {
        #region  Properties
        [ObservableProperty]
        [property: PrimaryKey]
        Guid id;

        [ObservableProperty]
        [property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        string? name;

        [ObservableProperty]
        DateTimeOffset from;

        [ObservableProperty]
        DateTimeOffset to;

        [ObservableProperty]
        bool isAllDay;

        [ObservableProperty]
        string? notes;

        [ObservableProperty]
        [property: Ignore]
        TimeZoneInfo? startTimeZone;

        [ObservableProperty]
        [property: Ignore]
        TimeZoneInfo? endTimeZone;

        [ObservableProperty]
        [property: Ignore]
        Color background;

        [ObservableProperty]
        string? backgroundColorCode;

        [ObservableProperty]
        Guid? recurrenceId;

        [ObservableProperty]
        string? recurrenceRule;

        #endregion

        #region COllections

        [ObservableProperty]
        [property: OneToMany]
        ObservableCollection<DateTimeOffset>? recurrenceExceptions = new();

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

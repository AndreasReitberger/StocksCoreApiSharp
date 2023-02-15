
using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;

namespace AndreasReitberger.Stocks.Realm.Additions
{
    public partial class StockDividendAppointment : ObservableObject, IStockDividendAppointment
    {
        #region  Properties
        [ObservableProperty]
        [property: PrimaryKey]
        Guid id;

        [ObservableProperty]
        //[property: ForeignKey(typeof(Stock))]
        Guid stockId;

        [ObservableProperty]
        string? name;

        [ObservableProperty]
        DateTime from;

        [ObservableProperty]
        DateTime to;

        [ObservableProperty]
        bool isAllDay;

        [ObservableProperty]
        string? notes;

        [ObservableProperty]
        [property: Ignored]
        TimeZoneInfo? startTimeZone;

        [ObservableProperty]
        [property: Ignored]
        TimeZoneInfo? endTimeZone;

        [ObservableProperty]
        [property: Ignored]
        Color background;

        [ObservableProperty]
        string? backgroundColorCode;

        [ObservableProperty]
        Guid? recurrenceId;

        [ObservableProperty]
        string? recurrenceRule;

        #endregion

        #region Collections

        [ObservableProperty]
        //[property: OneToMany]
        ObservableCollection<DateTime>? recurrenceExceptions = new();

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

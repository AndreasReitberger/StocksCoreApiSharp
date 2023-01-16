﻿using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.Stocks.Models.Additions
{
    [ObservableObject]
    public partial class StockDividendAppointment
    {
        #region  Properties
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
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

        [ObservableProperty]
        [property: OneToMany]
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

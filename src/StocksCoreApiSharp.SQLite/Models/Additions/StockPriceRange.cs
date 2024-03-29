﻿using AndreasReitberger.Stocks.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AndreasReitberger.Stocks.SQLite.Additions
{

    [Table(nameof(StockPriceRange) + "s")]
    public partial class StockPriceRange : ObservableObject, IStockPriceRange
    {
        #region Properties
        [ObservableProperty]
        [property: PrimaryKey]
        Guid id = Guid.Empty;

        [ObservableProperty]
        [property: ForeignKey(typeof(Depot))]
        Guid stockId = Guid.Empty;

        [ObservableProperty]
        DateTimeOffset date;

        [ObservableProperty]
        double open;

        [ObservableProperty]
        double close;

        [ObservableProperty]
        double high;

        [ObservableProperty]
        double low;

        [ObservableProperty]
        double volume;
        #endregion

        #region Constructor
        public StockPriceRange()
        {
            Id = Guid.NewGuid();
        }
        public StockPriceRange(Guid id)
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

using AndreasReitberger.Core.Utilities;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Stocks.Models
{
    public partial class Depot : BaseModel
    {
        #region Properties

        string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }

        DateTime? dateOfCreation = null;
        public DateTime? DateOfCreation
        {
            get => dateOfCreation;
            set
            {
                if (dateOfCreation == value)
                    return;
                dateOfCreation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Collections
        ObservableCollection<Stock> stocks = new();
        public ObservableCollection<Stock> Stocks
        {
            get => stocks;
            set
            {
                if (stocks == value)
                    return;
                stocks = value;
                OnPropertyChanged();
            }
        }
        #endregion

    }
}

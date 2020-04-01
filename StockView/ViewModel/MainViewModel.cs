using StockView.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockView.ViewModel
{
    public class MainViewModel
    {
        ObservableCollection<object> _children;
        public ObservableCollection<object> Children { get { return _children; } }
        IBusinessLogic businessLogic = new BusinessLogic();
        StockConfigReader stckreader = new StockConfigReader();
        public string configFileName = @"\DefinedStocks.xml";
        public MainViewModel()
        {
            _children = new ObservableCollection<object>();
            _children.Add(new StockDataViewModel(businessLogic, configFileName, stckreader));
        }
    }
}

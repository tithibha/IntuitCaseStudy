using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockView.Model
{
    public class StockTrend
    {
        public string StockSymbol
        {
            get; set;
        }
        public string StockName
        {
            get; set;
        }
        public string Price
        {
            get; set;
        }
        public string Change
        {
            get; set;
        }
        public string YearHigh
        {
            get; set;
        }
        public string YearLow
        {
            get; set;
        }
        //public string TimeStamp
        //{
        //    get; set;
        //}
    }
}

using StockView.CommonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockView.Model
{
    /*
    [ {
  "symbol" : "AAPL",
  "name" : "Apple Inc.",
  "price" : 251.89000000,
  "changesPercentage" : 2.59000000,
  "change" : 6.37000000,
  "dayLow" : 246.36000000,
  "dayHigh" : 255.00000000,
  "yearHigh" : 327.85000000,
  "yearLow" : 170.27000000,
  "marketCap" : 1102139621376.00000000,
  "priceAvg50" : 287.00342000,
  "priceAvg200" : 269.64044000,
  "volume" : 32880383,
  "avgVolume" : 47970853,
  "exhange" : "NASDAQ",
  "open" : 246.52000000,
  "previousClose" : 245.52000000,
  "eps" : 12.59500000,
  "pe" : 19.99920500,
  "earningsAnnouncement" : "2020-01-28T21:30:00.000+0000",
  "sharesOutstanding" : 4375479808,
  "timestamp" : 1585242982
} ] 
    */
    public class CompanyQuote
    {
        private string symbol;
        private string timestamp;
        private string sharesOutstanding;
        private string earningsAnnouncement;
        private string pe;
        private string eps;
        private string previousClose;
        private string open;
        private string exhange;
        private string avgVolume;
        private string volume;
        private string priceAvg200;
        private string priceAvg50;
        private string marketCap;
        private string yearHigh;
        private string yearLow;
        private string dayHigh;
        private string dayLow;
        private string change;
        private string changesPercentage;
        private string price;
        private string nm;

        public string Symbol { get { return symbol; } set { symbol = value; } }
        public string name
        {
            get { return nm; }
            set { nm = value;  }
        }
        public string Price { get { return price; } set { price = value;  } }
        public string ChangesPercentage { get { return changesPercentage; } set { changesPercentage = value;  } }
        public string Change { get { return change; } set { change = value;  } }
        public string DayLow { get { return dayLow; } set { dayLow = value;  } }
        public string DayHigh { get { return dayHigh; } set { dayHigh = value; } }
        public string YearHigh { get { return yearHigh; } set { yearHigh = value; } }
        public string YearLow { get { return yearLow; } set { yearLow = value;  } }
        public string MarketCap { get { return marketCap; } set { marketCap = value;  } }
        public string PriceAvg50 { get { return priceAvg50; } set { priceAvg50 = value;  } }
        public string PriceAvg200 { get { return priceAvg200; } set { priceAvg200 = value;  } }
        public string Volume { get { return volume; } set { volume = value;  } }
        public string AvgVolume { get { return avgVolume; } set { avgVolume = value;  } }
        public string Exhange { get { return exhange; } set { exhange = value;} }
        public string Open { get { return open; } set { open = value;  } }
        public string PreviousClose { get { return previousClose; } set { previousClose = value; } }
        public string Eps { get { return eps; } set { eps = value;  } }
        public string Pe { get { return pe; } set { pe = value; } }
        public string EarningsAnnouncement { get { return earningsAnnouncement; } set { earningsAnnouncement = value;  } }
        public string SharesOutstanding { get { return sharesOutstanding; } set { sharesOutstanding = value; } }
        public string Timestamp { get { return timestamp; } set { timestamp = value;  } }
        
    }
}

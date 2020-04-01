using System.Collections.Generic;

namespace StockView.Model
{
    public class Company
    {
        private string symbol;
        private string name;
        private string price;
        private string exchange;

        public string Symbol { get { return symbol; } set { symbol = value;  } }
        public string Name { get { return name; } set { name = value;  } }
        public string Price { get { return price; } set { price = value;  } }
        public string Exchange { get { return exchange; } set { exchange = value;} }
    }
    public class symbols
    {
        public List<Company> symbolsList { get; set; }
    }
}
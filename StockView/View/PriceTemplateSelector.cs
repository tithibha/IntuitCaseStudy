using StockView.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StockView.View
{
    public class PriceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate lowtemp { get; set; }
        public DataTemplate hightemp { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;
            FrameworkElement fw = container as FrameworkElement;
            if (fw != null)
            {
                if (item is CompanyQuote)
                {
                    double change = Convert.ToDouble(((CompanyQuote)item).Change);
                    if (change > 0)
                    {
                        hightemp = fw.FindResource("Inncreased") as DataTemplate;
                        return hightemp;
                    }
                    else
                    {
                        lowtemp = fw.FindResource("Decreased") as DataTemplate;
                        return lowtemp;
                    }
                }
            }
            return null;
        }
    }
}

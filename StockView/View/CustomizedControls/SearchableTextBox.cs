using StockView.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StockView.View
{
    public class SearchableTextBox:Control
    {
            TextBox _txtbox; Button _btn;
            StackPanel resultStack;
            Border border;
            ObservableCollection<Company> data;
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                _txtbox = Template.FindName("textBox", this) as TextBox;  
                _txtbox.KeyUp += _handleKeyup;
                _btn = Template.FindName("_btn", this) as Button;
                _btn.Click += onbuttonCliked;
                resultStack = Template.FindName("resultStack", this) as StackPanel;
                border = (resultStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Collapsed;
            
                data = ItemSourceList;
            }

        private void onbuttonCliked(object sender, RoutedEventArgs e)
        {
            _txtbox.Text="";
        }

        private void _handleKeyup(object sender, KeyEventArgs e)
            {
                bool found = false;
                border = (resultStack.Parent as ScrollViewer).Parent as Border;
                string query = (sender as TextBox).Text;

                if (query.Length == 0)
                {
                    // Clear
                    resultStack.Children.Clear();
                    border.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    border.Visibility = System.Windows.Visibility.Visible;
                }
                resultStack.Children.Clear();
                foreach (var obj in data)
                {
                    if (((obj.Name!=null) &&(obj.Name.ToLower().StartsWith(query.ToLower())))
                    ||(obj.Symbol.ToLower().StartsWith(query.ToLower())))
                    {
                        addItem(obj.Name);
                        found = true;
                    }
                }

                if (!found)
                {
                    resultStack.Children.Add(new TextBlock() { Text = "No results found." });
                }
            }

            private void addItem(string text)
            {
                TextBlock block = new TextBlock();

                // Add the text
                block.Text = text;

                // A little style...
                block.Margin = new Thickness(2, 3, 2, 3);
                block.Cursor = Cursors.Hand;

                // Mouse events
                block.MouseLeftButtonUp += (sender, e) =>
                {
                    _txtbox.Text = (sender as TextBlock).Text;
                    
                    border.Visibility= System.Windows.Visibility.Collapsed; 
                };

                block.MouseEnter += (sender, e) =>
                {
                    TextBlock b = sender as TextBlock;
                    b.Background = Brushes.PeachPuff;
                };

                block.MouseLeave += (sender, e) =>
                {
                    TextBlock b = sender as TextBlock;
                    b.Background = Brushes.Transparent;
                };

                // Add to the panel
                resultStack.Children.Add(block);
            }

            public static readonly DependencyProperty ItemSourceListProperty = DependencyProperty.Register("ItemSourceList",
                typeof(ObservableCollection<Company>), typeof(SearchableTextBox), new PropertyMetadata(null, ItemSourceListChanged));

            private static void ItemSourceListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var ss = d as SearchableTextBox;
                ss.ItemSourceList = (ObservableCollection<Company>)e.NewValue;
                
            }

            public ObservableCollection<Company> ItemSourceList
            {
                get
                {
                    return (ObservableCollection<Company>)GetValue(ItemSourceListProperty);
                }
                set
                {
                    SetValue(ItemSourceListProperty, value);
                    data = value;
                }
            }
    }
}



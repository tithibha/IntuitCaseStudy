   M S B A M L   `   `   `  ����D  @StockView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:StockView.ViewStockView.View  5      9!clr-namespace:StockView.ViewModelStockView.ViewModel  5      "    StockView.View.StockDataView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:StockView.View     5      0vm!clr-namespace:StockView.ViewModel     5        � 	Resources  5
         
Inncreased  	Decreased*   $StockView.View.PriceTemplateSelector  LatestPriceTemp   hightemp   lowtemp  searchablebox �� 
TargetType&    StockView.View.SearchableTextBox �� Property �� Value �� 
TargetType � RowDefinitions � ColumnDefinitions	 �� UpdateSourceTrigger
 �� Mode��%�  &	        &	 P     &	 �     7   7  &	 �     '�� �    �� 5   
   .R�5      �� 
��� � ���6   �� *Price+6E   5      �� 5   
   .R�5      �� 
���  ����6   �� *Price+6C   5        5   
   8   8  6e   �� 5   
   "  6&   .�5      ��  � �� 5      �� 5      "  6&   .R�5      �� #��z��6!   #��z��5      #��z��5      ._�5      �  5   "   �� 5   "   $
��Auto �60   6@   �� 5   "   $
��Auto �60   6@    5   "   �� 5   &   $
��Auto �67   �� 5    &   $
��Auto �67   �� 5!   &   $
��Auto �67   .+�5$   "   �� $��0��6,   $��0��69   $��5q�6I   $��12(�6T   $
��Wrap=�6b   Search Company5&   #   �� 5'   "   $��textBox��6j   $��0��6*   $��1��67   $��30��6G   $	��100��6S   $��5q�6_   ��5(   +   �� *CompanyName+$	 PropertyChanged=�$
 TwoWay=��� 5*   "   $
��_btn��6)   $��2,2,2,2q�65   $��30��6G   $��0��6S   $��2��6`   $��Add To List��6p   ��6�   �� *AddCompanyToListCommand+�� 5,   "   $��1��6)   $��1��66   $	��100��6F   $	��150��6R   
���   �$��1q�6s   ._�5-   &   �� $
��Auto=�63   $
��Auto=�6V   .��5.   *   �� $��resultStack��65   6J   5/   '   51   #   52      53      54      56      58      �� 59   
   " ��6   .�5:      ��  � ��
 �� ��6+   �� 5;       � ��$ 12(�6)   �� 5=       � ��$
 Auto��6&   �� 5>       � ��$ 5,5,5,5q�6(   5?      5A      .��5B      �  5C   
   �� 5D      $
��Auto �6   6,   �� 5E      $
��Auto �6   6,   �� 5F      $
��Auto �6   6,   .+�5H   
     $��0��6"   #��[ 6/      ItemSourceList 5I   "   �� *AllCompanies+6L   �� 5K   
   -   5        $��CompanyStocks��5K   -   $	��200��6   $��1��6    $��5q�6C   ��6N   �� *CompanyQuotes+$	 PropertyChanged=�$
 TwoWay=� �� View 5L      �� 5M      .P�5N      �� $
��Name��6�    �� DisplayMemberBinding 6%   ��  �� Path$ Symbol�$	 PropertyChanged=�$
 TwoWay=�6�   �� 5O       �� CellTemplateSelector# [ 6%   $��Price��6]   6n   �� 5P      $��	52 wk Low��6S    6%   �� $ YearLow�6h   �� 5Q      $��
52 wk High��6T    6%   �� $ YearHigh�6j   �� 5R      $��Changes��6R    6%   �� $ Change�6e   �� 5S      $��ChangesPercentage��6]    6%   �� $ ChangesPercentage�6z   �� 5T      $��	Edit List��6%    �� CellTemplate 5U      �� 5V      .R�5W   "   ��  ��Name$ 	btnRemove��6)   $��Remove��6<   $��50��6M   $��0q�6X   $��Right=�5X   !    � Cursor$
 Hand��6E   
����  ���5W   m   �� ��5X   e   �� *DataContext.RemoveCommand+ �� RelativeSource ��  �� AncestorType$ UserControlL�5Y      5[      5\      5^      5`      5a      
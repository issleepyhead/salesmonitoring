   M S B A M L   `   `   `  ����P  LSalesMonitoringSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:SalesMonitoringSystemSalesMonitoringSystem  5      )    #SalesMonitoringSystem.ProductsPanel   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:SalesMonitoringSystem     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5         ApplicationLight#��[  5
      .��5      � #��[  6   .+�5   
   �   � RowDefinitions  5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      � $��45��6   $��0��5      $	��5 0q�5      .+�5      %  HandyControl.Controls.FlexPanel     JustifyContent$ SpaceBetween=�6   $��5 10 5 0q�5      .+�5      %  HandyControl.Controls.SearchBar  -   5          Name$ ProductSearch��5   #      
IsRealTime . '  !HandyControl.Controls.InfoElement   Placeholder$ Search Product��5   #   $	��280��5   #   $��30��5   #   $��Center=�5   #   $
��Left=�5   #     SearchBarExtend#��[ 5   #   �� 5       �� Orientation$ 
Horizontal=�6!   .+�5!      &   HandyControl.Controls.Pagination  -   5        $��
Pagination��5!   (      MaxPageCount$ 10��5"   (      	PageIndex$ 1��5#   (      MaxPageInterval$ 3��5$   (   	   DataCountPerPage$	 15��5%   (   $��Center=�5&   (   )  #HandyControl.Controls.BorderElement
   CornerRadius$
 0��5'   (   $��0 0 10 0q�5(   (   �� 5)      -   5         ��Name$ 	AddButton��5)   !   ���#fff$��9(�6F   $
 0��5*   !   $	��Add��5+   !    
 ic_add'  !HandyControl.Controls.IconElement   Geometry# [ 5,   !      Height$ 12Q�5-   !      Width$ 12Q�5.   !   $��28��5/   !   $	��120��50   !   $��0q�51   !   $��0q�52   !     ApplicationPrimaryDark#��[ 53   !   $��Center=�54   !   55      56      57      &   System.Windows.Controls.DataGrid  58      -   5          Name$ ProductDataGridView��58        CustomDataGridRowStyle   RowStyle# �  59       # CustomDataGridColumnHeaderStyle   ColumnHeaderStyle# �  5:        CustomDataGridCellStyle   	CellStyle# �  5;         
IsReadOnly .    CanUserResizeColumns .     AutoGenerateColumns .  $��1��5?      $��
10 5 10 10q�5@      !  HandyControl.Controls.Empty   	ShowEmpty . $��0q�5B         	RowHeight$ 30Q�6#      Columns 5C      0	  *System.Windows.Controls.DataGridTextColumn	  5D       	  FontSize$ 13Q�6)   ,
  &System.Windows.Controls.DataGridColumn 
  Header$ ID��5E   %    
  Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 5G   %   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5F   %   �� *ID+5G   %   	  5H      $ 13Q�6)   $ PRODUCT NAME��5I   %   $ * 5K   %    5J   %   �� *PRODUCT_NAME+5K   %   	  5L      $ 13Q�6)   $ CATEGORY��5M   %   $ * 5O   %    5N   %   �� *
CATEGORY+5O   %   	  5P      $ 13Q�6)   $ PRICE��5Q   %   $ * 5S   %    5R   %   �� *PRICE+5S   %   	  5T      $ 13Q�6)   $ 
COST PRICE��5U   %   $ * 5W   %    5V   %   �� *
COST_PRICE+5W   %   	  5X      $ 13Q�6)   $ 
DATE ADDED��5Y   %   $ * 5[   %    5Z   %   �� *
DATE_ADDED+5[   %   5]      5^      5_      5`      
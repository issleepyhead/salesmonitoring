   M S B A M L   `   `   `  ����P  LSalesMonitoringSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:SalesMonitoringSystemSalesMonitoringSystem  5      +    %SalesMonitoringSystem.InventoryDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:SalesMonitoringSystem     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      $	��550��5	      $	��800��6   .��5
      �    ApplicationPrimaryLight#��[  6     � RowDefinitions  5   
   �� 5      $��60 �6   �� 5      $��50 �6   �� 5      $��* �6   .+�5   
   '  !HandyControl.Controls.SimplePanel    ApplicationDimBlue#��[ 6   .+�5      �� $��INVENTORY PRODUCTS��6     ApplicationLight#��[ 5      $��18(�5      $
��Left=�5      $��Center=�5      $��10 0 0 0q�5      $��DemiBold"�5      �� 5      -   5        $��Closebtn��5      $��24��5      $��24��5        
ButtonIcon#��[ 5        ErrorGeometry'  !HandyControl.Controls.IconElement   Geometry# [ 5      $��hc:ControlCommands.Close��5      $��0 0 10 0q�5      $��0q�5      $��Right=�5       $��Center=�5!      5"        5#   
   $��1��6   $��0 8 0 0q�5$      .+�5%      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�5&      .+�5'      �� $��Invoice Number:��6   $��Center=�5(      $��18(�5)      $��Medium"�5*      $��10 0 0 0q�5+      �� 5,      -   5        $��ReferenceNumberLabel��5,      $��Center=�5.      $��18(�5/      $
��Bold"�50      $��4 0 0 0q�51      ��5-      �� *REF_NO+51      52      �� 53      $ 
Horizontal=�6   $��Right=�54      .+�55      �� $��Total:��6   $��Center=�56      $��18(�57      $��Medium"�58      �� 59      -   5        $��	TotalCost��59      $��0��5:      $��Center=�5;      $��18(�5<      $
��Bold"�5=      
���  ��$��4 0 30 0q�5?      5@      5A      � 5B   
   $��2��6   $��0 0 0 8q�5C      .+�5D      &   System.Windows.Controls.DataGrid  -   5        $��ProductsDataGridView��5D        CustomDataGridRowStyle   RowStyle# �  5E       # CustomDataGridColumnHeaderStyle   ColumnHeaderStyle# �  5F        CustomDataGridCellStyle   	CellStyle# �  5G        CustomDataGridStyle#���  5H         
IsReadOnly .    CanUserResizeColumns .     AutoGenerateColumns .  $��
10,0,10,10q�5L      
�������	   Columns	 5N      0  *System.Windows.Controls.DataGridTextColumn  5O      
   FontSize$
 13Q�6)   ,  &System.Windows.Controls.DataGridColumn   Header$ PRODUCT NAME��5P   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5Q   )   �� *PRODUCT_NAME+  5R      $
 13Q�6)   $ PRODUCT PRICE��5S   )    5T   )   �� *PRODUCT_PRICE+  5U      $
 13Q�6)   $ PRODUCT COST��5V   )    5W   )   �� *COST+  5X      $
 13Q�6)   $ STOCK IN��5Y   )    5Z   )   �� *
STOCK_IN+5\      5]      5^      5_      
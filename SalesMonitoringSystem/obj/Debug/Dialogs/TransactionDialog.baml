   M S B A M L   `   `   `  ����P  LSalesMonitoringSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:SalesMonitoringSystemSalesMonitoringSystem  5      -    'SalesMonitoringSystem.TransactionDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:SalesMonitoringSystem     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      $	��550��5	      $	��800��6   .��5
      � -   5          �Name$  ParentPanel��5         ApplicationPrimaryLight#��[  5
       � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��60 �6   .+�5   
   '  !HandyControl.Controls.SimplePanel    ApplicationDimBlue#��[ 6   .+�5      �� $��TRANSACTION CART��6     ApplicationLight#��[ 5      $��18(�5      $
��Left=�5      $��Center=�5      $��30 0 0 0q�5      $��DemiBold"�5      �� 5      -   5        $��Closebtn��5      $��24��5      $��24��5        
ButtonIcon#��[ 5        ErrorGeometry'  !HandyControl.Controls.IconElement   Geometry# [ 5      $��hc:ControlCommands.Close��5       $��0 0 30 0q�5!      $��0q�5"      $��Right=�5#      $��Center=�5$      5%        5&   
   $��1��6   $��0 8 0 0q�5'      .+�5(      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�5)      .+�5*      �� $��INVOICE NO:��6   $��Center=�5+      $��18(�5,      $��Medium"�5-      $��30 0 0 0q�5.      �� 5/      -   5        $��ReferenceNumberLabel��5/      $��Center=�51      $��18(�52      $
��Bold"�53      $��4 0 0 0q�54      ��50      �� *
INVOICE_NO+54      55      �� 56      $ 
Horizontal=�6   $��Right=�57      .+�58      �� $��TOTAL:��6   $��Center=�59      $��18(�5:      $��Medium"�5;      �� 5<      -   5        $��
TotalPrice��5<      $�� ��5=      $��Center=�5>      $��18(�5?      $
��Bold"�5@      $��4 0 30 0q�5A      5B      5C      � 5D   
   -   5        $��
FlexPanel1��5D      $��2��6   .+�5E      %  HandyControl.Controls.FlexPanel     JustifyContent$ FlexEnd=�6   $
��30 0q�5F      .+�5G      �� $ 
Horizontal=�6   .+�5H      �� -   5        $��AddItemButton��5H      #��[  5I      $��12(�5J      )  #HandyControl.Controls.BorderElement   CornerRadius$ 0��5K      $	��Add��5L       
 ic_add# [ 5M         Height$ 12Q�5N         Width$ 12Q�5O      $��35��5P      $	��140��5Q      $��0q�5R        ApplicationPrimaryDark#��[ 5S      5T      5U      5V      � 5W   
   -   5        $  
ThirdPanel��5Y      $��3��5W      $��0 0 0 10q�5X      .+�5Z      &   System.Windows.Controls.DataGrid  -   5        $��ItemsDataGridView��5Z        CustomDataGridRowStyle   RowStyle# �  5[       # CustomDataGridColumnHeaderStyle	   ColumnHeaderStyle#	 �  5\       	 CustomDataGridCellStyle
   	CellStyle#
 � 	 5]      !  HandyControl.Controls.Empty   	ShowEmpty . *  $HandyControl.Controls.DataGridAttach   ApplyDefaultStyle .  $��0q�5`         
IsReadOnly .    CanUserResizeColumns .     AutoGenerateColumns .  $��	30,0,30,0q�5d      $��2��5e      
�������   	RowHeight$ 30Q�5f   *      Columns 5g      0  *System.Windows.Controls.DataGridTextColumn  5h         FontSize$ 13Q�6)   ,	  &System.Windows.Controls.DataGridColumn 	  Header$ PRODUCT NAME��5i   )    	  Width5
  /System.Windows.Controls.DataGridLengthConverter$ *
 5k   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5j   )   �� *PRODUCT_NAME+5k   )     5l      $ 13Q�6)   $ QUANTITY��5m   )   $ *
 5o   )    5n   )   �� *
QUANTITY+5o   )     5p      $ 13Q�6)   $ PRICE��5q   )   $ *
 5s   )    5r   )   �� *PRICE+5s   )     5t      $ 13Q�6)   $ TOTAL��5u   )   $ *
 5w   )    5v   )   �� *TOTAL+5w   )   5y      5z      � 5{   
   -	   5        $  
FlexPanel2��5|      $��4��5{      .+�5}        $ FlexEnd=�6   .+�5~      �� -
   5        $��
SaveButton��5~      $��0 0 30 0q�5      ���#fff$��16(�5�      $ 0��5�      $
��Save��5�      $��40��5�      $	��140��5�      $��0q�5�      #��[ 5�      5�      5�      5�      5�      
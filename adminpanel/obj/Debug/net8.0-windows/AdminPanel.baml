   M S B A M L   `   `   `  ����  
adminpanel    AuthApp.AdminPanel   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Admin Panel��5   	   $	��882��6   $
��1463��6*    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � ��5   
   �� 5	       �� ImageSource$J DC:\Users\13d\Desktop\SK\Projekt\adminpanelxxx\adminpanel\Img\adf.jpg��6    j� Stretch$ UniformToFill=�5
       j� 
AlignmentY$ Bottom=�5       � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      $��* �6   .+�5   
   �� ���#3A4750$��0��6&   $��Stretch=�66    J� Opacity$	 0.8Q�6R   ._�5      �� $��20q�6   .+�5      �� $��Tablak kivalasztasa:��6   $��14(�67   
�������6E   �� 5      -   5         ��Name$ TableComboBox��5      $	��200��6d   $��0,0,0,10q�6p   �� 5      -   5         ��Name$ 	AddButton��5      $��	Hozzaadas��6,   $	��200��6X   $��0,5,0,5q�6d   �� 5      -   5        $ UpdateButton��5      $��	Modositas��6/   $	��200��6^   $��0,5,0,5q�6j   �� 5      -   5        $ DeleteButton��5      $��Torles��6/   $	��200��6[   $��0,5,0,5q�6g   �� 5      -   5        $��Mentes��5      $	��200��6C   $��0,5,0,5q�6O   �� 5      -   5        $��Kijelentkezes��5      $	��200��6L   $��0,5,0,5q�6X   5      5      � 5"   
   $��1��6   $��20q�6   .+�5#      �� $��Talalatok listaja:��6   $��20(�61   $
��Bold"�6?   $��0,0,0,10q�6Q   &   System.Windows.Controls.DataGrid  5$      -   5        	  Name$	 CarDataGrid��5$      
   AutoGenerateColumns$

 True��5%         
IsReadOnly$ False��5&      $��0,36,0,0q�5'         SelectionUnit$ FullRow=�5(      
�������$��1q�5+      $	��100��5,      $
��Left=�5-      $	��Top=�5.      $   HorizontalScrollBarVisibility$
 Auto=�5/      "   VerticalScrollBarVisibility$
 Auto=�50      $
��Auto��51      ��53      �� 54       �� Color$ 	#33FFFFFF��6&   56      57      58      59      
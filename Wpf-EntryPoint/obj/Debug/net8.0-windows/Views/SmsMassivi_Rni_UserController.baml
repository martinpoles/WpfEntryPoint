   M S B A M L   `   `   `  ����I  EWpf-EntryPoint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Wpf_EntryPoint.ViewsWpf_EntryPoint.Views  5      E'clr-namespace:Wpf_EntryPoint.ViewModelsWpf_EntryPoint.ViewModels  5      8    2Wpf_EntryPoint.Views.SmsMassivi_Rni_UserController   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Wpf_EntryPoint.Views     5      :local1'clr-namespace:Wpf_EntryPoint.ViewModels     5      .��5      � 
�������  � ColumnDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $	��200 �6   �� 5      $��* �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5!      $��* �6   �� 5#      $
��Auto �6   .+�5(   
   �� $��Menù��6   $��0��5)      $��0��5*      $��1��5+      $��5q�5,      $��Stretch=�5-      $��Center=�5.      $��20��5/      ��50      �� *$"DataContext.ShowLandingPageCommand+ �� RelativeSource ��  �� AncestorType" �(  "System.Windows.Controls.DatePicker  52   
   -   5          Name$ StartDatePicker��52      $��0��53      $��1��54      $��5q�55      $	��100��56      $	��200��57      $��Center=�58       � ToolTip$ Data di inizio��59        5<   
   -   5        $ EndDatePicker��5<      $��3��5=      $��1��5>      $��5q�5?      $	��100��5@      $	��200��5A      $��Center=�5B      $ Data di fine��5C      �� 5F   
   -   5         ��Name$ SearchTextBox1��5F      $��0��5G      $��2��5H      $��5q�5I      $	��100��5J      $	��200��5K      $��Center=�5L      5O      �� 5R   
   -   5         ��Name$ CampaignCountLabel��5R      $��1��5S      $��2��5T      $��5q�5U      $��Center=�5V      $��0 Campagne disponibili��5W      � 5Z   
   -   5         �Name$ popup1��5Z      $��False��6W   	 � PlacementTarget	 6    �� 
 �� ElementName$
 SearchTextBox1��.��5[      �� 
�������$��1q�6(   
�������._�5\      �� -   5         ��Name$ suggestionListBox1��5\      66   5]      5^      �� 5a   
   -   5         ��Name$ selectedItemsPanel1��5a      $��0��5b      $��3��5c      $��2��5d      $��5q�5e      
���  ��$��Stretch=�5g      �� 5j   
   -   5        $ SearchTextBox2��5j      $��3��5k      $��2��5l      $��5q�5m      $	��100��5n      $	��200��5o      $��Center=�5p      5s      �� 5v   
   -	   5        $ ListCountLabel��5v      $��4��5w      $��2��5x      $��5q�5y      $��Center=�5z      $��0 Liste disponibili��5{      � 5~   
   -
   5        $ popup2��5~      $��False��6W   	 6    �� $
 SearchTextBox2��.��5      �� 
�������$��1q�6(   
�������._�5�      �� -   5        $ suggestionListBox2��5�      66   5�      5�      �� 5�   
   -   5        $ selectedItemsPanel2��5�      $��3��5�      $��3��5�      $��2��5�      $��5q�5�      
����ح�$��Stretch=�5�      �� 5�   
   -   5        $��Elabora��5�      $��2��5�      $��4��5�      $��1��5�      $��5q�5�      $��Center=�5�      $��Center=�5�      $	��100��5�      $	��200��5�      $	��200��5�      $	��400��5�      5�      5�      5�      
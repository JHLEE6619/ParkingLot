   M S B A M L   `   `   `  ����B  >Client3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Client3.ViewClient3.View  5      5clr-namespace:Client3.ViewModelClient3.ViewModel  5          Client3.View.PrePayment   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Client3.View     5      .vmclr-namespace:Client3.ViewModel     5        N� Title$  
PrePayment��5
       � DataContext 5      %   Client3.ViewModel.VM_PrePayment  5   
   .��5      � 
�������.+�5   
   � $��40,100,40,50q�6   ���	#FFCACACA � RowDefinitions 5      �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-    � ColumnDefinitions 5      �� 5      $��2* �6#   60   �� 5      $��1* �6#   60   .+�5!      �� $��[차량 번호]��6   $��18(�6'   $
��bold"�65   $
��Left=�6G   $��Center=�5"      $��20,0,0,0q�63   �� 5#      $��1��6N   $	��200��6[   $��30��6g   $��18(�6s   $
��Left=�6�   $��Center=�5$      $��20,0,0,0q�61   ��5#      �� *Record.VehicleNum+ �� Mode$ OneWayToSource=�5$   1   �� 5%      -   5         ��Name$ Btn_input_VehicleNum��5%      $��1��6P   $��1��6`   $��입력��6m   $��18(�6z   $
��Left=�5&      $��Center=�60   $��80��6K   $��30��6V   $��20,0,0,0q�6b   ��5%   3   �� *
Show_fee+5&   t   �� 5'      $��2��6   $��[입차 일시]��6%   $��18(�64   $
��bold"�6B   $
��Left=�6T   $��Center=�5(      $��20,0,0,0q�60   �� 5)      $��3��6   $
��Wrap=�6T   $��18(�6h   $
��bold"�6v   $
��Left=�6�   $��Center=�5*      $��20,0,0,0q�60   ��5)   %   �� *Record.EntryDate+$ OneWay=�5*   0   �� 5+      $��4��6   $��[출차 일시]��6%   $��18(�64   $
��bold"�6B   $
��Left=�6T   $��Center=�5,      $��20,0,0,0q�60   �� 5-      $��5��6   $
��Wrap=�6S   $��18(�6g   $
��bold"�6u   $
��Left=�6�   $��Center=�5.      $��20,0,0,0q�60   ��5-   %   �� *Record.ExitDate+$ OneWay=�5.   0   �� 5/      $��6��6   $��[주차 시간]��6%   $��18(�64   $
��bold"�6B   $
��Left=�6T   $��Center=�50      $��20,0,0,0q�60   �� 51      $��7��6   $
��Wrap=�6V   $��18(�6j   $
��bold"�6x   $
��Left=�6�   $��Center=�52      $��20,0,0,0q�60   ��51   %   �� *Record.ParkingTime+$ OneWay=�52   0   �� 53      $��8��6   $
��Wrap=�6S   $��18(�6g   $
��bold"�6u   $��Right=�6�   $��Center=�54      $��0,0,20,0q�60   ��53   %   �� *Record.TotalFee+$ OneWay=�54   0   �� 55      -   5        $ Btn_Payment��55      $y�	Collapsed=�6*   $��1��6A   $��8��6Q   $��결제하기��6^   $��18(�6m   $
��Left=�56      $��Center=�60   $��80��6K   $��30��6V   $��20,0,0,0q�6b   ��6�   �� *	Payment+57      �� 58   
   $
��Left=�6   $��	30,30,0,0q�6/   $
��Wrap=�6B   $��요금 사전 정산��6V   $	��Top=�6f   $��24(�59      $
��Bold"�6   5:      5;      
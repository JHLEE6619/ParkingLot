   M S B A M L   `   `   `  ����B  >Client3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Client3.ViewClient3.View  5      5clr-namespace:Client3.ViewModelClient3.ViewModel  5          Client3.View.Registration   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Client3.View     5      .vmclr-namespace:Client3.ViewModel     5        N� Title$  Registration��5
       � DataContext 5      '   !Client3.ViewModel.VM_Registration  5   
   .��5      � 
�������.+�5   
   � $��40,100,40,50q�6   ���	#FFCACACA � RowDefinitions 5      �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-    � ColumnDefinitions 5      �� 5      $��2* �6#   60   �� 5      $��1* �6#   60   .+�5!      �� $��[차량 번호]��6   $��18(�6'   $
��bold"�65   $
��Left=�6G   $��Center=�5"      $��20,0,0,0q�60   �� 5#      $��1��6D   $	��200��6Q   $��30��6]   $��18(�6i   $
��Left=�6w   $��Center=�5$      $��20,0,0,0q�60   ��5#      �� *User.VehicleNum+ �� Mode$ TwoWay=�5$   0   �� 5%      $��2��6   $��[비밀번호]��6%   $��18(�63   $
��bold"�6A   $
��Left=�6S   $��Center=�5&      $��20,0,0,0q�60   �� 5'      $��3��6<   $	��200��6I   $��30��6U   $��18(�6a   $
��Left=�6o   $��Center=�5(      $��20,0,0,0q�60   ��5'      �� *	User.Pw+$ TwoWay=�5(   0   �� 5)      $��4��6   $��[비밀번호 확인]��6%   $��18(�66   $
��bold"�6D   $
��Left=�6V   $��Center=�5*      $��20,0,0,0q�60   �� 5+      $��5��6   $	��200��6#   $��30��6/   $��18(�6;   $
��Left=�6I   $��Center=�5,      $��20,0,0,0q�60   �� 5-      $��6��6   $��[등록 기간]��6%   $��18(�64   $
��bold"�6B   $
��Left=�6T   $��Center=�5.      $��20,0,0,0q�60   �� 5/      -   5         ��Name$ Combox_regDate��5/      $��7��6�   $��2��6�   $	��310��6�   $��30��6�   $��18(�6�   $
��Left=�6�   $��Center=�50      $��20,0,0,0q�60   ��5/   /   �� *Items+ �� SelectedItem 6M   �� *SelectedItem+$ TwoWay=�51      �� 52      -   5         ��Name$ Btn_Payment��52   6   $y�	Collapsed=�6K   $��1��6b   $��8��6r   $��결제하기��6   $��18(�6�   $
��Left=�53      $��Center=�6,   $��80��6G   $��30��6R   $��20,0,0,0q�6^   ��52      �� *Registration+53   p   54      �� 55   
   $
��Left=�6   $��	30,30,0,0q�6/   $
��Wrap=�6B   $��정기 차량 등록��6V   $	��Top=�6f   $��24(�56      $
��Bold"�6   57      58      
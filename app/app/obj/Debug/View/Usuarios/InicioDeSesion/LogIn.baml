   M S B A M L   `   `   `  ����>  :app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:app.ViewModel.Usuariosapp.ViewModel.Usuarios  5   	   _4clr-namespace:app.View.Usuarios.LogIn.CustomControls&app.View.Usuarios.LogIn.CustomControls  5   	   S.clr-namespace:app.View.Usuarios.InicioDeSesion app.View.Usuarios.InicioDeSesion  5   	   ,    &app.View.Usuarios.InicioDeSesion.LogIn   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9usuarios$clr-namespace:app.ViewModel.Usuarios     5   	   OcustomControls4clr-namespace:app.View.Usuarios.LogIn.CustomControls     5   	   @local.clr-namespace:app.View.Usuarios.InicioDeSesion     5   	   -   5          � Title$  LogIn��5   	   $	��600��6   $
��1000��6$    � WindowStyle$
 none=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency . 
������  � 	Resources 5       �� MergedDictionaries �� Source   BooleanToVisibility�� 5   
    5      �� 5       $ ../../../Dictionary1.xaml?�6%   %   &	        �� 5$      6+   5&      y�5*      �� 5+   
    �� Path$ IsViewVisible�6   	 �� Mode$	 TwoWay=�6'   
 �� 	Converter#
 [  66   .��5.      � .+�5/   
   ��  �� CornerRadius$ 20��6   
�������$��5q�66   ��50      �� 51       �� 
StartPoint$	 0,0&�6&    �� EndPoint$
 0 ,1&�67   .��52      �  � Color$ 	OrangeRed��6#    � Offset$ 1Q�65   6B   � 53      $ #FFFFFF��6#   $
 0.75Q�63   6C   � 54      $ 	OrangeRed��6#   $	 0.0Q�65   6D   55      ._�58      �  � ColumnDefinitions 59      �� 5:      �� 5;      .+�5>      �� $ 20,20,20,20��6   $��1��64   $��30,20q�6D   
���   �$��2q�6g   ._�5?      �� $��10q�6!   .+�5@      ��  �� Orientation$ 
Horizontal=�6%   $��Center=�6>   .+�5A      �� $7��1/View/Usuarios/InicioDeSesion/logo_night_days.png��6$   $	��100��6_   �� 5B      $��
Montserrat)�6(   $
��Bold"�6@   $��48(�6R   $��Center=�6`    �� TextAlignment$ Center=�6{   $	��320��6�   �� 6�    � Language$ es-es�6�   $�BasketGlobal��6�   6�   5C      �� 5D      $ 
Horizontal=�6%   $��	0,12,0,10q�6>   .+�5E      �� 
���   �$	��120��69   $��2q�6E   $��
0,20,26,20q�6Y   6o   �� 5F      $��Iniciar sesión��6(   $��Center=�6>     ColorPrimario#��[ 6Y     TextBlockGeneral#��[ 6�   �� 5G      
���   �$	��120��69   $��2q�6E   $��
20,20,0,20q�6Y   6o   5H      �� 5I      .+�5J      �� $��Email��6(   $
��0,10q�65   #��[ 6C   �� 5K      -   5        $��TextBoxEmail��5K   &     TextBoxGeneral#��[ 6:   $��0,0,0,10q�6b   .��6t   .��5L   v   ��5M   "   �� 5N   &   $ 	LogInMail�6.    �� UpdateSourceTrigger$ PropertyChanged=�6?   $	 TwoWay=�6e    �� NotifyOnValidationError .  �� ValidatesOnExceptions .  �� ValidationRules 5P   *   6   0app.View.Usuarios.InicioDeSesion.ValidacionEmail  5Q   .   5S   '   .��5T   0    =� ErrorTemplate 5U   "   �� 5V   &   .R�5W   *   �� .+�5X   .   �� 
��� E��$��1q�6M   $ 5��6a   ._�5Y   2   �� 5Z   /   �� 5[   .   
���  ����68   �� *[0].ErrorContent+6Z   5\   +   5]   '   .��5^   <   5_      �� 5a      $��Password��6(   $
��0,10q�68   #��[ 6F   @   :app.View.Usuarios.LogIn.CustomControls.BindablePasswordBox  5k      $��0,0,0,10q�6B      Password 5l   "   �� 5m   &   $ Password�6.   $ PropertyChanged=�6>   $	 TwoWay=�6d    .  .  5o   *   9   3app.View.Usuarios.InicioDeSesion.ValidacionPassword  5p   .   5r   '    5t   "   �� 5u   &   .R�5v   *   �� .+�5w   .   �� 
��� E��$��1q�6M   $ 5��6a   ._�5x   2   �� 5y   /   �� 5z   .   
���  ����68   �� *[0].ErrorContent+6Z   5{   +   5|   '   5~      �� 5�      -   5         ��Name$ ErrorMessage��5�   (     	ErrorText#��[ 6Z   ��6>   �� *
LogInError+6Z   5�      �� 5�      $
��Left=�6%   $	��100��6@   .+�5�      �� -   5        $��BtnLogin��5�   %     ButtonLogin#��[ 6V    � Cursor$
 Hand��6{   ��65   �� *LogInCommand+5�      5�      �� 5�      -   5        $��¿Recordar contraseña?��5�         LabelGeneral#��[ 6@   6f   5�      5�      �� 5�      $ 	20,0,0,20��6   }�. ��5�      �� 5�       �� ImageSource$4 ./View/Usuarios/InicioDeSesion/BasketGlobal.png��6%    j� Stretch$ UniformToFill=�6b   5�      5�      5�      5�      5�      
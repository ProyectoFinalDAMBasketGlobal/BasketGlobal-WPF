   M S B A M L   `   `   `  ����>  :app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:app.View.Usuarios.RecordarContraseñas&app.View.Usuarios.RecordarContraseñas  5   	   @    :app.View.Usuarios.RecordarContraseñas.RecordarContraseña   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Flocal4clr-namespace:app.View.Usuarios.RecordarContraseñas     5   	     � Title$  RecordarContraseña��5   	   $	��600��6$   $
��1000��61    � WindowStyle$
 none=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency . 
������  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ../../../Dictionary1.xaml?�6%   %    5      .��5      � .+�5   
   ��  �� CornerRadius$ 20��6   
�������$��5q�66   ��5      �� 5      	 �� 
StartPoint$		 0,0&�6&   
 �� EndPoint$

 0 ,1&�67   .��5      �  � Color$ 	OrangeRed��6#    � Offset$ 1Q�65   6B   � 5      $ #FFFFFF��6#   $
 0.75Q�63   6C   � 5      $ 	OrangeRed��6#   $	 0.0Q�65   6D   5      ._�5      �  � ColumnDefinitions 5       �� 5!      �� 5"      .+�5%      �� $ 20,20,20,20��6   $��1��64   $��30,20q�6D   
���   �$��2q�6g   ._�5&      �� .+�5'      ��  �� Orientation$ 
Horizontal=�6%   .+�5(      �� $ 
Horizontal=�6)   $��Center=�6B   $	��432��6]   .+�5)   "   �� $7��1/View/Usuarios/InicioDeSesion/logo_night_days.png��6(   $	��100��6c   �� 5*   "   $��
Montserrat)�6,   $
��Bold"�6D   $��49(�6V   $��Center=�6d   �� 6�    � Language$ es-es�6�   $�BasketGlobal��6�   6�   5+      5,      �� 5-      $ 
Horizontal=�6%   $��Center=�6>   $��Center=�6[   $��	0,12,0,10q�6v   .+�5.      �� 
���   �$��50��69   $��2q�6D   $��
0,20,20,20q�6X   6n   �� 5/      $ ��He olvidado mi contraseña��6(   $��Center=�6I      ColorPrimario#��[  6d     TextBlockGeneral#��[ 6�   �� 50      
���   �$��50��69   $��2q�6D   $��
20,20,0,20q�6X   6n   51      �� 52      .+�53      �� $��Email��6(   $
��0,10q�65   #��[ 6C   �� 54      -   5        $��txtEmail��54   &     TextBoxGeneral#��[ 66   6^   �� 55      -   5         ��Name$ ErrorTextEmail��55   (   $��* Email no válido��6@     	ErrorText#��[ 6Y   �� 56      $��Confirmar Email��6(   $
��0,10q�6?   #��[ 6M   �� 57      -   5        $��txtEmailConfirmar��57   &   #��[ 6?   6g   �� 58      -   5        $ ErrorTextEmailConfirmar��58   (   $��* Email no válido��6I   #��[ 6b   �� 59      -   5        $ ErrorTextEquals��59   (   $��* Email no coincide��6A   #��[ 6\   5<      �� 5=      $
��Left=�6%   .+�5>      �� -   5        $��	btnEnviar��5>   %   {�.    ButtonLogin#��[ 6H    � Cursor$
 Hand��6�   5A      5B      �� 5C      -   5        $��Volver al LogIn...��5C         LabelGeneral#��[ 6=   6c   5D      5E      �� 5G      
������ $��0q�64   $ 	20,0,0,20��6H   ��5H      �� 5I       �� ImageSource$3 -/View/Usuarios/RecordarContraseñas/lobby.jpg��6*    j� Stretch$ UniformToFill=�6e   5K      5M      5N      5O      5P      
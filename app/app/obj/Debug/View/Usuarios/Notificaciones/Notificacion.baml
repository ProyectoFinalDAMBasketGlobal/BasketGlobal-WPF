   M S B A M L   `   `   `  ����>  :app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:app.View.Usuarios.Notificaciones app.View.Usuarios.Notificaciones  5   	   3    -app.View.Usuarios.Notificaciones.Notificacion   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:app.View.Usuarios.Notificaciones     5   	   -   5          � Title$  NotificacionMensaje��5   	   $	��600��6%   $
��1000��62    � WindowStyle$
 None=�5	   	    � 
ResizeMode$ NoResize=�5
   	   
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ../../../Dictionary1.xaml?�6%   %    5      .��5      � .+�5   
   ��  �� CornerRadius$ 20��6   ���	#CC000000$��5q�6:   ��5      �� 5      	 �� 
StartPoint$		 0,0&�6&   
 �� EndPoint$

 0 ,1&�67   .��5      �  � Color$ 	OrangeRed��6#    � Offset$ 1Q�65   6B   � 5      $ #FFFFFF��6#   $
 0.75Q�63   6C   � 5      $ 	OrangeRed��6#   $	 0.0Q�65   6D   5      ._�5      �� -   5         ��Name$ BtnCorreoEnviado��5      
������ ��5       �� 5!      $ 30��6   $��2q�6/   
��� ���
�������$	��503��6k   $	��111��6w   ._�5"      �� .+�5#      �� -   5         ��Name$ 	txtHeader��5#   (   $��Email Enviado��6;   
��� ���$��36(�6h   $��
Montserrat)�6v   $��Center=�6�   $��Heavy"�6�   $��0,10,0,0q�6�   �� 5$      -   5        $ 
txtContent��5$   (   $
��Wrap=�6<   $��Por favor revise su email��6Q   
��� d �$��16(�6�   $��
Montserrat)�6�   $��Center=�6�   $��Normal"�6�   $��0,10,0,0q�6�   5%      5&      ��5(      �� 5)       �� 
TargetType" ��6   .�5*      ��  �� Property � �� �� Value 5+      �� 5,   "    �� 
TargetType" ��62   .R�5-   &   ��  �� Triggers 5.   &   P� 5/   *    P� Property �@J�IsMouseOver P� Value . . �50   .   ��  � ��
 ���� 6K   �� 51   .    �@�Cursor$
 Hand��6G   52   +   54   #   56      57      59      5;      5<      5=      
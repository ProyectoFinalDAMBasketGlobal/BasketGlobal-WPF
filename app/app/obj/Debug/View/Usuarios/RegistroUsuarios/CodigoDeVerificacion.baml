   M S B A M L   `   `   `  ����>  :app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:app.View.Usuarios.RegistroUsuarios"app.View.Usuarios.RegistroUsuarios  5   	   =    7app.View.Usuarios.RegistroUsuarios.CodigoDeVerificacion   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Blocal0clr-namespace:app.View.Usuarios.RegistroUsuarios     5   	   -   5          � Title$  CodigoDeVerificacion��5   	   $	��600��6&   $
��1000��63    � WindowStyle$
 None=�5	   	    � 
ResizeMode$ NoResize=�5
      
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ../../../Dictionary1.xaml?�6%   %    5      .��5      � .+�5   
   ��  �� CornerRadius$ 20��6   ���	#CC000000$��5q�6:   ��5      �� 5      	 �� 
StartPoint$		 0,0&�6&   
 �� EndPoint$

 0 ,1&�67   .��5      �  � Color$ 	OrangeRed��6#    � Offset$ 1Q�65   6B   � 5      $ #FFFFFF��6#   $
 0.75Q�63   6C   � 5      $ 	OrangeRed��6#   $	 0.0Q�65   6D   5      ._�5      �� $	��320��6   $	��500��6"   ���#D9D9D9$ 20��6C   ._�5       �� $��Center=�6    $��Center=�6;   .+�5!      �� -   5         ��Name$ 	btnCerrar��5!      $��	390,0,0,0q�60   
������ 
������ $��40��6v   $��40��6�   .��5"      �� $/��)/View/Usuarios/RegistroUsuarios/Close.png��6    6V   5#      �� 5$      $	��400��6!   .+�5%      �� $��Verificacón de tu email��6$   
��� E��$
��Bold"�6Z   $��30(�6l   $��0,0,0,20q�6z   �� 5&      $
��Wrap=�6$      TextBlockGeneral#��[  69   $��20,0,0,0q�6c   A?"Se ha enviado un código de verificaión al siguiente correo: �� 5'   ]   -   5         ��Name$ 	TextEmail��5'   g   ���#CD6CE0#��[  6�   6�    introduzcalo..."5(      �� 5)      -   5         ��Name$ 	txtCodigo��5)   "   $��10q�65   $��16(�6A   $��35��6O   .��6�    5*       �� 
TargetType �� Property �� Value��%^   '��       �� 5+   "   " ��6(   .�5,   &   ��  �@��CornerRadius$ 5��6E   5-   #   5.      .��61   5/      �� 50      -   5        $ ErrorTextCodigo��50   $   $��20,0,0,0q�6=   $��* Email no válido��6O     	ErrorText#��[ 6h   �� 51      -   5        $ 	btnEnviar��51   !   $��Enviar��64   {�.  $��30��6W   $	��120��6c   
��� ������#FFFFFF$��18(�6�   $��Medium"�6�   
���   �$��Right=�6�   $��0,20,0,0q�6�    52      ��%_   '��       �� 53   "   " ��6(   .�54   &   ��  �@��CornerRadius$ 15��6E   55   #   56      57      58      5:      5;      5=      5>      5?      
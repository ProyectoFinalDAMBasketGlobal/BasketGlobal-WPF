   M S B A M L   `   `   `  ����>  :app, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:app.View.Reservasapp.View.Reservas  5   	   '    !app.View.Reservas.InfoAdquisicion   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:app.View.Reservas     5   	     � Title$  InfoAdquisicion��5   	   $	��600��6!   $
��1000��6.    � WindowStyle$
 None=�5	   	    � 
ResizeMode$ NoResize=�5
   	   
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5         ColorPrimario  ColorSecundario  
ColorFondo  
ColorTexto  BrushColorPrimario �� Color  BrushColorSecundario  BrushColorFondo  BrushColorTexto��%?  &	        &	      &	 >     &	 ]     &	 |     7   &	 �     7  &	 �     7  &	 �     ��5   
   #FF4500��6/   ��5   
   #FFA500��61   ��5   
   #2D2B3C��6,   ��5   
   #FFFFFF��6,   �� 5   
   8   65   �� 5   
   8   67   �� 5   
   8   62   �� 5   
   $ Black��62   5      .��5      � .+�5   
   ��  �� CornerRadius$ 20��6   ���	#CC000000$��5q�6:   ��5       �� 5!       �� 
StartPoint$	 0,0&�6&   	 �� EndPoint$
	 0 ,1&�67   .��5"      � 
 � Color#
 [ 6#    � Offset$ 1Q�6L   � 5#      $
 #FFFFFF��6#   $
 0.75Q�63   � 5$      #
 [  6#   $	 0.0Q�6J   5%      ._�5(      �� $��1��6   $	��265��6"   $	��500��6.   $ 20��6;   $��8 5 8 5q�6M   ��5)      �� 5*      $	 0,0&�6*   $		 1,0&�6;   .��5+      � $
 #8A73C1��6'   $ 0Q�67   � 5-      $
 #FFA500��6'   $ 1Q�67   5/      ��51      �� 52      $ 	#FFD9D9D9��6&   ._�54      �� $��Center=�6   $��Center=�6:   .+�55      �� $��Adquisicion��6    $��26(�63   $	��157��6A   $��0 0 0 30q�6M   ���#8A73C1 �� TextAlignment$ Center=�6t   6�   �� 56      -   5         ��Name$ 	txtNombre��56       $��Producto adquirido por:��63   $��Center=�6R   $��14(�6o   $��0,10,0,0q�6}   
����+��6�   �� 57      -   5        $ 
txtUsuario��57       $��Nombre de Usuario��64   $��Center=�6M   $��14(�6j   $��0,10,0,0q�6x   #��[ 6�   �� 59      $��	Producto:��6    $��Center=�61   $��14(�6N   $��0,10,0,0q�6\   
����+��6n   �� 5:      -   5        $ txtProducto��5:       $��Nombre del Producto��65   $��Center=�6P   $��14(�6m   $��0,10,0,0q�6{   #��[ 6�   �� 5<      $��Fecha Adquisicion:��6    $��Center=�6:   $��14(�6W   $��0,10,0,0q�6e   
����+��6w   �� 5=      -   5        $ txtFechaEntrada��5=       $��
10-02-2025��69   $��Center=�6K   $��14(�6h   $��0,10,0,0q�6v   #��[ 6�   �� 5?      $��	Cantidad:��6    $��Center=�61   $��14(�6N   $��0,10,0,0q�6\   
����+��6n   �� 5@      -   5        $ txtCantidad��5@       $��1��65   $��Center=�6>   $��14(�6[   $��0,10,0,0q�6i   #��[ 6{   �� 5B      $��Estado Adquisicion:��6    $��Center=�6;   $��14(�6X   $��0,10,0,0q�6f   
����+��6x   �� 5C      -   5        $ txtEstadoAdquisicion��5C       $��	Pendiente��6>   $��Center=�6O   $��14(�6l   $��0,10,0,0q�6z   #��[ 6�   �� 5E      -   5         ��Name$ 	btnCerrar��5E      $��0 20 0 0q�60   $��45��6B   $	��160��6N   $��Center=�5F      $��Cerrar��68   #��[ 5G      
���� ��$��3 2 3 4q�6c   $��18(�6}    5H       �� 
TargetType �� Property �� Value��%_   '��       �� 5I      " ��6$   .�5J   "   ��  �@��CornerRadius$ 20��6A   5K      5L      5M      5N      5O      5Q      5R      5S      
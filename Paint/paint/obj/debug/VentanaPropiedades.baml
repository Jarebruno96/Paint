   M S B A M L   `   `   `  ����@  <Paint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PaintPaint  5   	       Paint.VentanaPropiedades   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Paint     5   	     � Title$  Propiedades��5   	   $	��550��6   $	��400��6*    � Icon$* $Imagenes/IconoVentanaPropiedades.jpg��66   .��5	      � 
����ح� � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      �� 5      $
��1.5* �6   �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��9q�6;   $
��Auto��6F   $��Center=�6T   $��Center=�6o   .+�5      �� $	��Top=�6   $��Center=�60   �� 5      $��20(�6   .�6"   B� .�6,   Propiedades69   6E   5      �� 5      $��Center=�6   $��Center=�63   $��9q�6P   	Miniatura5      �� 5       $	��100��6   $	��150��6)   $��Center=�65   $��Center=�6P   
������� � ToolTip$$ Previsualización del elemento��6�   $
��Auto=�6�   $
��Auto=�6�   .��5!      �� -   5        $��miniaturaElemento��5!      $J DAquí se muestra la miniatura del elemento con el que está tratando��61   5#      5$      5&      � 5)   
   $��1��6    � Orientation$ 
Horizontal=�6!   $��Center=�6:   $��Center=�6U   .+�5+      �� $ 
Horizontal=�6   $��3q�62   .+�5,      �� $��3q�6   ID6+   �� 5-      -   5        $��id��5-      $��50��6&   $��3q�64   $��70��6?   
�������$  Identificador del elemento��6`   6�   5.      �� 50      $ 
Horizontal=�6   $��3q�62   .+�51      �� $��3q�6   Tipo6-   �� 52      -   5        $
��tipo��52      $��50��6(   $��70��66   $��3q�6D   
�������$ Tipo del elemento��6b   6�   53      54      � 57   
   $��2��6   $ Vertical=�6!   $��Center=�68   $��Center=�6S   .+�59      �� $ 
Horizontal=�6   $��Center=�62   .+�5:      �� $��5q�6   $��70��6'   Color Contorno6E   �� 5;      $��3q�6   $% Color del contorno del elemento��6#   .��5<      �� -   5        $��colorContorno��5<       $��15��65   $��15��6A   
�������6[   5=      �� 5>      -   5        $��bColorC��5>      $��3q�6(   $@ :Pulse aqui para cambiar el color del contorno del elemento��6I   .��6�   	Dar Color6�   �� 5?      $��5q�6   $5 /Indique si quiere que el color sea transparente��6'   Tranparente6n   �� 5@      -   5        $��bCCTransparente��5@      $��5q�63   $��15��6>   6j   5A      �� 5C      $ 
Horizontal=�6   $��Center=�62   .+�5D      �� $��3q�6   $��70��6'   Grosor Contorno6F   �� 5E      -   5        $��	gContorno��5E      $��3q�6+   
�������$��40��6I   $��75��6W   $&  Grosor del contorno del elemento��6e   6�   5G      �� 5I      $ 
Horizontal=�6   $��Center=�62   .+�5J      �� $��3q�6   $��82��6'   Color Fondo6B   �� 5K      $��3q�6   $" Color del Fondo del elemento��6#   .��5L      �� -   5        $��
colorFondo��5L       $��15��62   $��15��6>   
�������6X   5M      �� 5N      -	   5        $��bColorF��5N      $��3q�6(   $= 7Pulse aqui para cambiar el color del fondo del elemento��6I   .��6�   	Dar Color6�   �� 5O      $��5q�6   $5 /Indique si quiere que el color sea transparente��6'   Tranparente6n   �� 5P      -
   5        $��bCFTransparente��5P      $��5q�63   $��15��6>   6j   5Q      5R      � 5U   
   $��3��6   $ Vertical=�6!   $��3q�68   $��Center=�6C   $	��Top=�6`   .+�5W      �� $ 
Horizontal=�6   .+�5X      �� $��3q�6   $��60��6'   	P. Origen6@   �� 5Y      -   5        $��pInicio��5Y      $��3q�6)   $��40��64   $��75��6B   $4 .Punto de inicio en el que comienza el elemento��6P   6�   5Z      �� 5\      $ 
Horizontal=�6   .+�5]      �� $��3q�6   $��60��6'   P. Fin6=   �� 5^      -   5        $
��pFin��5^      $��3q�6&   $��40��61   $��75��6?   $, &Punto final en que termina el elemento��6M   6�   5_      �� 5a      $ 
Horizontal=�6   .+�5b      �� $��3q�6   $��60��6'   Alto6;   �� 5c      -   5        $
��alto��5c      $��3q�6&   $��40��61   $��75��6?   $&  Tamaño en el eje Y del elemento��6M   6y   5d      �� 5f      $ 
Horizontal=�6   .+�5g      �� $��3q�6   $��60��6'   Ancho6<   �� 5h      -   5        $��ancho��5h      $��3q�6'   $��40��62   $��75��6@   $&  Tamaño en el eje X del elemento��6N   6z   5i      5j      � 5m   
   $��4��6   $ Vertical=�6!   $��Center=�68   $��Center=�6S   .+�5n      �� $ 
Horizontal=�6   $��Center=�62   $��Center=�6M   .+�5o      �� $��3q�6   Angulo de rotacion6;   �� 5p      -   5        $��rotacion��5p      $��3q�6*   $��40��65   $��75��6C   $B <Angulo de rotacion que se le proporcionará a dicho elemento��6Q   6�   5q      5r      � 5u   
   $��5��6   $��Center=�6!   $��Center=�6<   $ Vertical=�6Y   .+�5w      �� $ 
Horizontal=�6   $��Center=�62   $��Center=�6M   .+�5x      �� $��3q�6   Fuente De Letra68   �� 5y      -   5        $��selectorFuente��5y      $��3q�6/   $	��100��6:   $U OPulsar aquí si se desean cambiar las caracteristicas de la fuente del elemento��6f   .��6�   Fuente De Letra6�   5z      �� 5|      $ 
Horizontal=�6   $��Center=�62   $��Center=�6M   .+�5}      �� $��3q�6   	Contenido62   �� 5~      -   5        $��	contenido��5~      $��3q�6+   $	��120��66   $	��200��6E   6V   5      5�      �� 5�   
   $��6��6   $��Center=�6"   $��Center=�6=   $ 
Horizontal=�6Z   .+�5�      �� -   5        $��bAplicar��5�      $��15q�6%   $< 6Pulse aqui para previsualizar como quedara el elemento��6H   .��6�   	Aplicar6�   �� 5�      -   5        $��bGuardar��5�      $��15q�6%   $F @Pulse aquí para guardar las nuevas caracteristicas del elemento��6H   .��6�   Guardar Cambios6�   �� 5�      -   5        $��	bCancelar��5�      $��15q�6&   $\ VPulse aquí si desea salir de las propiedades del elemento sin realizar ningún cambio��6J   .��6�   
Cancelar6�   5�      5�      5�      
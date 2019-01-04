using Paint.Elementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Paint.Herramientas;
using System.Diagnostics;

namespace Paint
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //Variables de clase
        public event opcionesElementoEventHandler añadir;
        public event opcionesElementoEventHandler cargar;
        public event opcionesElementoEventHandler eliminar;
        public event opcionesElementoEventHandler modificar;

        private Line lineaReglaBot = new Line();
        private Line lineaReglaLef = new Line();
        private Color colorContorno = Color.FromArgb(255,0,0,0);
        private Color colorFondo = Color.FromArgb(0, 255, 255, 255);
        private bool flagDibujando = false;
        private bool flagDesplazando = false;
        private bool flagVentanaAbierta = false;
        private double grosor=1;
        DibujoBase d;
        UIElement temp; //Variable temporal que uso para modificar o eliminar el elemento seleccionado
        Point pClickado;

        /// <summary>
        /// Constructor de la ventana principal
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            
        }
        /// <summary>
        /// Evento que se produce cuando quiero cambiamos las dimensiones de la ventana, hay que registrarlo para poder mantener actualizadas las reglas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            cargarValoresRegla();
        }

        /// <summary>
        /// Metodo que recarga los valores de las reglas para mantener actualizados los valores de estas, para poner bien los valores, toma como refrencia la posición en la que se encuentran las barras de desplazamiento del ScroolViewer
        /// </summary>
        public void cargarValoresRegla()
        {
            Line lineaMedida;

            int comienzoEnEjeX = (Int32)sV.HorizontalOffset;
            int comienzoEnEjeY = (Int32)sV.VerticalOffset;

            reglaLef.Children.Clear();
            for (int i=0; i< lienzo.ActualHeight;i++) {

                lineaMedida = new Line();
                lineaMedida.Stroke = Brushes.Black;
                lineaMedida.X1 = 0;
                lineaMedida.Y1 = i;
                lineaMedida.Y2 = i;
                //Cada 50 puntos, una raya larga, y cada 10, una raya corta
                if ( (i % 50) == 0) {
                   
                    lineaMedida.X2 = reglaLef.ActualWidth;
                    
                    reglaLef.Children.Add(lineaMedida);
                    TextBlock numero = new TextBlock();
                    numero.Text = (i+comienzoEnEjeY).ToString();
                    numero.FontSize = 8;
                    numero.FontWeight = FontWeights.UltraBold;
                    Canvas.SetTop(numero, i + 1);
                    Canvas.SetLeft(numero,reglaLef.ActualWidth);
                    numero.RenderTransform = new RotateTransform(90,0,0);
                    reglaLef.Children.Add(numero);
                }
                else if ((i % 10) == 0)
                {
                    lineaMedida.X2 = reglaLef.ActualWidth/2;
                    reglaLef.Children.Add(lineaMedida);
                }
                
            }

            reglaBot.Children.Clear();
            for (int i = 0; i < lienzo.ActualWidth; i++)
            {
                lineaMedida = new Line();
                lineaMedida.Stroke = Brushes.Black;
                lineaMedida.X1 = i;
                lineaMedida.Y2 = reglaLef.ActualHeight;
                lineaMedida.X2 = i;
                //Cada 50 puntos, una raya larga, y cada 10, una raya corta
                if ((i % 50) == 0)
                {
                    lineaMedida.Y1 = 0;
                    reglaBot.Children.Add(lineaMedida);
                    TextBlock numero = new TextBlock();
                    numero.Text = (i + comienzoEnEjeX).ToString();
                    numero.FontSize = 8;
                    numero.FontWeight = FontWeights.UltraBold;
                    Canvas.SetLeft(numero,i+1);
                    reglaBot.Children.Add(numero);
                }
                else if( (i % 10)==0)
                {
                    lineaMedida.Y1 = reglaBot.ActualHeight/2;
                    reglaBot.Children.Add(lineaMedida);
                }
            }
        }

        /// <summary>
        /// Evento que se dispara en cuanto se termina de cargar de la ventana, se usa para cargar por primera vez las reglas, y para poner los barras que se desplazan junto con la posicion del ratón, además. hay que desactivamos los botones
        /// de modificar y eliminar un elemento, por que no tenemos nada seleccionado aun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cargarValoresRegla();
            cargarDesplazadores();
            bEliminarElemento.IsEnabled = false;
            bModificarElemento.IsEnabled = false;
        }

        /// <summary>
        /// Metodo que nos permite iniciar los desplazadores, se inician en la 0;0
        /// </summary>
        private void cargarDesplazadores()
        {
            lineaReglaLef.X1 = 0;
            lineaReglaLef.Y1 = 0;
            lineaReglaLef.X2 = reglaLef.ActualWidth;
            lineaReglaLef.Y2 = 0;
            lineaReglaLef.Stroke = Brushes.Red;
            reglaLef.Children.Add(lineaReglaLef);

            lineaReglaBot.X1 = 0;
            lineaReglaBot.Y1 = 0;
            lineaReglaBot.Y2 = reglaBot.ActualHeight;
            lineaReglaBot.X2 = 0;
            lineaReglaBot.Stroke = Brushes.Red;
            reglaBot.Children.Add(lineaReglaBot);
        }

        /// <summary>
        /// Evento que se produce cuando deseamos abrir la ventana que contiene el resumen de los elementos. Se controla ademas, que no pueda estar desplegada mas de una ventana mostrando este resumen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!flagVentanaAbierta)
            {
                flagVentanaAbierta = true;
                VentanaResumenFiguras vRF = new VentanaResumenFiguras();
                vRF.Owner = this;
                /*Cuando ocurran los eventos de la ventana hija, se ejecutan estos metodos*/
                vRF.modificarEnCanvas += modificarElemento;
                vRF.añadirEnCanvas += añadirElemento;
                vRF.eliminarEnCanvas += eliminarElemento;
                vRF.Closed += cambiarFlagDesplegado;
                /*Cuando ocurran los eventos de esta clase, se ejecutan esos metodos de la clase hija*/
                añadir += vRF.insertarNuevoElemento;
                cargar += vRF.cargarElementosDibujados;
                eliminar += vRF.eliminarElemento;
                modificar += vRF.modificarElemento;



                for (int i = 0; i < lienzo.Children.Count; i++)
                {
                    DibujoBase dB = elementoSeleccionadoDelCanvas(lienzo.Children[i]);
                    CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                    argumentos.dB = dB;
                    OnCargar(argumentos);

                }

                vRF.Show();
            }
            else
            {
                MessageBox.Show("La ventana ya se encuentra desplegada","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }

        /// <summary>
        /// Cuando se cierra la ventana con el resumen de los elementos, hay que indicar que se puede volver a desplegar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cambiarFlagDesplegado(object sender, EventArgs e)
        {
            flagVentanaAbierta = false;
        }

        /// <summary>
        /// Método que permite actualizar un elemento dentro del lienzo
        /// </summary>
        /// <param name="dB"> Elemento que vamos a modificar en el lienzo</param>

        private void modificarElemento(DibujoBase dB)
        {
            for (int i=0; i<lienzo.Children.Count;i++)
            {
                if (lienzo.Children[i].Uid.Equals(dB.ID))
                {
                    

                    switch (dB.Tipo)
                    {
                        case "Rectangulo":
                            (lienzo.Children[i] as Rectangle).Height = (dB as Figura).Alto;
                            (lienzo.Children[i] as Rectangle).Width = (dB as Figura).Ancho;
                            (lienzo.Children[i] as Rectangle).Stroke = new SolidColorBrush((dB as Figura).ColorContorno);
                            (lienzo.Children[i] as Rectangle).Fill = new SolidColorBrush((dB as Figura).ColorFondo);
                            (lienzo.Children[i] as Rectangle).StrokeThickness = (dB as Figura).GrosorContorno;
                            (lienzo.Children[i] as Rectangle).RenderTransform = new RotateTransform((dB as Figura).Rotacion, (dB as Figura).Ancho / 2, (dB as Figura).Alto / 2);
                            Canvas.SetLeft((lienzo.Children[i] as Rectangle), (dB as Figura).PuntoInicio.X);
                            Canvas.SetTop((lienzo.Children[i] as Rectangle), (dB as Figura).PuntoInicio.Y);
                            break;
                        case "Elipse":
                            (lienzo.Children[i] as Ellipse).Height = (dB as Figura).Alto;
                            (lienzo.Children[i] as Ellipse).Width = (dB as Figura).Ancho;
                            (lienzo.Children[i] as Ellipse).Stroke = new SolidColorBrush((dB as Figura).ColorContorno);
                            (lienzo.Children[i] as Ellipse).Fill = new SolidColorBrush((dB as Figura).ColorFondo);
                            (lienzo.Children[i] as Ellipse).StrokeThickness = (dB as Figura).GrosorContorno;
                            (lienzo.Children[i] as Ellipse).RenderTransform = new RotateTransform((dB as Figura).Rotacion, (dB as Figura).Ancho / 2, (dB as Figura).Alto / 2);
                            Canvas.SetLeft((lienzo.Children[i] as Ellipse), (dB as Figura).PuntoInicio.X);
                            Canvas.SetTop((lienzo.Children[i] as Ellipse), (dB as Figura).PuntoInicio.Y);
                            break;
                        case "Recta":
                            (lienzo.Children[i] as Line).Stroke = new SolidColorBrush((dB as Recta).ColorContorno);
                            (lienzo.Children[i] as Line).X1 = (dB as Recta).PuntoInicio.X;
                            (lienzo.Children[i] as Line).Y1 = (dB as Recta).PuntoInicio.Y;
                            (lienzo.Children[i] as Line).X2 = (dB as Recta).PuntoFin.X;
                            (lienzo.Children[i] as Line).Y2 = (dB as Recta).PuntoFin.Y;
                            (lienzo.Children[i] as Line).StrokeThickness = (dB as Recta).GrosorContorno;
                            break;
                        case "Texto":
                            (lienzo.Children[i] as TextBlock).Text = (dB as Texto).Contenido;
                            (lienzo.Children[i] as TextBlock).Background = new SolidColorBrush((dB as Texto).ColorFondo);
                            (lienzo.Children[i] as TextBlock).Foreground = new SolidColorBrush((dB as Texto).ColorContorno);
                            (lienzo.Children[i] as TextBlock).FontFamily = (dB as Texto).TipoLetra;
                            (lienzo.Children[i] as TextBlock).FontSize = (dB as Texto).TamañoFuente;
                            (lienzo.Children[i] as TextBlock).FontStyle = (dB as Texto).EstiloFuente;
                            (lienzo.Children[i] as TextBlock).Height = (dB as Texto).Alto;
                            (lienzo.Children[i] as TextBlock).Width = (dB as Texto).Ancho;
                            (lienzo.Children[i] as TextBlock).RenderTransform = new RotateTransform((dB as Texto).Rotacion,(dB as Texto).Ancho/2, (dB as Texto).Alto / 2);
                            Canvas.SetLeft((lienzo.Children[i] as TextBlock), (dB as Texto).PuntoInicio.X);
                            Canvas.SetTop((lienzo.Children[i] as TextBlock), (dB as Texto).PuntoInicio.Y);
                            break;
                        case "Mano Alzada":
                            (lienzo.Children[i] as Polyline).Stroke = new SolidColorBrush((dB as ManoAlzada).ColorContorno);
                            (lienzo.Children[i] as Polyline).Points = (dB as ManoAlzada).Puntos;
                            (lienzo.Children[i] as Polyline).StrokeThickness = (dB as ManoAlzada).GrosorContorno;
                            break;
                    }

                    CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                    argumentos.dB = elementoSeleccionadoDelCanvas(lienzo.Children[i]);
                    OnModificar(argumentos);
                    break;
                    
                }
            }
            
        }

        /// <summary>
        /// Metodo que permite añadir un elemento al lienzo
        /// </summary>
        /// <param name="dB">Es el elemento que vamos a añadir al lienzo</param>
        private void añadirElemento(DibujoBase dB)
        {
            Object o = dB.convertirAElementoVisual();
            //(o as UIElement).MouseLeftButtonUp += new MouseButtonEventHandler(lienzo_MouseLeftButtonUp);
            if (opcionCursor.IsChecked==true)
            {
                (o as UIElement).MouseLeftButtonDown += new MouseButtonEventHandler(visualizarOpcionesDeElemento);

            }
            //(o as UIElement).MouseLeftButtonDown += new MouseButtonEventHandler(visualizarOpcionesDeElemento);

            if ((dB is Texto) || (dB is Figura))
            {
                Canvas.SetTop(o as UIElement, dB.PuntoInicio.Y);
                Canvas.SetLeft(o as UIElement, dB.PuntoInicio.X);
            }

            lienzo.Children.Add(o as UIElement);

        }

        /// <summary>
        /// Metodo que permite eliminar un elemento dentro del lienzo
        /// </summary>
        /// <param name="dB">Elemento que queremos eliminar</param>
        /// <returns>Devuelve true si el elemento fue eliminado, y false, si no se pudo eliminar el elemetno</returns>
        private void eliminarElemento(DibujoBase dB)
        {
            for (int i=0; i<lienzo.Children.Count;i++)
            {
                if ((dB.ID.Equals(lienzo.Children[i].Uid)))
                {
                    
                    lienzo.Children.Remove(lienzo.Children[i]);
                    break;
                }
            }
        }
        /// <summary>
        /// Evento que se produce al mover el raton por el lienzo, gracias a este evento, podemos recalcular en todo momento la posicion de los deplazadores, y si estamos dibujando, modificar el elemento que estamos dibuijando en tiempo real
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lienzo_MouseMove(object sender, MouseEventArgs e)
        {
            //Para los desplazadores
            reglaLef.Children.Remove(lineaReglaLef);
            lineaReglaLef.Y1 = lineaReglaLef.Y2 = e.GetPosition(lienzo).Y - sV.VerticalOffset;
            reglaLef.Children.Add(lineaReglaLef);

            reglaBot.Children.Remove(lineaReglaBot);
            lineaReglaBot.X1 = lineaReglaBot.X2 = e.GetPosition(lienzo).X - sV.HorizontalOffset;
            reglaBot.Children.Add(lineaReglaBot);

            if(flagDesplazando && temp!=null)
            {
                if (temp is Rectangle || temp is Ellipse || temp is TextBlock)
                {
                    Canvas.SetTop(temp,e.GetPosition(lienzo).Y);
                    Canvas.SetLeft(temp, e.GetPosition(lienzo).X);
                }else if (temp is Line)
                {
                    double posIniEjeX = (temp as Line).X1;
                    double posIniEjeY = (temp as Line).Y1;

                    (temp as Line).X1 = e.GetPosition(lienzo).X;
                    (temp as Line).Y1 = e.GetPosition(lienzo).Y;
                    (temp as Line).X2 += e.GetPosition(lienzo).X - posIniEjeX;
                    (temp as Line).Y2 += e.GetPosition(lienzo).Y - posIniEjeY;


                }
                else if (temp is Polyline)
                {
                    double posIniEjeX = (temp as Polyline).Points[0].X;
                    double posIniEjeY = (temp as Polyline).Points[0].Y;

                    for (int i=0; i<(temp as Polyline).Points.Count;i++)
                    {
                        if (i==0)
                        {
                            (temp as Polyline).Points[i] = new Point(e.GetPosition(lienzo).X, e.GetPosition(lienzo).Y);
                        }else
                        {
                            (temp as Polyline).Points[i] = new Point((temp as Polyline).Points[i].X + e.GetPosition(lienzo).X-posIniEjeX, (temp as Polyline).Points[i].Y+ e.GetPosition(lienzo).Y-posIniEjeY);
                        }
                    }
                }
            }
            //Si estamos dibujando
            if(flagDibujando){
                Point puntoRaton = e.GetPosition(lienzo);
                /*En caso de ser una recta*/
                if (opcionRecta.IsChecked==true)
                {
                    (d as Recta).PuntoFin = e.GetPosition(lienzo);
                    modificarElemento(d);
                }
                /*En caso de ser dibujo a mano alzada*/
                if (opcionManoAlzada.IsChecked == true)
                {
                    (d as ManoAlzada).Puntos.Add(e.GetPosition(lienzo));
                    modificarElemento(d);
                }
                /*En caso de que sea una figura*/
                /*Hay que, en funcion del punto clickado que es fijo, y el punto en el que se encuentra el raton, ver cual es la altura y anchura resultante, y ver donde queda finalmente el punto de inicio para poder colocarlo*/
                if(opcionRectangulo.IsChecked == true || opcionElipse.IsChecked == true){

                    if(puntoRaton.X < pClickado.X){
                        (d as Figura).Ancho = pClickado.X - puntoRaton.X;
                        d.PuntoInicio = new Point(puntoRaton.X,d.PuntoInicio.Y); //Solo tendria que hacer d.PuntoInicio.X= puntoRaton.X, pero no me deja hacerlo jajaja 
                    }
                    else
                    {
                        (d as Figura).Ancho = puntoRaton.X - pClickado.X;
                        d.PuntoInicio = new Point(pClickado.X, puntoRaton.Y);
                    }

                    if(puntoRaton.Y < pClickado.Y){
                        (d as Figura).Alto = pClickado.Y - puntoRaton.Y;
                        d.PuntoInicio = new Point(d.PuntoInicio.X, puntoRaton.Y);
                    }
                    else
                    {
                        (d as Figura).Alto =  puntoRaton.Y - pClickado.Y;
                        d.PuntoInicio = new Point(d.PuntoInicio.X, pClickado.Y);

                    }

                    modificarElemento(d);
                    
                }
                /*En caso de que sea texto*/
                /*Hay que, en funcion del punto clickado que es fijo, y el punto en el que se encuentra el raton, ver cual es la altura y anchura resultante, y ver donde queda finalmente el punto de inicio para poder colocarlo*/

                if (opcionTexto.IsChecked==true)
                {
                    if (puntoRaton.X < pClickado.X)
                    {
                        (d as Texto).Ancho = pClickado.X - puntoRaton.X;
                        d.PuntoInicio = new Point(puntoRaton.X, d.PuntoInicio.Y);
                    }
                    else
                    {
                        (d as Texto).Ancho = puntoRaton.X - pClickado.X;
                        d.PuntoInicio = new Point(pClickado.X, puntoRaton.Y);
                    }

                    if (puntoRaton.Y < pClickado.Y)
                    {
                        (d as Texto).Alto = pClickado.Y - puntoRaton.Y;
                        d.PuntoInicio = new Point(d.PuntoInicio.X, puntoRaton.Y);
                    }
                    else
                    {
                        (d as Texto).Alto = puntoRaton.Y - pClickado.Y;
                        d.PuntoInicio = new Point(d.PuntoInicio.X, pClickado.Y);

                    }

                    modificarElemento(d);
                }

            }

            
        }

        /// <summary>
        /// Evento que hay que registrar, para que en caso de que nos salgamos del lienzo mientras estamos dibujando, considerar que ya hemos terminado de dibujar el elemento o de desplazarlo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lienzo_MouseLeave(object sender, MouseEventArgs e)
        {
            if (flagDibujando || flagDesplazando)
            {
                terminarDibujo();
                
            }
            
        }

        /// <summary>
        /// Evento que hay que registrar para que cuando queramos dibujar algo, podamos crear dicho elemento en funcion del tipo de elemento que queremos añadir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lienzo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            bEliminarElemento.IsEnabled = false;
            bModificarElemento.IsEnabled = false;
            elementoSeleccionado.Content = null;

            /*Si es un dibujo a mano alzada*/
            if (opcionManoAlzada.IsChecked == true)
            {
                flagDibujando = true;
                d = new ManoAlzada("Mano Alzada");
                d.PuntoInicio = e.GetPosition(lienzo);
                d.ColorContorno = colorContorno;
                (d as ManoAlzada).GrosorContorno = grosor;
                lienzo.Children.Add(d.convertirAElementoVisual() as UIElement);
            }
            /*Si es un rectangulo*/
            if(opcionRectangulo.IsChecked == true){

                flagDibujando = true;
                d = new Figura("Rectangulo");
                pClickado = e.GetPosition(lienzo);
                d.PuntoInicio = e.GetPosition(lienzo);
                d.ColorContorno = colorContorno;
                (d as Figura).GrosorContorno = grosor;
                (d as Figura).ColorFondo = colorFondo;
                lienzo.Children.Add( (d.convertirAElementoVisual() as UIElement));
            }
            /*Si es una elipse*/
            if (opcionElipse.IsChecked==true)
            {
                flagDibujando = true;
                d = new Figura("Elipse");
                pClickado = e.GetPosition(lienzo);
                d.PuntoInicio = e.GetPosition(lienzo);
                d.ColorContorno = colorContorno;
                (d as Figura).GrosorContorno = grosor;
                (d as Figura).ColorFondo = colorFondo;
                lienzo.Children.Add((d.convertirAElementoVisual() as UIElement));
            }
            /*Si es un texto*/
            if (opcionTexto.IsChecked == true)
            {
                flagDibujando = true;
                d = new Texto("Texto");
                pClickado = e.GetPosition(lienzo);
                d.PuntoInicio = e.GetPosition(lienzo);
                d.ColorContorno = colorContorno;
                (d as Texto).ColorFondo = colorFondo;
                lienzo.Children.Add(d.convertirAElementoVisual() as UIElement);
            }

            /*Si es una recta*/
            if (opcionRecta.IsChecked == true)
            {
                flagDibujando = true;
                d = new Recta("Recta");
                pClickado = e.GetPosition(lienzo);
                d.PuntoInicio = e.GetPosition(lienzo);
                (d as Recta).PuntoFin = new Point(d.PuntoInicio.X,d.PuntoInicio.Y);
                d.ColorContorno = colorContorno;
                (d as Recta).GrosorContorno = grosor;
                lienzo.Children.Add((d.convertirAElementoVisual() as UIElement));
            }


        }

        /// <summary>
        /// Evento que hay que registrar para poder saber cuando hemos terminado de dibujar nuestro elemento con el raton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lienzo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            terminarDibujo();
        }

        /// <summary>
        /// Metodo que se llama para terminar un dibujo que estamos haciendo o para indicar que hemos terminado de desplazarlo
        /// </summary>
        public void terminarDibujo()
        {
            if (flagDibujando)
            {
                flagDibujando = false;
                if (opcionTexto.IsChecked == true)
                {
                    VentanaIntroducirTexto vIT = new VentanaIntroducirTexto();
                    if (vIT.ShowDialog() == true)
                    {
                        (d as Texto).Contenido = vIT.Contenido;
                        (d as Texto).TamañoFuente = vIT.TamañoFuente;
                        (d as Texto).TipoLetra = vIT.TipoLetra;
                        (d as Texto).EstiloFuente = vIT.EstiloFuente;
                        modificarElemento(d);
                    }

                }

                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                argumentos.dB = d;
                OnAñadir(argumentos);
            }
            if (flagDesplazando == true && temp != null)
            {
                flagDesplazando = false;
                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                argumentos.dB = elementoSeleccionadoDelCanvas(temp);
                OnModificar(argumentos);
            }
        }
        /// <summary>
        /// Evento que gestiona cuando añadimos algo al canvas para que apareza en el listView
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAñadir(CambioEnElementoEventArgs e)
        {
            if (this.añadir!=null)
            {
                this.añadir(this,e);
            }
        }
        /// <summary>
        /// Evento que permite cargar el listView con todas los elementos del canvas
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCargar(CambioEnElementoEventArgs e)
        {
            if (this.cargar!=null)
            {
                this.cargar(this,e);
            }
        }

        /// <summary>
        /// Evento que permite eliminar un elemento del listView cuando eliminamos un elemento del canvas
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEliminar(CambioEnElementoEventArgs e)
        {
            if (this.eliminar != null)
            {
                this.eliminar(this, e);
            }
        }

        /// <summary>
        /// Evento que permite eliminar un elemento del listView cuando eliminamos un elemento del canvas
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnModificar(CambioEnElementoEventArgs e)
        {
            if (this.modificar != null)
            {
                this.modificar(this, e);
            }
        }

        /// <summary>
        /// Evento que permite modificar el color por defecto  del contorno de los elementos que añadiremos con el ratón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectorColorContornoPorDefecto_Click(object sender, RoutedEventArgs e)
        { 
            System.Windows.Forms.ColorDialog cD = new System.Windows.Forms.ColorDialog();
            cD.SolidColorOnly = false;
            if (cD.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                colorContorno = Color.FromArgb(cD.Color.A, cD.Color.R, cD.Color.G, cD.Color.B);
                colorContornoPorDefecto.Fill = new SolidColorBrush(colorContorno);
            }
        }

        /// <summary>
        /// Evento que permite modificar el color por defecto  del contorno de los elementos que añadiremos con el ratón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectorColorFondoPorDefecto_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cD = new System.Windows.Forms.ColorDialog();
            cD.SolidColorOnly = false;
            cD.AnyColor = true;
            cD.FullOpen = false;
            cD.ShowHelp = true;
            if (cD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                colorFondo = Color.FromArgb(cD.Color.A, cD.Color.R, cD.Color.G, cD.Color.B);
                colorFondoPorDefecto.Fill = new SolidColorBrush(colorFondo);

            }
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee guardar el dibujo que estárealizando para poder modficarlo después
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guardarRibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            guardar();
        }

        /// <summary>
        /// Metodo que nos permite guardar lo que llevamos hecho del dibujo
        /// </summary>
        public void guardar()
        {
            Microsoft.Win32.SaveFileDialog sFD = new Microsoft.Win32.SaveFileDialog();

            sFD.Filter = "Text Files|*.txt";
            String[] lineas = new String[lienzo.Children.Count + 1];
            lineas[0] = lienzo.Width + Utils.c + lienzo.Height;
            if (sFD.ShowDialog() == true)
            {


                for (int i = 0; i < lienzo.Children.Count; i++)
                {
                    DibujoBase dB = elementoSeleccionadoDelCanvas(lienzo.Children[i]);
                    lineas[i + 1] = dB.misDatosAString();
                }

                String nombreFichero = sFD.FileName;
                System.IO.File.WriteAllLines(nombreFichero, lineas);
            }
        }

        /// <summary>
        /// Evento que necesitamos registrar para cuando el usuario desee abrir un dibujo que haya realizado previamente y poder trabajar con el
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abrirRibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog oFD = new Microsoft.Win32.OpenFileDialog();
            oFD.Filter = "Text Files|*.txt";
            if (oFD.ShowDialog()== true)
            {
                String[] lineas = System.IO.File.ReadAllLines(oFD.FileName);
                try
                {
                    String[] dimensiones = lineas[0].Split(Utils.c[0]);
                    lienzo.Width = Double.Parse(dimensiones[0]);
                    lienzo.Height = Double.Parse(dimensiones[1]);
                    for (int i = 1; i < lineas.Count(); i++)
                    {
                        if (lineas[i].Contains("Mano Alzada"))
                        {
                            ManoAlzada mA = new ManoAlzada();
                            if (mA.desglosarInfo(lineas[i]))
                            {
                                añadirElemento(mA);
                                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                                argumentos.dB = mA;
                                OnAñadir(argumentos);
                            }
                            else
                            {
                                MessageBox.Show("Se ha producido un error al leer un dibujo a mano alzada");
                            }

                        }
                        else if (lineas[i].Contains("Texto"))
                        {
                            Texto t = new Texto();
                            if (t.desglosarInfo(lineas[i]))
                            {
                                añadirElemento(t);
                                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                                argumentos.dB = t;
                                OnAñadir(argumentos);
                            }
                            else
                            {
                                MessageBox.Show("Se ha producido un error al leer un Texto");
                            }
                        }
                        else if (lineas[i].Contains("Recta_"))
                        {
                            Recta r = new Recta();
                            if (r.desglosarInfo(lineas[i]))
                            {
                                añadirElemento(r);
                                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                                argumentos.dB = r;
                                OnAñadir(argumentos);
                            }else
                            {
                                MessageBox.Show("Se ha producido un error al leer una Recta");

                            }
                        }
                        else
                        {
                            Figura f = new Figura();
                            if (f.desglosarInfo(lineas[i]))
                            {
                                añadirElemento(f);
                                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                                argumentos.dB = f;
                                OnAñadir(argumentos);

                            }
                            else
                            {
                            MessageBox.Show("Se ha producido un error al leer una Recta");

                            }
                        }
                    }
                }
                catch(FormatException)
                {
                    MessageBox.Show("El archivo no es valido","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }

            }
            
            
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee exportar el lienzo a .png
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportarRibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (lienzo == null)
                return;
            //Sacamos un cuadro de dialogo preguntando donde vamos a guardar el archivo .png
            Microsoft.Win32.SaveFileDialog sFD = new Microsoft.Win32.SaveFileDialog();
            //Indicamos el tipo de archivo con el filtro
            sFD.Filter =" PNG Files|*png" ;

            if (sFD.ShowDialog()==true)
            {
                //Rect es una estructura que contiene las 4 dimensiones de un elemento 
                Rect bounds = VisualTreeHelper.GetDescendantBounds(lienzo);

                //Tamaño que queremos renderizar, densidad de pixelesm y el formato del mapa de bits
                RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

                //DrawingVisual es un elemento visual que se puede renderizar, y de hecho, es lo que renderizamos
                DrawingVisual dv = new DrawingVisual();
                //Abrimos esa intancia para la representacion, el valor devuelto, se puede usar para representar el drawing visual
                using (DrawingContext dc = dv.RenderOpen())
                {
                    //Un visualBrush pinta un area con un elemento visual
                    VisualBrush vb = new VisualBrush(lienzo);
                    //Pinta en esa representacion visual, un rectangulo que contiene ese visualBrush, es decir, "el lienzo como si fuese una brocha"
                    //Como no neceistamos un pen, el segundo parametro es null
                    //Explicacion de la linea entera, mete en esa representacion, un rectagulo, dado por las dimensiones del rectangulo, la brocha que contiene la imagen de ese elemetno visual
                    dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }
                //Renderizamos el drawingVisual
                rtb.Render(dv);
                //Codificamos en formato
                PngBitmapEncoder png = new PngBitmapEncoder();

                png.Frames.Add(BitmapFrame.Create(rtb));

                // y Lo guardamos
                using (Stream stm = File.Create(sFD.FileName+".png"))
                {
                    png.Save(stm);
                }
            }


        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee eliminar el elemento que tiene seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bEliminarElemento_Click(object sender, RoutedEventArgs e)
        {
            if (temp!=null)
            {
                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                argumentos.dB = elementoSeleccionadoDelCanvas(temp);
                OnEliminar(argumentos);
                lienzo.Children.Remove(temp);
                bModificarElemento.IsEnabled = false;
                bEliminarElemento.IsEnabled = false;
                elementoSeleccionado.Content = null;
            }
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario selecciona un elemento presente en el lienzo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualizarOpcionesDeElemento(object sender, MouseButtonEventArgs e)
        {
            bEliminarElemento.IsEnabled = true;
            bModificarElemento.IsEnabled = true;
            temp = sender as UIElement;
            elementoSeleccionado.Content = elementoSeleccionadoDelCanvas(temp).convertirAElementoVisual();
            if (opcionMover.IsChecked == true)
            {
                flagDesplazando = true;
            }            
            e.Handled = true;
        }

        /// <summary>
        /// Evento que registramos para cuando para cuando deseamos modificar las propiedades del elemento que tenemos seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bModificarElemento_Click(object sender, RoutedEventArgs e)
        {
            if (temp !=null)
            {
                VentanaPropiedades vP = new VentanaPropiedades();
                vP.elementoSeleccionado += indicarElementoEnCanvasParaModificar;
                vP.indicarElemento();

                if (vP.ShowDialog()==true)
                {
                    modificarElemento(vP.Ori);
                    elementoSeleccionado.Content = vP.Ori.convertirAElementoVisual();
                }
                
            }
        }

        /// <summary>
        /// Metodo que recibiendo un elemento presente dentro del canvas, nos devuelve un objeto de su correspondiete clase con todos sus datos
        /// </summary>
        /// <param name="elemento">Elemento presente dentro del canvas</param>
        /// <returns>Objeto con los datos correspondientes</returns>
        public DibujoBase elementoSeleccionadoDelCanvas(UIElement elemento)
        {
            DibujoBase dB = null;
            if (elemento is Rectangle)
            {
                dB = new Figura();
                dB.ID = elemento.Uid;
                dB.Tipo = "Rectangulo";
                dB.ColorContorno = ((elemento as Rectangle).Stroke as SolidColorBrush).Color;
                (dB as Figura).ColorFondo = ((elemento as Rectangle).Fill as SolidColorBrush).Color;
                (dB as Figura).Alto = (elemento as Rectangle).Height;
                (dB as Figura).Ancho = (elemento as Rectangle).Width;
                (dB as Figura).GrosorContorno = (elemento as Rectangle).StrokeThickness;
                dB.PuntoInicio = new Point(Canvas.GetLeft(elemento), Canvas.GetTop(elemento));

                if ((elemento as Rectangle).RenderTransform != null)
                {

                    (dB as Figura).Rotacion = ((elemento as Rectangle).RenderTransform as RotateTransform).Angle;
                }

            }
            else if (elemento is Ellipse)
            {
                dB = new Figura();
                dB.ID = elemento.Uid;
                dB.Tipo = "Elipse";
                dB.ColorContorno = ((elemento as Ellipse).Stroke as SolidColorBrush).Color;
                (dB as Figura).GrosorContorno = (elemento as Ellipse).StrokeThickness;
                (dB as Figura).ColorFondo = ((elemento as Ellipse).Fill as SolidColorBrush).Color;
                (dB as Figura).Alto = (elemento as Ellipse).Height;
                (dB as Figura).Ancho = (elemento as Ellipse).Width;
                (dB as Figura).PuntoInicio = new Point(Canvas.GetLeft(elemento), Canvas.GetTop(elemento));

                if ((elemento as Ellipse).RenderTransform != null)
                {
                    (dB as Figura).Rotacion = ((elemento as Ellipse).RenderTransform as RotateTransform).Angle;
                }

            }
            else if (elemento is Line)
            {
                dB = new Recta();
                dB.ID = elemento.Uid;
                dB.Tipo = "Recta";
                dB.ColorContorno = ((elemento as Line).Stroke as SolidColorBrush).Color;
                dB.PuntoInicio = new Point((elemento as Line).X1, (elemento as Line).Y1);
                (dB as Recta).PuntoFin = new Point((elemento as Line).X2, (elemento as Line).Y2);
                (dB as Recta).GrosorContorno = (elemento as Line).StrokeThickness;

            }
            else if (elemento is Polyline)
            {
                dB = new ManoAlzada();
                dB.ID = elemento.Uid;
                dB.Tipo = "Mano Alzada";
                dB.ColorContorno = ((elemento as Polyline).Stroke as SolidColorBrush).Color;
                (dB as ManoAlzada).GrosorContorno = (elemento as Polyline).StrokeThickness;
                dB.PuntoInicio = new Point((elemento as Polyline).Points[0].X, (elemento as Polyline).Points[0].Y);
                (dB as ManoAlzada).Puntos = (elemento as Polyline).Points;
            }
            else if (elemento is TextBlock)
            {
                dB = new Texto();
                dB.ID = elemento.Uid;
                dB.Tipo = "Texto";
                dB.ColorContorno = ((elemento as TextBlock).Foreground as SolidColorBrush).Color;
                (dB as Texto).ColorFondo = ((elemento as TextBlock).Background as SolidColorBrush).Color;
                dB.PuntoInicio = new Point(Canvas.GetLeft(elemento), Canvas.GetTop(elemento));
                (dB as Texto).Contenido = (elemento as TextBlock).Text;
                (dB as Texto).TamañoFuente = (elemento as TextBlock).FontSize;
                (dB as Texto).TipoLetra = (elemento as TextBlock).FontFamily;
                (dB as Texto).Alto = (elemento as TextBlock).Height;
                (dB as Texto).Ancho = (elemento as TextBlock).Width;
                if ((elemento as TextBlock).RenderTransform != null)
                {
                    (dB as Texto).Rotacion = ((elemento as TextBlock).RenderTransform as RotateTransform).Angle;
                }

            }
            return dB;
        }

        /// <summary>
        /// Evento que nos devuelve el elemento del canvas que tenemos seleccionado
        /// </summary>
        /// <returns></returns>
        public DibujoBase indicarElementoEnCanvasParaModificar()
        {
            return elementoSeleccionadoDelCanvas(temp);
        }

        /// <summary>
        /// Metodo que deshabilita el evento mouseDown de los elementos del lienzo
        /// </summary>
        private void deshabilitarSeleccion()
        {
            for (int i=0; i<lienzo.Children.Count;i++)
            {
                lienzo.Children[i].MouseLeftButtonDown -= visualizarOpcionesDeElemento;
            }
        }

        /// <summary>
        /// Metodo que habilita el evento mouseDown de los elementos del lienzo
        /// </summary>
        private void habilitarSeleccion()
        {
            for (int i = 0; i < lienzo.Children.Count; i++)
            {
                lienzo.Children[i].MouseLeftButtonDown += visualizarOpcionesDeElemento;
            }
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee borrar todos los elementos del canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borrarTodoRibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement i in lienzo.Children)
            {

                CambioEnElementoEventArgs argumentos = new CambioEnElementoEventArgs();
                argumentos.dB = elementoSeleccionadoDelCanvas(i);
                OnEliminar(argumentos);
            }
            lienzo.Children.Clear();
        }

        /// <summary>
        /// Evento que se produce para cambiar el grosor del contorno por defecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
        private void fino_Click(object sender, RoutedEventArgs e)
        {
            grosor = 1;
        }
        /// <summary>
        /// Evento que se produce para cambiar el grosor del contorno por defecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void normal_Click(object sender, RoutedEventArgs e)
        {
            grosor = 5;
        }

        /// <summary>
        /// Evento que se produce para cambiar el grosor del contorno por defecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void ancho_Click(object sender, RoutedEventArgs e)
        {
            grosor = 10;
        }

        /// <summary>
        /// Evento que registramos para que cuando desplacemos el lienzo con los barras de desplazamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sV = sender as ScrollViewer;
            cargarValoresRegla();
            
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee modificar las dimensiones del lienzo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCambiarCoordenadas_Click(object sender, RoutedEventArgs e)
        {
            VentanaIntroducirDimensiones vID = new VentanaIntroducirDimensiones();
            vID.Owner = this;

            if (vID.ShowDialog() == true)
            {
                lienzo.Height = vID.Altura;
                lienzo.Width = vID.Anchura;
            }
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario desee consultar el manual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bMostrarManual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"Manual.pdf");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("No se ha encontrado el manual","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Evento que registramos para que antes de de salir, el usuario pueda guardar el dibujo que lleva hecho
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void v_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult valor = MessageBox.Show("¿Desea Guardar antes de salir?","Advertencia",MessageBoxButton.YesNoCancel,MessageBoxImage.Question);

            switch (valor)
            {
                case MessageBoxResult.Yes:
                    guardar();
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }

        /// <summary>
        /// Metodo que nos permiter modificar el hecho de que podamos seleccinar un elemento o no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onOffSeleccion (object sender, RoutedEventArgs e) {

            System.Windows.Controls.Ribbon.RibbonRadioButton boton = sender as System.Windows.Controls.Ribbon.RibbonRadioButton;
            
            if (boton.Label.Equals(opcionMover.Label) || boton.Label.Equals(opcionCursor.Label))
            {
                habilitarSeleccion();
            }
            else
            {
                deshabilitarSeleccion();
            }
        }

    }
}

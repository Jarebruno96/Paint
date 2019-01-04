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
using System.Windows.Shapes;
using Paint.Elementos;
using System.Windows.Forms;


namespace Paint
{
    /// <summary>
    /// Lógica de interacción para VentanaPropiedades.xaml
    /// </summary>
    public delegate DibujoBase seleccionarElemento();

    public partial class VentanaPropiedades : Window
    {
       public  event seleccionarElemento elementoSeleccionado;

        //Variables de clase
        DibujoBase cop;
        DibujoBase ori;
        //propiedades de acceso a los atributos
        public DibujoBase Ori
        {
            get { return ori; }
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public VentanaPropiedades()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Metodo con el que cargamos la informacion del elemento seleccionado en las cajas de texto de la ventana
        /// </summary>
        public void indicarElemento()
        {
            ori = elementoSeleccionado();
         
            if (ori != null)
            {
                cop = ori.duplicar();
                
                miniaturaElemento.Content = cop.convertirAElementoVisual();

                cargarInformacion(cop);
            }
            else
            {
                System.Windows.MessageBox.Show("No hay ninguna figura seleccionada", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                this.IsEnabled = false;
                this.Close();
            }
        }

        /// <summary>
        /// Evento que registramos para poder guardar los cambios que hayamos hecho en la figura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (aplicarCambios())
            {
                ori.copiar(cop);
                DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Hay campos de informacion erroneos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Evento que registramos para poder cerrar la ventana sin guardar los cambios que hayamos hecho
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Método con el que cargamos la informacion del parametro en las distintas cajas de texto
        /// </summary>
        /// <param name="db"></param>
        private void cargarInformacion(DibujoBase db)
        {
            /*Asignamos la informacion de los campos comunes*/
            id.Text = db.ID;
            tipo.Text  = db.Tipo;       
            colorContorno.Fill = new SolidColorBrush(db.ColorContorno);
            pInicio.Text = db.PuntoInicio.ToString();

            switch (db.Tipo)
            {
                case "Texto":
                    /*Desactivamos los campos que no son propios de los textos*/
                    pFin.IsEnabled = false;
                    gContorno.IsEnabled = false;
                    /*Introducimos la informacion de los campos específicos de los Textos*/
                    colorFondo.Fill = new SolidColorBrush((db as Texto).ColorFondo);
                    alto.Text = (db as Texto).Alto.ToString();
                    ancho.Text = (db as Texto).Ancho.ToString();
                    rotacion.Text = (db as Texto).Rotacion.ToString();
                    contenido.Text = (db as Texto).Contenido;
                    break;
                case "Mano Alzada":
                    /*Desactivamos los campos que no son propios de los dibujos a Mano Alzada*/
                    colorFondo.IsEnabled = false;
                    bColorF.IsEnabled = false;
                    rotacion.IsEnabled = false;
                    selectorFuente.IsEnabled = false;
                    contenido.IsEnabled = false;
                    pInicio.IsEnabled = false;
                    gContorno.Text = (db as ManoAlzada).GrosorContorno.ToString();
                    
                    break;
                case "Recta":
                    /*Desactivamos los campos que no son propios de las rectas*/
                    alto.IsEnabled = false;
                    ancho.IsEnabled = false;
                    rotacion.IsEnabled = false;
                    bColorF.IsEnabled = false;
                    selectorFuente.IsEnabled = false;
                    contenido.IsEnabled = false;
                    /*Introducimos la informacion de los campos específicos de las rectas*/
                    pFin.Text = (db as Recta).PuntoFin.ToString();
                    gContorno.Text = (db as Recta).GrosorContorno.ToString();
                    break;
                default:
                    /*Desactivamos los campos que no son propios de las figuras*/
                    selectorFuente.IsEnabled = false;
                    contenido.IsEnabled = false;
                    pFin.IsEnabled = false;
                    /*Introducimos la informacion de los campos específicos de las figuras*/
                    colorFondo.Fill = new SolidColorBrush((db as Figura).ColorFondo);
                    alto.Text = (db as Figura).Alto.ToString();
                    ancho.Text = (db as Figura).Ancho.ToString();
                    rotacion.Text = (db as Figura).Rotacion.ToString();
                    gContorno.Text = (db as Figura).GrosorContorno.ToString();
                    break;
            }
        }

        /// <summary>
        /// Evento con el que previsualizamos los cambios que estamos haciendo al elemento, pero sin llegar a guardalos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAplicar_Click(object sender, RoutedEventArgs e)
        {

            if (!aplicarCambios())
            {
                System.Windows.MessageBox.Show("Hay campos de informacion erroneos.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                miniaturaElemento.Content = cop.convertirAElementoVisual();
                //miniaturaElemento.Content = Utils.convertirAFigura(cop as Figura);
            }

        }

        /// <summary>
        /// Metodo que con el aplicamos los cambios de las cajas de texto en el elemento
        /// </summary>
        /// <returns>Devuelve un parametro de control, true, si los datos son correctos, false, si los datos son erroneos</returns>
        public bool aplicarCambios()
        {
            bool validos = true ;
            String[] punto = null;
            switch (cop.Tipo)
            {
                case "Texto":

                    /*Damos el valor del contenido*/
                    if (contenido.Text.Contains(Utils.c[0]))
                    {
                        validos = false;
                        contenido.Background = Brushes.LightPink;
                    }else
                    {
                        contenido.Background = Brushes.White;
                        (cop as Texto).Contenido = contenido.Text;
                    }
                    /*Validamos que el valor de la rotacion sea correcto*/
                    try
                    {
                        rotacion.Background = Brushes.White;
                        (cop as Texto).Rotacion = Double.Parse(rotacion.Text);


                    }
                    catch (FormatException)
                    {
                        rotacion.Background = Brushes.LightPink;
                        validos = false;
                    }


                    /*Validamos que el punto de origen sea correcto*/
                    pInicio.Background = Brushes.White;

                    punto = puntoValido(pInicio.Text);

                    if (punto == null)
                    {
                        pInicio.Background = Brushes.LightPink;
                        validos = false;
                    }
                    else
                    {
                        try
                        {
                            (cop as Texto).PuntoInicio = new Point(Double.Parse(punto[0]), Double.Parse(punto[1]));
                        }
                        catch (FormatException)
                        {
                            validos = false;
                            pInicio.Background = Brushes.LightPink;
                        }
                    }


                    /*Validamos que la altura sea valida*/

                    alto.Background = Brushes.White;
                    try
                    {
                        (cop as Texto).Alto = Double.Parse(alto.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        alto.Background = Brushes.LightPink;
                    }

                    /*Validamos que la anchura sea valida*/

                    ancho.Background = Brushes.White;
                    try
                    {
                        (cop as Texto).Ancho = Double.Parse(ancho.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        ancho.Background = Brushes.LightPink;
                    }
                    return validos;
                    /*********************************************************/
                    /*SI ES UNA RECTA EL ELEMENTO CON EL QUE ESTAMOS TRATANDO*/
                    /*********************************************************/
                case "Recta":
                    /*Validamos que el punto de origen sea correcto*/
                    pInicio.Background = Brushes.White;
                    punto = null;
                    punto = puntoValido(pInicio.Text);

                    gContorno.Background = Brushes.White;
                    try
                    {
                        (cop as Recta).GrosorContorno = Double.Parse(gContorno.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        gContorno.Background = Brushes.LightPink;
                    }

                    if (punto == null)
                    {
                        pInicio.Background = Brushes.LightPink;
                        validos = false;
                    }
                    else
                    {
                        try
                        {
                            (cop as Recta).PuntoInicio = new Point(Double.Parse(punto[0]), Double.Parse(punto[1]));
                        }
                        catch (FormatException)
                        {
                            validos = false;
                            pInicio.Background = Brushes.LightPink;
                        }
                    }

                    /*Validamos que el punto final sea correcto*/
                    pInicio.Background = Brushes.White;
                    punto = null;
                    punto = puntoValido(pFin.Text);

                    if (punto == null)
                    {
                        pInicio.Background = Brushes.LightPink;
                        validos = false;
                    }
                    else
                    {
                        try
                        {
                            (cop as Recta).PuntoFin = new Point(Double.Parse(punto[0]), Double.Parse(punto[1]));
                        }
                        catch (FormatException)
                        {
                            validos = false;
                            pFin.Background = Brushes.LightPink;
                        }
                    }

                    return validos;
                case "Mano Alzada":
                    gContorno.Background = Brushes.White;
                    try
                    {
                        (cop as ManoAlzada).GrosorContorno = Double.Parse(gContorno.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        gContorno.Background = Brushes.LightPink;
                    }
                    return true;
                default:
                    /**************************************************************************/
                    /*SI ES UNA FIGURA EL ELEMENTO QUE ESTAMOS MODIFICANDO                    */
                    /*************************************************************************/
                    gContorno.Background = Brushes.White;
                    try
                    {
                        (cop as Figura).GrosorContorno = Double.Parse(gContorno.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        gContorno.Background = Brushes.LightPink;
                    }

                    /*Validamos que el valor de la rotacion sea correcto*/
                    try
                    {
                        rotacion.Background = Brushes.White;
                        (cop as Figura).Rotacion = Double.Parse(rotacion.Text);


                    }
                    catch (FormatException)
                    {
                        rotacion.Background = Brushes.LightPink;
                        validos = false;
                    }


                    /*Validamos que el punto de origen sea correcto*/
                    pInicio.Background = Brushes.White;

                    punto = puntoValido(pInicio.Text);

                    if (punto == null)
                    {
                        pInicio.Background = Brushes.LightPink;
                        validos = false;
                    }
                    else
                    {
                        try
                        {
                            (cop as Figura).PuntoInicio = new Point(Double.Parse(punto[0]), Double.Parse(punto[1]));
                        }
                        catch (FormatException)
                        {
                            validos = false;
                            pInicio.Background = Brushes.LightPink;
                        }
                    }


                    /*Validamos que la altura sea valida*/

                    alto.Background = Brushes.White;
                    try
                    {
                        (cop as Figura).Alto = Double.Parse(alto.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        alto.Background = Brushes.LightPink;
                    }

                    /*Validamos que la anchura sea valida*/

                    ancho.Background = Brushes.White;
                    try
                    {
                        (cop as Figura).Ancho = Double.Parse(ancho.Text);
                    }
                    catch (FormatException)
                    {
                        validos = false;
                        ancho.Background = Brushes.LightPink;
                    }
                    return validos;
            }
        }

        /// <summary>
        /// Metodo que comprueba la validacion de un punto
        /// </summary>
        /// <param name="punto">punto del que vamos a comprobar su validez</param>
        /// <returns>decuelve true si es valido, devuelve false si no es valido</returns>
        private String[] puntoValido(String punto) {

            String[] temp = null;           
                if (punto.Contains(";"))
                {
                    temp = punto.Trim().Split(';');
                    if (temp.Length == 2)
                    {
                            return temp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
        }

        /// <summary>
        /// Evento que registramos para poder cambiar el color de contorno del elemento que estamos tratando
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bColorC_Click(object sender, RoutedEventArgs e)
        {

            ColorDialog cD = new ColorDialog();
            if (cD.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                Color color = Color.FromArgb(cD.Color.A, cD.Color.R, cD.Color.G, cD.Color.B);
                cop.ColorContorno = color;
                colorContorno.Fill =new SolidColorBrush(color);

            }


        }

        /// <summary>
        /// Evento que registramos para poder cambiar el color de contorno del elemento que estamos tratando
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bColorF_Click(object sender, RoutedEventArgs e)
        {

            ColorDialog cD = new ColorDialog();
            if (cD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color color = Color.FromArgb(cD.Color.A, cD.Color.R, cD.Color.G, cD.Color.B);
                colorFondo.Fill = new SolidColorBrush(color);
                switch (cop.Tipo)
                {
                    case "Texto":
                        (cop as Texto).ColorFondo = color;
                        break;
                    default:
                        (cop as Figura).ColorFondo = color;
                        break;

                }

            }
        }

        /// <summary>
        /// Evento que registramos para poder cambiar el tipo de fuente y el tamaño del contenido del texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectorFuente_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fD = new FontDialog();
            
            if (fD.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                (cop as Texto).TipoLetra = new FontFamily(fD.Font.FontFamily.Name);
                (cop as Texto).TamañoFuente = fD.Font.Size;
            }
            
        }

        /// <summary>
        /// Evento que registramos para poder poner el color del contorno como transparente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCCTransparente_Click(object sender, RoutedEventArgs e)
        {
            if (bCCTransparente.IsChecked==true)
            {
                cop.ColorContorno = Color.FromArgb(0,cop.ColorContorno.R,cop.ColorContorno.G,cop.ColorContorno.B);
            }else
            {
                cop.ColorContorno = Color.FromArgb(255, cop.ColorContorno.R, cop.ColorContorno.G, cop.ColorContorno.B);
            }
        }

        /// <summary>
        /// Evento que registramos para poder poner el color del fondo como transparente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCFTransparente_Click(object sender, RoutedEventArgs e)
        {

            switch (cop.Tipo)
            {
                case "Texto":
                    if (bCFTransparente.IsChecked == true)
                    {
                        (cop as Texto).ColorFondo = Color.FromArgb(0, (cop as Texto).ColorFondo.R, (cop as Texto).ColorFondo.G, (cop as Texto).ColorFondo.B);
                    }
                    else
                    {
                        (cop as Texto).ColorFondo = Color.FromArgb(255, (cop as Texto).ColorFondo.R, (cop as Texto).ColorFondo.G, (cop as Texto).ColorFondo.B);

                    }
                    break;
                default:
                    if (bCFTransparente.IsChecked == true)
                    {
                        (cop as Figura).ColorFondo = Color.FromArgb(0, (cop as Figura).ColorFondo.R, (cop as Figura).ColorFondo.G, (cop as Figura).ColorFondo.B);
                    }
                    else
                    {
                        (cop as Figura).ColorFondo = Color.FromArgb(255, (cop as Figura).ColorFondo.R, (cop as Figura).ColorFondo.G, (cop as Figura).ColorFondo.B);

                    }
                    break;
            }
        }
    }
}

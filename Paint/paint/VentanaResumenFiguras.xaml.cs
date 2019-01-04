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
using System.Windows.Shapes;
using System.Web.Security;
using Paint.Herramientas;

namespace Paint
{
    /// <summary>
    /// Lógica de interacción para VentanaResumenFiguras.xaml
    /// </summary>
    public delegate void opcion(DibujoBase dB);

    public partial class VentanaResumenFiguras : Window
    {

        public event opcion modificarEnCanvas;
        public event opcion eliminarEnCanvas;
        public event opcion añadirEnCanvas;
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public VentanaResumenFiguras()
        {
            InitializeComponent();
            
            
        }
        
        /// <summary>
        /// Evento que registramos para que cuando el usuario haya indicado el elemento que quiere añadir, al pulsar el boton esta se añada al listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAñadir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String tipoElemento = (selectorFiguras.SelectedItem as Label).Content.ToString();
                switch (tipoElemento)
                {
                    case "Recta"://Si el usuario quiere hacer una nueva recta
                        Recta r = new Recta(tipoElemento);
                        resumenFiguras.Items.Add(r);
                        añadirEnCanvas(r);
                        break;
                    case "Texto"://Si el usuario quiere hacer una nueva caja con texto
                        Texto t = new Texto(tipoElemento);
                        resumenFiguras.Items.Add(t);
                        añadirEnCanvas(t);
                        break;
                    default://En otro caso, es una figura
                        Figura f = new Figura(tipoElemento);
                        resumenFiguras.Items.Add(f);
                        añadirEnCanvas(f);
                        break;
                }
                
            }
            catch (NullReferenceException) {
                MessageBox.Show("No has seleccionado un elemento para añadir", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Evento asociado al boton Eliminar, cada vez que se pulsa, se elimina el elemento que se tiene seleccionado en el resumen de figuras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                eliminarEnCanvas(resumenFiguras.SelectedItem as DibujoBase);
                resumenFiguras.Items.Remove(resumenFiguras.SelectedItem);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("No has seleccionado un elemento para eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Evento que se produce cuando el usuario cambia el item que esta seleccionado en el resumen de las figuras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resumenFiguras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resumenFiguras.SelectedItem != null)
            {
                try
                {
                    miniaturaElmento.Content = (resumenFiguras.SelectedItem as DibujoBase).convertirAElementoVisual();

                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("No hay nada seleccionado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


        }

        /// <summary>
        /// Evento que registramos para que cuando el usuario pulse el boton de propiedades, se genere la ventana con las propiedades del elemento seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bPropiedades_Click(object sender, RoutedEventArgs e)
        {  

            /*Preguntar a Chamorro si esto es acoplado o no*/
                VentanaPropiedades vP = new VentanaPropiedades();
                vP.Owner = this;
                vP.elementoSeleccionado += cogerElementoSeleccionado;
                vP.indicarElemento();

                if (vP.IsEnabled)
                {
                    vP.ShowDialog();

                    if (vP.DialogResult == true)
                    {
                        /*Poner aqui lo del delegado y que modifique la figura*/
                        modificarEnCanvas(resumenFiguras.SelectedItem as DibujoBase);
                        miniaturaElmento.Content = (resumenFiguras.SelectedItem as DibujoBase).convertirAElementoVisual();

                        MessageBox.Show("Elemento Actualizado", "Actualizacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                
            
                

        }

        /// <summary>
        /// Metodo que nos devuelve el elemento seleccionado del ListView
        /// </summary>
        /// <returns></returns>
        private DibujoBase cogerElementoSeleccionado()
        {
            try
            {
                return (resumenFiguras.SelectedItem as DibujoBase);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Evento que añade un elemento al listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> contiene el elemento que vamos a añadir</param>
        public void insertarNuevoElemento(object sender, CambioEnElementoEventArgs e)
        {
            resumenFiguras.Items.Add(e.dB);
        }

        /// <summary>
        /// Evento que añade un elemento al listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> contiene el elemento que vamos a añadir</param>
        public void cargarElementosDibujados(object sender, CambioEnElementoEventArgs e)
        {
            resumenFiguras.Items.Add(e.dB);
        }

        /// <summary>
        /// Evento que elimina un elemento del listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> contiene el elemento que vamos a eliminar</param>
        public void eliminarElemento(object sender, CambioEnElementoEventArgs e)
        {
            for (int i=0; i<resumenFiguras.Items.Count;i++)
            {
                if ((resumenFiguras.Items[i] as DibujoBase).ID.Equals(e.dB.ID))
                {
                    resumenFiguras.Items.Remove(resumenFiguras.Items[i]);
                    miniaturaElmento.Content = null;
                }
            }
            
            
        }

        /// <summary>
        /// Evento que elimina un elemento del listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"> contiene el elemento que vamos a eliminar</param>
        public void modificarElemento(object sender, CambioEnElementoEventArgs e)
        {
            for (int i = 0; i < resumenFiguras.Items.Count; i++)
            {
                if ((resumenFiguras.Items[i] as DibujoBase).ID.Equals(e.dB.ID))
                {
                    (resumenFiguras.Items[i] as DibujoBase).PuntoInicio = e.dB.PuntoInicio;
                    (resumenFiguras.Items[i] as DibujoBase).ColorContorno = e.dB.ColorContorno;

                    switch (e.dB.Tipo)
                    {
                        case "Texto":
                            (resumenFiguras.Items[i] as Texto).Contenido = (e.dB as Texto).Contenido;
                            (resumenFiguras.Items[i] as Texto).ColorFondo= (e.dB as Texto).ColorFondo;
                            (resumenFiguras.Items[i] as Texto).Rotacion = (e.dB as Texto).Rotacion;
                            (resumenFiguras.Items[i] as Texto).TipoLetra = (e.dB as Texto).TipoLetra;
                            (resumenFiguras.Items[i] as Texto).Alto = (e.dB as Texto).Alto;
                            (resumenFiguras.Items[i] as Texto).Ancho = (e.dB as Texto).Ancho;
                            (resumenFiguras.Items[i] as Texto).TamañoFuente = (e.dB as Texto).TamañoFuente;
                            (resumenFiguras.Items[i] as Texto).EstiloFuente = (e.dB as Texto).EstiloFuente;
                            
                            break;
                        case "Recta":
                            (resumenFiguras.Items[i] as Recta).GrosorContorno = (e.dB as Recta).GrosorContorno;
                            (resumenFiguras.Items[i] as Recta).PuntoFin = (e.dB as Recta).PuntoFin;
                            break;
                        case "Mano Alzada":
                            (resumenFiguras.Items[i] as ManoAlzada).GrosorContorno = (e.dB as ManoAlzada).GrosorContorno;
                            (resumenFiguras.Items[i] as ManoAlzada).Puntos= (e.dB as ManoAlzada).Puntos;
                            break;
                        default:
                            (resumenFiguras.Items[i] as Figura).ColorFondo = (e.dB as Figura).ColorFondo;
                            (resumenFiguras.Items[i] as Figura).Rotacion = (e.dB as Figura).Rotacion;
                            (resumenFiguras.Items[i] as Figura).Alto = (e.dB as Figura).Alto;
                            (resumenFiguras.Items[i] as Figura).Ancho = (e.dB as Figura).Ancho;
                            (resumenFiguras.Items[i] as Figura).GrosorContorno = (e.dB as Figura).GrosorContorno;
                            break;
                    }
                    miniaturaElmento.Content = (resumenFiguras.Items[i] as DibujoBase).convertirAElementoVisual();
                }
                break;
            }


        }

    }
}

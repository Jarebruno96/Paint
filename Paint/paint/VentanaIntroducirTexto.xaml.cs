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

namespace Paint
{
    /// <summary>
    /// Lógica de interacción para VentanaIntroducirTexto.xaml
    /// </summary>
    public partial class VentanaIntroducirTexto : Window
    {
        //Atributos de la clase
        private String contenido;
        private FontFamily tipoLetra = new FontFamily("Arial");
        private double tamañoFuente = 10;
        private FontStyle estiloFuente = FontStyles.Normal;

        //Metodos de acceso a dichos atributos
        public String Contenido
        {
            get { return contenido; }
        }
        public FontFamily TipoLetra
        {
            get { return tipoLetra; }
        }
        public double TamañoFuente
        {
            get { return tamañoFuente; }
        }
        public FontStyle EstiloFuente
        {
            get { return estiloFuente; }
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public VentanaIntroducirTexto()
        {
            InitializeComponent();
            contenido = "";
        }

        /// <summary>
        /// Evento asociado al boton de aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!cajaDeTexto.Text.Contains(Utils.c))
            {
                contenido = cajaDeTexto.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("El elemento contiene un caracter inválido", "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Evento asociado al boton de opciones avanzadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bPropiedadesAvanzadas_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog fD = new System.Windows.Forms.FontDialog();
            fD.ShowEffects = false;
            fD.ShowHelp = true;
            if (fD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tipoLetra = new FontFamily(fD.Font.FontFamily.Name);
                switch (fD.Font.Style.ToString())
                {
                    case "Normal":
                        estiloFuente = FontStyles.Normal;
                        break;
                    case "Italic":
                        estiloFuente = FontStyles.Italic;
                        break;
                    case "Bold":
                        estiloFuente = FontStyles.Normal;
                        break;
                    default:
                        estiloFuente = FontStyles.Italic;
                        break;
                }
                tamañoFuente = fD.Font.Size;

            }
        }
    }
}

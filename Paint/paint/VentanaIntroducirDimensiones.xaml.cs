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
    /// Lógica de interacción para VentanaIntroducirDimensiones.xaml
    /// </summary>
    public partial class VentanaIntroducirDimensiones : Window
    {
        //Atributos de la clase
        private double altura;
        private double anchura;

        //Propiedades de acceso a los atributos anteriores
        public double Altura
        {
            get{ return altura; }
        }
        public double Anchura
        {
            get { return anchura; }
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public VentanaIntroducirDimensiones()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que registramos para cuando el usuario quiera guardar las nuevas dimensiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAceptar_Click(object sender, RoutedEventArgs e)
        {
            bool validos = true;

            //Altura
            try
            {
                cajaAltura.Background = Brushes.White;
                altura = Double.Parse(cajaAltura.Text);
                if (altura<=0)
                {
                    validos = false;
                    cajaAltura.Background = Brushes.LightPink;
                }
            }
            catch (FormatException)
            {
                validos = false;
                cajaAltura.Background = Brushes.LightPink;
            }


            //Anchura
            try
            {
                cajaAnchura.Background = Brushes.White;
                anchura = Double.Parse(cajaAnchura.Text);
                if (anchura <= 0)
                {
                    validos = false;
                    cajaAnchura.Background = Brushes.LightPink;
                }
            }
            catch (FormatException)
            {
                validos = false;
                cajaAnchura.Background = Brushes.LightPink;
            }

            if (validos) {
                DialogResult = true;
            }

        }
    }
}

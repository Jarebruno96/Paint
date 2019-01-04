using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Web.Security;
using System.Windows.Shapes;

namespace Paint.Elementos
{
    class Figura :DibujoBase
    {
        //Atributos de la clase
        private Color colorFondo;
        private double rotacion;
        private double altura;
        private double anchura;
        private double grosorContorno;

        //Propiedades de acceso de los atributos anteriores
        public double Ancho
        {
            get { return anchura; }
            set
            {
                anchura = value;
                OnPropertyChanged("Ancho");
            }
        }
        public double Alto
        {
            get { return altura; }
            set
            {
                altura = value;
                OnPropertyChanged("Alto");
            }
        }
        public Color ColorFondo
        {
            get { return colorFondo; }
            set
            {
                colorFondo = value;
                OnPropertyChanged("ColorFondo");
            }
        }
        public double Rotacion
        {
            get { return rotacion; }
            set
            {
                rotacion = value;
                OnPropertyChanged("Rotacion");
            }
        }
        public double GrosorContorno
        {
            get { return grosorContorno; }
            set { grosorContorno = value;
                OnPropertyChanged("GradoContorno");
            }
        }


        public Figura(String tipo) {
            this.Tipo = tipo;
            do
            {
                this.ID = Membership.GeneratePassword(8, 0);
            } while (this.ID.Contains(Utils.c[0]));

           
        }

        public Figura()
        {

        }

        public override Object convertirAElementoVisual()
        {
           
            Shape elemento = null;
            switch (Tipo)
            {
                case "Elipse":
                    elemento = new Ellipse();
                    break;
                case "Rectangulo":
                    elemento = new Rectangle();
                    break;
            }

            elemento.Width = Ancho;
            elemento.StrokeThickness = GrosorContorno;
            elemento.Height = Alto;
            elemento.Fill = new SolidColorBrush(ColorFondo);
            elemento.Stroke = new SolidColorBrush(ColorContorno);
            elemento.RenderTransform = new RotateTransform(Rotacion, (Ancho) / 2, (Alto) / 2);
            elemento.Uid = ID;
            return elemento;
        }

        public override DibujoBase duplicar()
        {
            Figura f = new Figura();
            f.ID = ID;
            f.Tipo = Tipo;
            f.ColorContorno = ColorContorno;
            f.PuntoInicio = PuntoInicio;
            f.Alto = Alto;
            f.Ancho = Ancho;
            f.Rotacion = Rotacion;
            f.ColorFondo = ColorFondo;
            f.GrosorContorno = GrosorContorno;
            return f;
        }

        public override void copiar(DibujoBase dB)
        {
            Figura e = (dB as Figura);
            ID = e.ID;
            Tipo = e.Tipo;
            ColorContorno = e.ColorContorno;
            PuntoInicio = e.PuntoInicio;
            Alto = e.Alto;
            Ancho = e.Ancho;
            ColorFondo = e.ColorFondo;
            Rotacion = e.Rotacion;
            GrosorContorno = e.GrosorContorno;
        }

        public override string misDatosAString()
        {
            return ID + Utils.c + Tipo + Utils.c + PuntoInicio.ToString() + Utils.c +ColorContorno.ToString() + Utils.c +ColorFondo.ToString() + Utils.c +Rotacion + Utils.c +Alto + Utils.c +Ancho + Utils.c +GrosorContorno;
        }

        public override bool desglosarInfo(string linea)
        {
            try
            {
                String[] datos = linea.Split(Utils.c[0]);
                String[] temp1;
                String[] temp2 = new String[4];
                ID = datos[0];
                Tipo = datos[1];
                temp1 = datos[2].Split(';');
                PuntoInicio = new Point(Double.Parse(temp1[0]),Double.Parse(temp1[1]));
                temp2[0] = datos[3].Substring(1, 2);
                temp2[1] = datos[3].Substring(3, 2);
                temp2[2] = datos[3].Substring(5, 2);
                temp2[3] = datos[3].Substring(7, 2);
                ColorContorno =  Color.FromArgb((byte)Convert.ToInt32(temp2[0],16), (byte)Convert.ToInt32(temp2[1],16), (byte)Convert.ToInt32(temp2[2],16), (byte)Convert.ToInt32(temp2[3],16));
                temp2[0] = datos[4].Substring(1, 2);
                temp2[1] = datos[4].Substring(3, 2);
                temp2[2] = datos[4].Substring(5, 2);
                temp2[3] = datos[4].Substring(7, 2);
                ColorFondo = Color.FromArgb((byte)Convert.ToInt32(temp2[0], 16), (byte)Convert.ToInt32(temp2[1], 16), (byte)Convert.ToInt32(temp2[2], 16), (byte)Convert.ToInt32(temp2[3], 16));
                Rotacion = Double.Parse(datos[5]);
                Alto = Double.Parse(datos[6]);
                Ancho = Double.Parse(datos[7]);
                GrosorContorno = Double.Parse(datos[8]);


            }
            catch (FormatException)
            {
                return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }
    }
}

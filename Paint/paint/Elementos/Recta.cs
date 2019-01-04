using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Web.Security;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Paint.Elementos
{
    class Recta :DibujoBase
    {
        //Atributos de la clase
        private double grosorContorno;
        private Point puntoFin;
        //Propiedades de acceso a los atributos
        public double GrosorContorno
        {
            get { return grosorContorno; }
            set { grosorContorno = value; }
        }
        public Point PuntoFin
        {
            get { return puntoFin; }
            set
            {
                puntoFin = value;
                OnPropertyChanged("PuntoFin");
            }
        }


        public Recta(String tipo)
        {
            this.Tipo = tipo;
            do
            {
                this.ID = Membership.GeneratePassword(8, 0);
            } while (this.ID.Contains(Utils.c[0]));
        }
        public Recta()
        {

        }

        public override Object convertirAElementoVisual()
        {
            Line l = new Line();
            l.X1 = PuntoInicio.X;
            l.Y1 = PuntoInicio.Y;
            l.X2 = PuntoFin.X;
            l.Y2 = PuntoFin.Y;
            l.Stroke = new SolidColorBrush(ColorContorno);
            l.StrokeThickness = GrosorContorno;
            l.Uid = ID;
            return l;
        }

        public override DibujoBase duplicar()
        {
            Recta r = new Recta();
            r.ID = ID;
            r.Tipo = Tipo;
            r.ColorContorno = ColorContorno;
            r.PuntoInicio = PuntoInicio;
            r.PuntoFin = PuntoFin;
            r.GrosorContorno = GrosorContorno;
            return r;
            
        }

        public override void copiar(DibujoBase dB)
        {
            Recta e = (dB as Recta);
            ID = e.ID;
            Tipo = e.Tipo;
            ColorContorno = e.ColorContorno;
            PuntoInicio = e.PuntoInicio;
            PuntoFin = e.PuntoFin;
            GrosorContorno = e.GrosorContorno;
        }

        public override string misDatosAString()
        {
            return ID + Utils.c + Tipo + Utils.c + PuntoInicio.ToString() + Utils.c + ColorContorno.ToString() + Utils.c + PuntoFin.ToString() + Utils.c + GrosorContorno;
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
                PuntoInicio = new Point(Double.Parse(temp1[0]), Double.Parse(temp1[1]));
                temp2[0] = datos[3].Substring(1, 2);
                temp2[1] = datos[3].Substring(3, 2);
                temp2[2] = datos[3].Substring(5, 2);
                temp2[3] = datos[3].Substring(7, 2);
                ColorContorno = Color.FromArgb((byte)Convert.ToInt32(temp2[0], 16), (byte)Convert.ToInt32(temp2[1], 16), (byte)Convert.ToInt32(temp2[2], 16), (byte)Convert.ToInt32(temp2[3], 16));
                temp1 = datos[4].Split(';');
                PuntoFin = new Point(Double.Parse(temp1[0]), Double.Parse(temp1[1]));
                GrosorContorno = Double.Parse(datos[5]);

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

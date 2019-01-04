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
    class ManoAlzada :DibujoBase
    {
        //Atributos de la clase
        private PointCollection puntos = new PointCollection();
        private double grosorContorno;

        //Propiedades de acceso a los atributos
        public double GrosorContorno
        {
            get { return grosorContorno; }
            set { grosorContorno = value; }
        }
        public PointCollection Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }

        public ManoAlzada(String tipo)
        {
            this.Tipo = tipo;
            do
            {
                this.ID = Membership.GeneratePassword(8, 0);
            } while (this.ID.Contains(Utils.c[0]));
        }
       
        public ManoAlzada()
        {

        }

        public override object convertirAElementoVisual()
        {
            Polyline pL = new Polyline();
            pL.Points = puntos;
            pL.Uid = ID;
            pL.StrokeThickness = GrosorContorno;
            pL.Stroke = new SolidColorBrush(ColorContorno);
            return pL;
        }

        public override DibujoBase duplicar()
        {
            ManoAlzada mA = new ManoAlzada();
            mA.ID = ID;
            mA.Tipo = Tipo;
            mA.PuntoInicio = PuntoInicio;
            mA.ColorContorno = ColorContorno;
            mA.Puntos = Puntos;
            mA.GrosorContorno = GrosorContorno;
            return mA;
        }

        public override void copiar(DibujoBase x)
        {
            ManoAlzada mA = (x as ManoAlzada);
            ID = mA.ID;
            Tipo = mA.Tipo;
            ColorContorno = mA.ColorContorno;
            Puntos = mA.Puntos;
            PuntoInicio = mA.PuntoInicio;
            GrosorContorno = mA.GrosorContorno;
        }

        public override string misDatosAString()
        {
            return ID + Utils.c + Tipo + Utils.c + PuntoInicio.ToString() + Utils.c + ColorContorno.ToString() + Utils.c + Puntos.ToString() + Utils.c + GrosorContorno;
        }

        public override bool desglosarInfo(string linea)
        {
            try
            {
                String[] datos = linea.Split(Utils.c[0]);
                String[] temp1;
                String[] temp2 = new String[4];
                String[] temp3;
                ID = datos[0];
                Tipo = datos[1];
                temp1 = datos[2].Split(';');
                PuntoInicio = new Point(Double.Parse(temp1[0]), Double.Parse(temp1[1]));
                temp2[0] = datos[3].Substring(1, 2);
                temp2[1] = datos[3].Substring(3, 2);
                temp2[2] = datos[3].Substring(5, 2);
                temp2[3] = datos[3].Substring(7, 2);
                ColorContorno = Color.FromArgb((byte)Convert.ToInt32(temp2[0], 16), (byte)Convert.ToInt32(temp2[1], 16), (byte)Convert.ToInt32(temp2[2], 16), (byte)Convert.ToInt32(temp2[3], 16));
                temp3 = datos[4].Split(' ');
                for (int i=0; i<temp3.Count();i++)
                {
                    temp1 = temp3[i].Split(';');
                    Puntos.Add(new Point(Double.Parse(temp1[0]),Double.Parse(temp1[1])));
                }
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

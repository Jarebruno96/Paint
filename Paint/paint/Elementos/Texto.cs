using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Web.Security;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace Paint.Elementos
{
    class Texto :DibujoBase
    {
        //Atributos de la clase
        private String contenido;
        private Color colorFondo;
        private double rotacion;
        private FontFamily tipoLetra = new FontFamily("Arial");
        private double altura;
        private double anchura;
        private double tamañoFuente = 10;
        private FontStyle estiloFuente = FontStyles.Normal;

        //Propiedades de acceso a los atributos anteriores
        public FontStyle EstiloFuente
        {
            get { return estiloFuente; }
            set { estiloFuente = value; }
        }
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
        public String Contenido
        {
            get { return contenido; }
            set
            {
                contenido = value;
                OnPropertyChanged("Contenido");
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
        public FontFamily TipoLetra
        {
            get { return tipoLetra; }
            set
            {
                tipoLetra = value;
                OnPropertyChanged("TipoLetra");
            }
        }
        public double TamañoFuente
        {
            get { return tamañoFuente; }
            set { tamañoFuente = value; }
        }


        public Texto(String tipo)
        {
            this.Tipo = tipo;
            do
            {
                this.ID = Membership.GeneratePassword(8, 0);
            } while (this.ID.Contains(Utils.c[0]));
        }

        public Texto() { 
        
        }

        public override Object convertirAElementoVisual()
        {
            
            TextBlock t = new TextBlock();
            t.Width = Ancho;
            t.Height = Alto;
            t.Background = new SolidColorBrush(ColorFondo);
            t.Foreground = new SolidColorBrush(ColorContorno);
            t.RenderTransform = new RotateTransform(Rotacion, (t.Width) / 2,(t.Height) / 2);
            t.Text = Contenido;
            t.Uid = ID;
            t.FontFamily = TipoLetra;
            t.FontSize = TamañoFuente;
            t.FontStyle = estiloFuente;

            return t;
            
        }

        public override DibujoBase duplicar()
        {
            Texto t = new Texto();
            t.ID = ID;
            t.Tipo = Tipo;
            t.ColorContorno = ColorContorno;
            t.PuntoInicio = PuntoInicio;
            t.Alto = Alto;
            t.Ancho = Ancho;
            t.Contenido = Contenido;
            t.ColorFondo = ColorFondo;
            t.Rotacion = Rotacion;
            t.TipoLetra = TipoLetra;
            t.TamañoFuente = TamañoFuente;
            t.EstiloFuente = estiloFuente;
            return t;
        }

        public override void copiar(DibujoBase dB)
        {
            Texto e = (dB as Texto);
            ID = e.ID;
            Tipo = e.Tipo;
            ColorContorno = e.ColorContorno;
            PuntoInicio = e.PuntoInicio;
            Alto = e.Alto;
            Ancho = e.Ancho;
            ColorFondo = e.ColorFondo;
            TamañoFuente = e.TamañoFuente;
            TipoLetra = e.TipoLetra;
            Rotacion = e.Rotacion;
            Contenido = e.Contenido;
            EstiloFuente = e.EstiloFuente;
        }

        public override string misDatosAString()
        {
            return ID + Utils.c + Tipo + Utils.c + PuntoInicio.ToString() + Utils.c + ColorContorno.ToString() + Utils.c + Contenido + Utils.c + colorFondo.ToString() + Utils.c + Rotacion + Utils.c + tipoLetra.ToString() + Utils.c + Alto + Utils.c + Ancho + Utils.c + TamañoFuente + Utils.c + EstiloFuente.ToString();
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
                Contenido = datos[4];
                temp2[0] = datos[5].Substring(1, 2);
                temp2[1] = datos[5].Substring(3, 2);
                temp2[2] = datos[5].Substring(5, 2);
                temp2[3] = datos[5].Substring(7, 2);
                ColorFondo = Color.FromArgb((byte)Convert.ToInt32(temp2[0], 16), (byte)Convert.ToInt32(temp2[1], 16), (byte)Convert.ToInt32(temp2[2], 16), (byte)Convert.ToInt32(temp2[3], 16));
                Rotacion = Double.Parse(datos[6]);
                TipoLetra = new FontFamily(datos[7]);
                Alto = Double.Parse(datos[8]);
                Ancho = Double.Parse(datos[9]);
                TamañoFuente = Double.Parse(datos[10]);
                switch (datos[11])
                {
                    case "Normal":
                        EstiloFuente = FontStyles.Normal;
                        break;
                    case "Italic":
                        EstiloFuente = FontStyles.Italic;
                        break;
                }
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

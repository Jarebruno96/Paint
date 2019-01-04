using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Paint.Elementos
{
    public abstract class DibujoBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /*Atributos comunes de todos los elementos que pueden existir en el Dibujo*/
        private String id;        
        private String tipo;
        private Point puntoInicio;
        private Color colorContorno = Color.FromArgb(255,0,0,0);
        /*Propiedades con las que accederemos a dicha informacion*/
        public String ID
        {
            get { return id; }
            set { id = value;
                  OnPropertyChanged("ID");    
            }
        }
        public String Tipo {
            get { return tipo; }
            set { tipo = value;
                  OnPropertyChanged("Tipo");
            }
        }
        public Point PuntoInicio {
            get { return puntoInicio; }
            set { puntoInicio = value;
                  OnPropertyChanged("PuntoInicio");
            }
        }
        public Color ColorContorno {
            get { return colorContorno; }
            set { colorContorno = value;
                  OnPropertyChanged("ColorContorno");
            }
        }



        /// <summary>
        /// Implematamos este metodo para que cuando se modifique un atributo del elemento, este cambio se refleje en el listView
        /// </summary>
        /// <param name="pName"></param>
        protected void OnPropertyChanged(String pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        }

        /// <summary>
        /// Metodo que convierte un objeto a un elemento visual para añadirlo al canvas 
        /// </summary>
        /// <returns></returns>
        public abstract Object convertirAElementoVisual();

        /// <summary>
        /// Devuelve una copia del elemento
        /// </summary>
        /// <returns></returns>
        public abstract DibujoBase duplicar();

        /// <summary>
        /// Copia todos los datos del elemento que se le pasa
        /// </summary>
        /// <param name="x"></param>
        public abstract void copiar(DibujoBase x);

        /// <summary>
        /// Devuelve un cadena con todos los datos del elemento
        /// </summary>
        /// <returns></returns>
        public abstract String misDatosAString();

        /// <summary>
        /// Coge los valores de la cadena que se le pasan y se lo asigna a los atributos correspondientes
        /// </summary>
        /// <param name="linea"></param>
        /// <returns></returns>
        public abstract bool desglosarInfo(String linea);

    }
}

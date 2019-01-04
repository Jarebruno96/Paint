using Paint.Elementos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Herramientas
{
    public delegate void opcionesElementoEventHandler(object sender, CambioEnElementoEventArgs e);
    public class CambioEnElementoEventArgs : EventArgs
    {
        public DibujoBase dB;
    }
}

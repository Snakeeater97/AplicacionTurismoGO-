using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTurismoGO_
{
    internal class Validaciones
    {
        public static bool ValidarCantidadAdultos(int cantidadAdultos)
        {
            return cantidadAdultos >= 1;
        }
    }
}

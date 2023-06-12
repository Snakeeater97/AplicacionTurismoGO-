using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTurismoGO_
{
    internal class Vuelos
    {
        public class Vuelo
        {
            public string CodVuelo { get; set; }
            public string Origen { get; set; }
            public string NombreOrigen { get; set; }
            public string Destino { get; set; }
            public string NombreDestino { get; set; }
            public string Aerolinea { get; set; }
            public string TiempoVuelo { get; set; }
            public string Tarifa { get; set; }
            public DateTime FechaHoraSalida { get; set; }
            public DateTime FechaHoraArribo { get; set; }
        }
    }
}

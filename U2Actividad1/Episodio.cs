using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U2Actividad1
{
    [Serializable]
    public class Episodio
    {
        public string TituloEspañol { get; set; }
        public string NumeroEpisodio { get; set; }
        public string NumeroTemporada { get; set; }
        public string TituloOriginal { get; set; }
        public string Descripcion { get; set; }
    }
}

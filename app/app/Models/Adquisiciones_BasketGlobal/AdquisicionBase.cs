using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models.Adquisiciones_BasketGlobal
{
    public class Adquisicion
    {
        public string _id { get; set; }
        public string id_usu { get; set; }
        public string id_prod { get; set; }
        public string fecha_adquisicion { get; set; }
        public int cantidad { get; set; }
        public string estado_adquisicion { get; set; }
    }
}

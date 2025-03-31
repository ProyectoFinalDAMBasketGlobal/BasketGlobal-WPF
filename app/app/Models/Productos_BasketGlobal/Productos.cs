using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models.Productos_BasketGlobal
{
    public class Producto
    {
        public string _id { get; set; }
        public string nombre { get; set; }
        public string categoria { get; set; }
        public string marca { get; set; }
        public bool esImportado { get; set; }
        public string origen { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public double? precio_original { get; set; }
        public bool tieneOferta { get; set; }
        public int stock { get; set; }
        public bool estado { get; set; }
        public string imagenBase64 { get; set; }

    }
}

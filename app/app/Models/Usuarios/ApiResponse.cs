using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace app.Models.Usuarios
{
    public class ApiResponse<T>
    {
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("data")]
        public T data { get; set; }
    }
}

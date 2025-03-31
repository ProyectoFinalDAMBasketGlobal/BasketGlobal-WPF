using Newtonsoft.Json;

namespace app.Models.Adquisiciones_BasketGlobal
{
    public class ApiResponse<T>
    {
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
        [JsonProperty("adquisiciones")]
        public T adquisiciones { get; set; }
        [JsonProperty("productos")]
        public T productos { get; set; }
    }
}

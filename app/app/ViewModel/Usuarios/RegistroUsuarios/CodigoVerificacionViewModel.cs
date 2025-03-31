using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace app.ViewModel.Usuarios.RegistroUsuarios
{
    public class CodigoVerificacionViewModel
    {
        public const string ApiUrlEliminarUsuario = "http://127.0.0.1:3505/Usuario/eliminarUsuario";

        public async Task<HttpResponseMessage> EliminarUsuario(string emailEliminar) 
        {

            using (var client = new HttpClient()) 
            {
                try {
                    var json = JsonConvert.SerializeObject(new {emailApp = emailEliminar });
                    var content = new StringContent(json,Encoding.UTF8,"application/json");

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(ApiUrlEliminarUsuario),
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"WPF : Se ha eliminado usuario : {response.Content}");
                        return response;
                    }
                    else {
                        Debug.WriteLine($"WPF : Error al eliminar : {emailEliminar}");
                        return response;
                    }
                }
                catch (Exception ex) 
                {
                    throw new Exception("WPF : ERROR "+ex.Message);
                } 
            }
        }

    }
}

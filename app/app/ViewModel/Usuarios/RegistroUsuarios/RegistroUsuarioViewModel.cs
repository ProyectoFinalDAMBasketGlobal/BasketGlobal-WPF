using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using app.Models.Usuarios;
using Newtonsoft.Json;

namespace app.ViewModel.Usuarios.RegistroUsuarios
{
    public class RegistroUsuarioViewModel
    {
        private const string ApiUrlRegistroUsuario = "http://127.0.0.1:3505/Usuario/registroUsuario";
        private const string ApiUrlVerificarUsuario = "http://127.0.0.1:3505/Usuario/verificarUsuario";

        public async Task<HttpResponseMessage> RegistrarUsuario(Usuario UsuarioNuevo)
        {
            using (var cliente = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(UsuarioNuevo);
                Debug.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await cliente.PostAsync(ApiUrlRegistroUsuario, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Usuario creado correctamente : En ViewModel");
                        return response;
                    }
                    else
                    {
                        Debug.WriteLine($"Error : {response.StatusCode} : En ViewModel");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex.Message} : En ViewModel");
                    return null;
                }
            }
        }

        public async Task<HttpResponseMessage> ValidarUsuario(Usuario VerificarUsuario)
        {
            using (var cliente = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(VerificarUsuario);
                Debug.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await cliente.PostAsync(ApiUrlVerificarUsuario, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("WPF : Usuario verificado correctamente");
                        return response;
                    }
                    else
                    {
                        Debug.WriteLine($"WPF : Error : {response.StatusCode}");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
        }

    }
}

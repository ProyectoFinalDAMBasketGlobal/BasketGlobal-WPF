using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using app.Models.Usuarios;
using IntermodularWPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace app.ViewModel.Usuarios.RegistroUsuarios
{
    public class RegistroPerfilViewModel
    {
        private const string ApiUrlRegistroPerfilAdministrador = "http://127.0.0.1:3505/Administrador/registrarAdministrador";
        private const string ApiUrlRegistroPerfilEmpleado = "http://127.0.0.1:3505/Empleado/registrarEmpleado";
        private const string ApiUrlRegistroPerfilCliente= "http://127.0.0.1:3505/Cliente/registrarCliente";

        public async Task<HttpResponseMessage> RegistrarPerfil(MultipartFormDataContent PerfilNuevo, string rol)
        {
            string registroRuta = "";

            if (rol == "Administrador")
            {
                registroRuta = ApiUrlRegistroPerfilAdministrador;
            }
            else if (rol == "Empleado")
            {
                registroRuta = ApiUrlRegistroPerfilEmpleado;
            }
            else if (rol == "Cliente")
            {
                registroRuta = ApiUrlRegistroPerfilCliente;
            }

            using (var cliente = new HttpClient())
            {
     

                try
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SettingsData.Default.token);
                    cliente.DefaultRequestHeaders.Add("x-user-role", SettingsData.Default.rol); // Enviar el rol en el encabezado

                    HttpResponseMessage response = await cliente.PostAsync(registroRuta, PerfilNuevo);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Usuario creado correctamente");
                        return response;
                    }
                    else
                    {
                        Debug.WriteLine($"Error: {response.StatusCode}");
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

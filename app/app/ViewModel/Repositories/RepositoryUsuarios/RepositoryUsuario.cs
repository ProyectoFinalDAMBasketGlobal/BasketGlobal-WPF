using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using app.Models.ApiRouteUsuario;
using app.Models.IUsuariosRepository;
using app.Models.Usuarios;
using app.View.Usuarios.Notificaciones;
using Newtonsoft.Json;

namespace app.ViewModel.Repositories.RepositoryUsuarios
{
    public class RepositoryUsuario : RepositoryBase, IUsuarioRepository
    {

        //Repository de CommandLogin
        public async Task<dynamic> AuthenticateUser(NetworkCredential credential)
        {
            
             
            var json = JsonConvert.SerializeObject(new { email = credential.UserName, password = credential.Password, appType = "wpf" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try {

                var response = await API.PostAsync(ApiRouteUsuarios.Usuario.LogIn, content);

                if (response == null)
                {
                    dynamic nullContet = new { ReasonPhrase = "Error de conexión.", Content = "Por favor revise su conexión al servidor." };
                    ShowNotification(nullContet);
                    return null;    
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
    
                if (response.IsSuccessStatusCode)
                {
                    dynamic responseData = JsonConvert.DeserializeObject<dynamic>(jsonContent); 
                    //Retornar un PerfilBase o Usuario con sus datos, por lo que para no complicarlo mas regreso un dinamyc ya que puede ser un inicio de sesion o u registro tardio 
                    
                    return responseData;
                }
                else
                {
                    //La respuesta puede ser json o html
                    string contentType = response.Content.Headers.ContentType?.MediaType ?? "unknown";
                    if (contentType == "application/json")
                    { //Si es JSON
                        var errorContet = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                        ShowNotification(errorContet);
                        return null;
                    }
                    else
                    { //SI es HTML
                        string contenidoExtraido = ExtractPreContent(jsonContent);
                        dynamic errorHtml = new { ReasonPhrase = response.ReasonPhrase, Content = contenidoExtraido };
                        ShowNotification(errorHtml);
                        return null;
                    }
                }
            } catch (Exception ex) 
            {
                //Caso(s): Manejar otros errores
                Debug.WriteLine("Error desconocido: " + ex);
                dynamic exceptionContet = new { ReasonPhrase = "Exception LogIn.", Content = ex.Message.ToString() };
                ShowNotification(exceptionContet);
                return null;
            }

            
        }

        //Repository de CommandDelete
        public async Task<bool> Delete(string id, string role)
        {
            string rutaEliminar = "";
            if (role == "Administrador")
                rutaEliminar = ApiRouteUsuarios.Administrador.Eliminar;
            if (role == "Empleado")
                rutaEliminar = ApiRouteUsuarios.Empleado.Eliminar;
            if (role == "Cliente")
                rutaEliminar = ApiRouteUsuarios.Cliente.Eliminar;

  
            try
            {
                Debug.WriteLine("Entro en eliminar");
                Debug.WriteLine(SettingsData.Default.rol);
                Debug.WriteLine("Entro en eliminar");
                API.DefaultRequestHeaders.Clear();
                API.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SettingsData.Default.token);
                API.DefaultRequestHeaders.Add("x-user-role", SettingsData.Default.rol);

                var json = JsonConvert.SerializeObject(new { _id = id, rol = role });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(rutaEliminar),
                    Content = content
                };

                var response = await API.SendAsync(request);


                if (response == null)
                {
                    Debug.WriteLine("Error respuesta nula");
                    dynamic nullContet = new { ReasonPhrase = "Error de conexión.", Content = "Por favor revise su conexión al servidor." };
                    ShowNotification(nullContet);
                    return false;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Respuesta satisfactoria");
                    dynamic responseData = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                    ShowNotification(responseData); 
                    return true;
                }
                else
                {
                    //La respuesta puede ser json o html
                    string contentType = response.Content.Headers.ContentType?.MediaType ?? "unknown";
                    if (contentType == "application/json")
                    { //Si es JSON
                        Debug.WriteLine("No success / json");
                        Debug.WriteLine($"{jsonContent}");
                        var errorContet = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                        ShowNotification(errorContet);
                        return false;
                    }
                    else
                    { //SI es HTML
                        Debug.WriteLine("No success / html");
                        string contenidoExtraido = ExtractPreContent(jsonContent);
                        dynamic errorHtml = new { ReasonPhrase = response.ReasonPhrase, Content = contenidoExtraido };
                        ShowNotification(errorHtml);
                        return false;
                    }
                }

            }
            catch (Exception e) {
                Debug.WriteLine("WTF");
                Debug.WriteLine("Error desconocido: " + e);
                dynamic exceptionContet = new { ReasonPhrase = "Exception Delete.", Content = e.Message.ToString() };
                ShowNotification(exceptionContet);
                return false; 
            }
        }
        
        public bool Access(string token)
        {
            throw new NotImplementedException();
        }
        public void Add(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }
        public void Update(string id, string rol, MultipartFormDataContent usuarioEditar)
        {
            throw new NotImplementedException();
        }
        //Notificacion base
        private void ShowNotification(dynamic content)
        {
            //Decidi recibir los datos asi dado asi es como un respose regresa algunas respuestas automaticas,
            //de manera que todos mis end points son iguales para swe homogeneo
            Notificacion not = new Notificacion(content.ReasonPhrase.ToString(), content.Content.ToString());
            //Se busca la venta actual para poder bloquear la ventana que lo ejecuta.
            not.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            not.ShowDialog();
        }

        //En caso nesecario se borran los datos almacenos.
        private void ClearSettings()
        {
            SettingsData.Default.token = "";
            SettingsData.Default.appToken = "";
            SettingsData.Default.idPerfil = "";
            SettingsData.Default.Save();
        }

        //Funcion que gestiona una solicitud HTML del servidor devolviendo el pre
        string ExtractPreContent(string html)
        {
            //Se usa la clase Regex.Match para encontrar la primera ocurrencia de un texto que esté dentro de las etiquetas <pre>...</pre>.
            var match = Regex.Match(html, @"<pre>(.*?)<\/pre>", RegexOptions.Singleline);
            //Si se encontró, match.Groups[1].Value devuelve el contenido capturado entre <pre> y </pre>.
            return match.Success ? match.Groups[1].Value : "No se encontró contenido en <pre>";
        }
    }
}

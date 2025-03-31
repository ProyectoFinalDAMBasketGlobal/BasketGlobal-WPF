using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using app.Models.Usuarios;
using app.Models.Usuarios.Perfiles;
using app.View.Usuarios.Notificaciones;
using IntermodularWPF;
using Newtonsoft.Json;
using app.Models.ApiRouteUsuario;
using System.Windows.Input;
using System.Security;
using app.Models.IUsuariosRepository;
using app.ViewModel.Repositories.RepositoryUsuarios;
using System.Threading;
using System.Security.Principal;
using app.View.Home;
using app.View.Usuarios.RegistroUsuarios;
using app.View.Usuarios.InicioDeSesion;

namespace app.ViewModel.Usuarios
{
    public class UsuarioViewModel : ViewModelBase
    {

        //Autenticación
        private const string ApiUrlLogIn = "http://127.0.0.1:3505/Usuario/logIn";


        //Variables-Binding LogIn para el inicio de sesion y propiedades de tipo comando que validaran el login
        private string _logInMail = "";
        private SecureString _password;
        private string _logInError = "";
        private bool _isViewVisible = true;
        private IUsuarioRepository  _usuarioRepository;
                
                //Estos comando se inician en el constructor del UsuarioViewModel                

        public ICommand LogInCommand { get; } //No se implenta set dado que solo la clase command deberia inicilizarla
        public ICommand DeleteCommand { get; }
        public string LogInMail { get => _logInMail; set { _logInMail = value; OnPropertyChanged("LogInMail"); } }
        public SecureString Password { get => _password; set { _password = value; OnPropertyChanged("Password"); } }
        public string LogInError { get => _logInError; set { _logInError = value; OnPropertyChanged("LogInError"); } }
        public bool IsViewVisible { get => _isViewVisible; set { _isViewVisible = value; OnPropertyChanged("IsViewVisible"); } }



        //Datos Pre-Registros ValidationRule
        private string _emailPreregistro = "";
        private string _emailPreregistroConfirmacion = "";
        private string _emailPreregistroConfirmacion2 = "";
        private string _rolSeleccionado = "";
        //Datos CambioPassword ValidationRule
        private string _passwordd = "";
        private string _passwordConfirm = "";
        private string _passwordConfirm2 = "";
        public string EmailPreRegistro { get => _emailPreregistro; set { _emailPreregistro = value; OnPropertyChanged("EmailPreRegistro"); } }
        public string EmailPreregistroConfirmacion { get => _emailPreregistroConfirmacion; set { _emailPreregistroConfirmacion = value; OnPropertyChanged("EmailPreregistroConfirmacion"); } }
        public string EmailPreregistroConfirmacion2 { get => _emailPreregistroConfirmacion2; set { _emailPreregistroConfirmacion2 = value; OnPropertyChanged("EmailPreregistroConfirmacion2"); } }
        public string RolSeleccionado { get => _rolSeleccionado; set  { _rolSeleccionado = value; OnPropertyChanged("RolSeleccionado"); } }
        public string Passwordd { get => _passwordd;  set { _passwordd = value; OnPropertyChanged("Passwordd"); } }
        public string PasswordConfirm { get => _passwordConfirm;  set { _passwordConfirm = value; OnPropertyChanged("PasswordConfirm"); } }
        public string PasswordConfirm2 { get => _passwordConfirm2;  set { _passwordConfirm2 = value; OnPropertyChanged("PasswordConfirm2"); } }



        private ObservableCollection<UsuarioBase> allPerfiles;

        private ObservableCollection<Usuario> allUsers;
        public ObservableCollection<UsuarioBase> AllPerfiles { get => allPerfiles; set { allPerfiles = value; OnPropertyChanged("AllPerfiles"); } }
        public ObservableCollection<Usuario> AllUsers { get => allUsers; set { allUsers = value; OnPropertyChanged("AllUsers"); } }

        private UsuarioBase usuarioSeleccionado;

        public UsuarioBase UsuarioSeleccionado { get => usuarioSeleccionado; set { usuarioSeleccionado = value; OnPropertyChanged("UsuarioSeleccionado"); CommandManager.InvalidateRequerySuggested(); } } 
        //CommandManager.InvalidateRequerySuggested(); Notifica que CanExecute ha cambiado
      
        private static UsuarioViewModel _instance;
        public static UsuarioViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UsuarioViewModel();
                }
                return _instance;
            }
        }

        private UsuarioViewModel()
        {
            _usuarioRepository = new RepositoryUsuario();

            //Se inicializa los comando mediante la clase generica que cree en el directorio raiz de viewModel
            LogInCommand = new ViewModelCommand(ExecuteLogInCommand, CanExecuteLogInCommand);

            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);

            // Constructor privado para evitar instancias externas
            allPerfiles = new ObservableCollection<UsuarioBase>();
            allUsers = new ObservableCollection<Usuario>();

            _ = CargarTodosLosUsuarios();
        }


        //Commands LogIn
        private bool CanExecuteLogInCommand(object obj)
        {
            bool validData;
            string patternEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            string patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$";

 
            if (string.IsNullOrEmpty(LogInMail) || LogInMail.Length < 3 || !Regex.IsMatch(LogInMail, patternEmail) ||
                Password == null || Password.Length < 3  ||!Regex.IsMatch(new NetworkCredential(string.Empty,Password).Password, patternPassword)) 
                return false;
            else
                validData = true;
            return validData;    
        }

        private async void ExecuteLogInCommand(object obj)
        {

           dynamic isValidUsuario = await _usuarioRepository.AuthenticateUser(new NetworkCredential(LogInMail, Password));

            if (isValidUsuario != null)
            {

                if (isValidUsuario.data.privileges == "Administrador" || isValidUsuario.data.privileges == "Empleado")
                {
                    MessageBox.Show("Registro tardio.");
                    CodigoDeVerificacion code = new CodigoDeVerificacion
                    {
                        Email = isValidUsuario.data.email,
                        EmailApp = isValidUsuario.data.emailApp,
                        ID = isValidUsuario.data.id,
                        Privileges = isValidUsuario.data.privileges
                    };
                    code.Owner  = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                    bool? resultCode = code.ShowDialog();

                    MessageBox.Show("Result Code: " + resultCode.Value.ToString());

                    if (resultCode == true)
                    {
                        MessageBox.Show("Todo correcto tiene que cerrar login");
                        Inicio h = new Inicio();

                        // Verifica si LogIn ya existe y está oculto
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is LogIn logInView)
                            {
                                logInView.Close();
                                h.Show();
                                return;
                            }
                        }
                    }
                    else if (resultCode == false)
                    {
                        MessageBox.Show("Se cancelo la verificación");
                    }
                }
                else if(isValidUsuario.data.user != null)
                {
                    MessageBox.Show("Login progress.");
                    SettingsData.Default.token = isValidUsuario.data.token;
                    SettingsData.Default.appToken = isValidUsuario.data.appToken;
                    SettingsData.Default.idPerfil = isValidUsuario.data.user._id;
                    SettingsData.Default.rol = isValidUsuario.data.user.rol;
                    SettingsData.Default.nombre = isValidUsuario.data.user.nombre;
                    SettingsData.Default.Save();
                    //Esta propiedad permite establecer la identidad del usuario que ejecuta el subproceso actual 
                    //Thread.CurrentPrincipal = new GenericPrincipal(
                    //    new GenericIdentity(isValidUsuario.data.user._id.ToString()),
                    //     new string[] { isValidUsuario.data.user.rol.ToString(), isValidUsuario.data.user.rol.ToString() });
                    //IsViewVisible = false;

                    Inicio user = new Inicio();
                    //user.Show();

                    // 🔹 Cerrar la ventana actual
                    var ventanaActual = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                    if (ventanaActual != null)
                    {
                        ventanaActual.Close();
                        user.Show();
                    }

                }
            }

        }

        //Commands Delete
        private bool CanExecuteDeleteCommand(object obj)
        {
            Console.WriteLine("Comando Verficacion.");
            return obj is UsuarioBase usuario && usuario == UsuarioSeleccionado;
        }

        private async void ExecuteDeleteCommand(object obj)
        {
            string nombreUsuario = SettingsData.Default.nombre;

            Console.WriteLine("Comando ejecutado correctamente.");

            MessageBox.Show("Id: " + UsuarioSeleccionado._id+ " y Rol: "+ UsuarioSeleccionado.rol.ToString());

            var result = MessageBox.Show(nombreUsuario + ", ¿Estás seguro de que quieres eliminar la cuenta seleccionada?",
                              "Confirmar eliminación",
                              MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var response = await _usuarioRepository.Delete(UsuarioSeleccionado._id.ToString(),UsuarioSeleccionado.rol.ToString());
            

                if (response)
                {
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Aquí podrías actualizar la lista de usuarios eliminando el usuario borrado
                    //allPerfiles.Remove(UsuarioSeleccionado);
                    var stringResponse = CargarTodosLosUsuarios();
                    UsuarioSeleccionado = null; // Deseleccionar usuario después de eliminar
                    MessageBox.Show($"{stringResponse}");   
                }
                else
                {
                    //MessageBox.Show("Hubo un error al eliminar el usuario. Intenta de nuevo.");
                    //UsuarioSeleccionado = null; // Deseleccionar usuario después de eliminar
                }
            }
        }



        //OK EX
        public async Task<string> CargarTodosLosUsuarios()
        {
            using (var client = new HttpClient()) {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SettingsData.Default.token);
                    client.DefaultRequestHeaders.Add("x-user-role", SettingsData.Default.rol); // Enviar el rol en el encabezado

                    HttpResponseMessage responseAdministradores = await client.GetAsync(ApiRouteUsuarios.Administrador.GetAll);
                    HttpResponseMessage responseEmpleados = await client.GetAsync(ApiRouteUsuarios.Empleado.GetAll);
                    HttpResponseMessage responseCliente = await client.GetAsync(ApiRouteUsuarios.Cliente.GetAll);

                    HttpResponseMessage responseUsuarios = await client.GetAsync(ApiRouteUsuarios.Usuario.TodosLosUsuarios);

                    if (responseUsuarios.StatusCode == HttpStatusCode.NotFound || 
                        responseAdministradores.StatusCode == HttpStatusCode.NotFound
                        || responseEmpleados.StatusCode == HttpStatusCode.NotFound
                        || responseCliente.StatusCode == HttpStatusCode.NotFound)
                    {
                        return SettingsData.Default._404;
                    }

                    var jsonAdministrador = await responseAdministradores.Content.ReadAsStringAsync();
                    var jsonEmpleado = await responseEmpleados.Content.ReadAsStringAsync();
                    var jsonCliente = await responseCliente.Content.ReadAsStringAsync();

                    var jsonUsuario = await responseUsuarios.Content.ReadAsStringAsync();

                    if (responseAdministradores.IsSuccessStatusCode &&
                        responseCliente.IsSuccessStatusCode && 
                        responseEmpleados.IsSuccessStatusCode &&
                        responseUsuarios.IsSuccessStatusCode)
                    {

                        var administradoresResponse = JsonConvert.DeserializeObject<ApiResponse<List<Administrador>>>(jsonAdministrador);
                        var empleadosResponse = JsonConvert.DeserializeObject<ApiResponse<List<Empleado>>>(jsonEmpleado);
                        var clientesResponse = JsonConvert.DeserializeObject<ApiResponse<List<Cliente>>>(jsonCliente);

                        var usuariosResponse = JsonConvert.DeserializeObject<ApiResponse<List<Usuario>>>(jsonUsuario);

                        AllPerfiles = new ObservableCollection<UsuarioBase>();
                        AllUsers= new ObservableCollection<Usuario>();

                        foreach (var user in usuariosResponse.data)
                        {
                            if (!string.IsNullOrEmpty(user._id) && user != null)
                                AllUsers.Add(new Usuario
                                {
                                    _id = user._id,
                                    password = user.password,
                                    email = user.email,
                                    emailApp = user.emailApp,

                                });
                        }
                        foreach (var administrador in administradoresResponse.data) 
                        {
                            if (!string.IsNullOrEmpty(administrador._id) && administrador != null)
                                AllPerfiles.Add(new Administrador
                                {
                                    _id = administrador._id,
                                    idUsuario = administrador.idUsuario,
                                    nombre = administrador.nombre,
                                    apellido = administrador.apellido,
                                    dni = administrador.dni,
                                    rol = administrador.rol,
                                    date = administrador.date,
                                    ciudad = administrador.ciudad,
                                    sexo = administrador.sexo,
                                    registro = administrador.registro,
                                    baja = administrador.baja,
                                    rutaFoto = "http://127.0.0.1:3505/" + administrador.rutaFoto,
                                });
                        }
                        foreach (var empleado in empleadosResponse.data) 
                        {
                            if (!string.IsNullOrEmpty(empleado._id) && empleado != null)
                                AllPerfiles.Add(new Empleado
                                {
                                    _id = empleado._id,
                                    idUsuario = empleado.idUsuario,
                                    nombre = empleado.nombre,
                                    apellido = empleado.apellido,
                                    dni = empleado.dni,
                                    rol = empleado.rol,
                                    date = empleado.date,
                                    ciudad = empleado.ciudad,
                                    sexo = empleado.sexo,
                                    registro = empleado.registro,
                                    baja = empleado.baja,
                                    rutaFoto = "http://127.0.0.1:3505/" + empleado.rutaFoto,
                                });
                        }
                        foreach (var cliente in clientesResponse.data) 
                        {
                            if (!string.IsNullOrEmpty(cliente._id) && cliente!= null)
                                AllPerfiles.Add(new Cliente
                            {
                                _id = cliente._id,
                                idUsuario = cliente.idUsuario,
                                nombre = cliente.nombre,
                                apellido = cliente.apellido,
                                dni = cliente.dni,
                                rol = cliente.rol,
                                date = cliente.date,
                                ciudad = cliente.ciudad,
                                sexo = cliente.sexo,
                                registro = cliente.registro,
                                baja = cliente.baja,
                                rutaFoto = "http://127.0.0.1:3505/" + cliente.rutaFoto,
                            });
                        }
                        OnPropertyChanged("AllPerfiles");
                        OnPropertyChanged("AllUsers");
                        return SettingsData.Default._200;
                    }
                    else {
                        dynamic usuarioResponse = JsonConvert.DeserializeObject<dynamic>(jsonUsuario);
                        dynamic administradorResponse = JsonConvert.DeserializeObject<dynamic>(jsonAdministrador);
                        dynamic empleadoResponse = JsonConvert.DeserializeObject<dynamic>(jsonEmpleado);
                        dynamic clienteResponse = JsonConvert.DeserializeObject<dynamic>(jsonCliente);
                        if (usuarioResponse.ReasonPhrase == "Token Expired" ||
                            administradorResponse.ReasonPhrase == "Token Expired" ||
                            empleadoResponse.ReasonPhrase == "Token Expired" ||
                            clienteResponse.ReasonPhrase == "Token Expired")
                        {
                            ClearSettings();    
                            return SettingsData.Default._419;
                        }
                        else {
                            Debug.Write($"WPF : Error 500 : " +
                                $"\nAdministrador status: {responseAdministradores.StatusCode} , Contenido : {jsonAdministrador} " +
                                $"\nEmpleado status: {responseEmpleados.StatusCode}, Contenido : {jsonEmpleado}" +
                                $"\nCliente status: {responseCliente.StatusCode}, Contenido : {jsonCliente}" +
                                $"\nUsuarios status: {responseUsuarios.StatusCode}, Contenido : {jsonUsuario}");

                            return SettingsData.Default._500;
                        }
                    }     
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Mensaje: " + e.Message +
                        "\nCausa Raíz: " + e.InnerException.Message +
                        "\nDetalle Técnico: " + e.InnerException.InnerException.Message);
                    return SettingsData.Default._503;
                }
            }
        }

        //Sin Ruta
        public async Task<HttpResponseMessage> EliminarPerfil(string id, string role) {

            string rutaEliminar = "";
            if (role == "Administrador")
                rutaEliminar = ApiRouteUsuarios.Administrador.Eliminar;
            if (role == "Empleado")
                rutaEliminar = ApiRouteUsuarios.Empleado.Eliminar;
            if (role == "Cliente")
                rutaEliminar = ApiRouteUsuarios.Cliente.Eliminar;

            using (var client = new HttpClient()) {
                try {
           
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",UserSession.Instance.Token);
                    client.DefaultRequestHeaders.Add("x-user-role", UserSession.Instance.Data["rol"].ToString());

                    var json = JsonConvert.SerializeObject(new { _id = id, rol = role });
                    var content = new StringContent(json,Encoding.UTF8,"application/json");

                    var request = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(rutaEliminar),
                        Content = content
                    };

                    var response = await client.SendAsync(request);


                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(response);
                        return response;

                    }
                    else {
                        Debug.WriteLine(response);
                        return response;
                    }

                }catch (Exception e) { throw new Exception(e.Message); }
            }
        }

        public async Task<bool> EmailDisponible(string emailDisponible)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(new {email = emailDisponible});
                    var content = new StringContent (json,Encoding.UTF8,"application/json");

                    var responseEmail = await cliente.PostAsync(ApiRouteUsuarios.Usuario.EmailDisponible, content);

                    if (responseEmail.IsSuccessStatusCode)
                    {
                            return true;
                    }
                    else
                    {
                        Debug.WriteLine($"Errors: {responseEmail.StatusCode}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex.Message}");
                    return false;
                }
            }
        }

        public async Task<HttpResponseMessage> EditarPerfil(string id,string rol, MultipartFormDataContent usuarioEditar) {
            string rutaPerfilEditar = "";

            if (rol == "Administrador"){ rutaPerfilEditar = $"{ApiRouteUsuarios.Administrador.Editar}/{id}"; }
            else if (rol == "Empleado") { rutaPerfilEditar = $"{ApiRouteUsuarios.Empleado.Editar}/{id}"; }
            else if (rol == "Cliente") { rutaPerfilEditar = $"{ApiRouteUsuarios.Cliente.Editar}/{id}"; }
            else { MessageBox.Show("Error de progrmacion. Llamar al desarrollador", "Error :", MessageBoxButton.OK, MessageBoxImage.Error); }

            using (var client = new HttpClient()) {
                try {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",SettingsData.Default.token);
                    client.DefaultRequestHeaders.Add("x-user-role", SettingsData.Default.rol);

                    HttpResponseMessage response = await client.PutAsync(rutaPerfilEditar, usuarioEditar);

                    var respuestaContenido = await response.Content.ReadAsStringAsync();
                    // Imprimir la respuesta para depuración
                    Debug.WriteLine($"Código de estado: {response.StatusCode}");
                    Debug.WriteLine($"Contenido de la respuesta: {respuestaContenido}");

                    if (response.IsSuccessStatusCode) {
                        dynamic respuestaContenidoJson = JsonConvert.DeserializeObject<dynamic>(respuestaContenido);
                        Debug.WriteLine($"Usuario Editado. {respuestaContenidoJson.user}");
                        return response;
                    } else {
                        Debug.WriteLine($"Error al Editar.");
                        return response;
                    }
                    
                } catch (Exception e) { throw new Exception("WPF : ViewModel : "+e.Message); }
            }


        }

        public async void Buscar(string rol,MultipartFormDataContent usuarioBuscar) {
            string rutaBuscar = rol == "Administrador" ? ApiRouteUsuarios.Administrador.Buscar :
                                rol == "Empleado" ? ApiRouteUsuarios.Empleado.Buscar : ApiRouteUsuarios.Cliente.Buscar;

            using (var client = new HttpClient()) 
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SettingsData.Default.token);
                    client.DefaultRequestHeaders.Add("x-user-role", SettingsData.Default.rol);

                    HttpResponseMessage respuesta = await client.PostAsync(rutaBuscar, usuarioBuscar);

                    var respuestaContenido = await respuesta.Content.ReadAsStringAsync();

                    Debug.WriteLine($"Status: {respuesta.StatusCode} \n Contenido: {respuestaContenido}");

                    if (respuesta.IsSuccessStatusCode)
                    {
                        var usuariosResponse = JsonConvert.DeserializeObject<ApiResponse<List<UsuarioBase>>>(respuestaContenido);

                        AllPerfiles = new ObservableCollection<UsuarioBase>();
                        

                        foreach (var usuario in usuariosResponse.data)
                        {
                            if (!string.IsNullOrEmpty(usuario._id) && usuario._id != null)
                                AllPerfiles.Add(new Administrador
                                {
                                    _id = usuario._id,
                                    idUsuario = usuario.idUsuario,
                                    nombre = usuario.nombre,
                                    apellido = usuario.apellido,
                                    dni = usuario.dni,
                                    rol = usuario.rol,
                                    date = usuario.date,
                                    ciudad = usuario.ciudad,
                                    sexo = usuario.sexo,
                                    registro = usuario.registro,
                                    rutaFoto = "http://127.0.0.1:3505/" + usuario.rutaFoto,
                                });

                            OnPropertyChanged("AllPerfiles");

                        }
                    }
                    else {
                        MessageBox.Show("No encontrado");
                    }
                }
                catch (Exception e) { MessageBox.Show($"{e.Message}","Error : ",MessageBoxButton.OK,MessageBoxImage.Error); }

            }


        }

        //MODULADO
        public async Task<bool> AccessToken()
        {
            using (var client = new HttpClient())
            {
                try
                {//Se envia el token
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SettingsData.Default.token);
 
                    var response = await client.GetAsync(ApiRouteUsuarios.Usuario.AccessToken);

                    if (response == null) {
                        //Caso(s): El servidor esta apagado
                        dynamic nullContet = new { ReasonPhrase = "Error de conexión.",Content = "Por favor revise su conexión al servidor." };
                        ShowNotification(nullContet);
                        return false;
                    }
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    //Caso(s): Respuesta Efectiva 
                    if (response.IsSuccessStatusCode)
                        return true;
                    else {
                        MessageBox.Show("Entro no success ?");
                        //Caso(s): Verifico el contenido de la respuesta porque puede ser JSON/HTML
                        string contentType = response.Content.Headers.ContentType?.MediaType ?? "unknown";
                        if (contentType == "application/json") { //Si es JSON
                            var errorContet = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                            ShowNotification(errorContet);
                            ClearSettings(); //Borro SettingData dado que es provable que aqui contenga un token expirado.
                            return false;
                        }else { //SI es HTML

                            string contenidoExtraido = ExtractPreContent(jsonContent);
                            dynamic errorHtml = new{ ReasonPhrase = response.ReasonPhrase, Content = contenidoExtraido};
                            ShowNotification(errorHtml);
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    //Caso(s): Manejar otros errores
                    Debug.WriteLine("Error desconocido: " + e);
                    dynamic exceptionContet = new { ReasonPhrase = "Exception Access Token.", Content = e.Message.ToString() };
                    ShowNotification(exceptionContet);
                    return false;
                }
            }
        }

        public async Task<HttpResponseMessage> RecuperarPassword(string _email) {

            using (var cliente = new HttpClient()) {
                try {

                    var json = JsonConvert.SerializeObject(new {email = _email });
                    var content = new StringContent(json, Encoding.UTF8, "application/json"); 

                    var response = await cliente.PostAsync(ApiRouteUsuarios.Usuario.RecuperarPassword, content);

                    var contentError = await response.Content.ReadAsStringAsync();


                    Debug.WriteLine("Status: \n" + response.StatusCode);
                    Debug.WriteLine("Contenido: \n" + contentError);

                    return response;
                }
                catch (Exception) {
                    return null;
                }
            }
        
        }

        public async Task<HttpResponseMessage> CambiarPassword(StringContent content) {

            using (var client = new HttpClient()) {

                try {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",SettingsData.Default.token);
                    client.DefaultRequestHeaders.Add("x-user-role",SettingsData.Default.rol);

                    HttpResponseMessage response = await client.PostAsync(ApiRouteUsuarios.Usuario.CambiarPassword, content);

                    var contentError = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine("Status: \n" + response.StatusCode);
                    Debug.WriteLine("Contenido: \n" + contentError);

                    return response; 

                } catch (Exception) {
                    return null;
                }
                


            }
        }

        //Registro Usuarios/Perfiles
        public async Task<HttpResponseMessage> RegistrarUsuario(Usuario UsuarioNuevo)
        {
            using (var cliente = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(UsuarioNuevo);
                Debug.WriteLine(json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await cliente.PostAsync(ApiRouteUsuarios.Usuario.Registro, content);

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
                    HttpResponseMessage response = await cliente.PostAsync(ApiRouteUsuarios.Usuario.Verificar, content);

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

        //Notificacion base
        private void ShowNotification(dynamic content)
        {
            //Decidi recibir los datos asi dado asi es como un respose regresa algunas respuestas automaticas, de manera que todos mis end points son iguales para swe homogeneo
            Notificacion not = new Notificacion(content.ReasonPhrase.ToString(), content.Content.ToString());
            //Se busca la venta actual para poder bloquear la ventana que lo ejecuta.
            not.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            not.ShowDialog();
        }

        //En caso nesecario se borran los datos almacenados.
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
            var match = Regex.Match(html, @"<pre>(.*?)<\/pre>", RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value : "No se encontró contenido en <pre>";
        }

        //public async Task<HttpResponseMessage> LogIn(Usuario UsuarioNuevo)
        //{
        //    using (var cliente = new HttpClient())
        //    {
        //        var json = JsonConvert.SerializeObject(UsuarioNuevo);
        //        Debug.WriteLine(json);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        try
        //        {
        //            HttpResponseMessage response = await cliente.PostAsync(ApiUrlLogIn, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var contenido = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<dynamic>(contenido);
        //                Debug.WriteLine("Perfil logeado o 1/2");
        //                Debug.WriteLine("Contenido: " + contenido);

        //                return response;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Success / no");
        //                Debug.WriteLine($"Error: {response.StatusCode}");
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Exception: {ex.Message}");
        //            return null;
        //        }
        //    }
        //}
    }
}


//NOTAS

//Inicializar un HTTPResponseMessage
//HttpResponseMessage response = new HttpResponseMessage
//{
//    StatusCode = HttpStatusCode.OK,
//    Content = new StringContent("{\"header\": \"Titulo\", \"content\": \"Contenido de la respuesta\"}", Encoding.UTF8, "application/json"),
//    Headers = {
//        { "Header-Name", "Header-Value" }
//    }
//};

//string jsonContent = "";

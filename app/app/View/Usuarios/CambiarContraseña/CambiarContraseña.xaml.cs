using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using app.Models.Usuarios;
using app.View.Usuarios.Notificaciones;
using app.ViewModel.Usuarios;
using Newtonsoft.Json;

namespace app.View.Usuarios.CambiarContraseña
{
    /// <summary>
    /// Lógica de interacción para CambiarContraseña.xaml
    /// </summary>
    public partial class CambiarContraseña : Window
    {
        public readonly UsuarioViewModel _viewModel;

        UsuarioBase perfilEdita;
        string IDUsuario;
        Usuario usuarioEdita;
        public CambiarContraseña()
        {
            InitializeComponent();
            _viewModel = UsuarioViewModel.Instance; 
            DataContext = _viewModel;
            BindingExpression bindingPassword = txtPassword.GetBindingExpression(TextBox.TextProperty);
            BindingExpression bindingPasswordConfirm = txtPasswordConfirmacion.GetBindingExpression(TextBox.TextProperty);
            if (bindingPassword != null)
                bindingPassword?.UpdateSource();
            if (bindingPasswordConfirm != null)
                bindingPasswordConfirm?.UpdateSource();
            ValidPasswords();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            perfilEdita = _viewModel.AllPerfiles.FirstOrDefault(item => item._id == SettingsData.Default.idPerfil);
            IDUsuario = perfilEdita.idUsuario;
            usuarioEdita = _viewModel.AllUsers.FirstOrDefault(item => item._id == IDUsuario);

            txtNombre.Text = perfilEdita.nombre;
            txtEmailApp.Text = usuarioEdita.emailApp;
            txtEmail.Text = usuarioEdita.email;
            string imageUrl = perfilEdita.rutaFoto; // Ruta de la API
            BitmapImage bitmap = new BitmapImage();

            try
            {
                // Cargar la imagen desde la URL
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Asegura que la imagen se carga completamente
                bitmap.EndInit();

                // Crear un ImageBrush y asignarlo al Ellipse
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmap;
                imageBrush.Stretch = Stretch.UniformToFill; // Asegura que la imagen llena el Ellipse correctamente
                imgPerfil.Fill = imageBrush;
                //Guardo la imagen en una variable para despues usarla


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.Passwordd = "";
            _viewModel.PasswordConfirm = "";
            _viewModel.PasswordConfirm2 = "";
            this.Close();
        }

        private async void btnCambiar_Click(object sender, RoutedEventArgs e)
        {
            btnCambiar.IsEnabled = false;

            //MessageBox.Show(usuarioEdita.email+" "+usuarioEdita.emailApp+" "+txtPassword);


            var json = JsonConvert.SerializeObject(new { email = usuarioEdita.email, emailApp = usuarioEdita.emailApp, password = txtPassword.Text });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _viewModel.CambiarPassword(content);
            if (response == null){

                Notificacion not = new Notificacion("Error de conexión.", "Por favor revise  su conexión al servidor.");
                not.Owner = this;
                not.ShowDialog();
                _viewModel.Passwordd = "";
                _viewModel.PasswordConfirm = "";
                _viewModel.PasswordConfirm2 = "";
                btnCambiar.IsEnabled = true;

            }
            else if (response.IsSuccessStatusCode)
            {
                var result200= await response.Content.ReadAsStringAsync();
                var _200 = JsonConvert.DeserializeObject<dynamic>(result200);
                Notificacion not = new Notificacion(_200.ReasonPhrase.ToString(), _200.Content.ToString());
                not.Owner = this;
                not.ShowDialog();

                _viewModel.Passwordd = "";
                _viewModel.PasswordConfirm = "";
                _viewModel.PasswordConfirm2 = "";
                this.Close();
               
            }
            else {
                var resultError = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(resultError);
                Notificacion not = new Notificacion(error.ReasonPhrase.ToString(), error.Content.ToString());
                not.Owner = this;
                not.ShowDialog();
                _viewModel.Passwordd = "";
                _viewModel.PasswordConfirm = "";
                _viewModel.PasswordConfirm2 = "";
                btnCambiar.IsEnabled = true;
            }

            _viewModel.Passwordd = "";
            _viewModel.PasswordConfirm = "";
            _viewModel.PasswordConfirm2 = "";
            btnCambiar.IsEnabled = true;    
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidPasswords();
        }

        private void txtPasswordConfirmacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidPasswords();
        }

        public void ValidPasswords(){

            bool errores = Validation.GetHasError(txtPassword) || Validation.GetHasError(txtPasswordConfirmacion);
            btnCambiar.IsEnabled = !errores;
        }
    }
}

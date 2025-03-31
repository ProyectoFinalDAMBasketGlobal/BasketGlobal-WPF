using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using app.View.Usuarios.InicioDeSesion;
using app.View.Usuarios.Notificaciones;
using app.ViewModel.Usuarios;
using Newtonsoft.Json;

namespace app.View.Usuarios.RecordarContraseñas
{
    /// <summary>
    /// Lógica de interacción para RecordarContraseña.xaml
    /// </summary>
    public partial class RecordarContraseña : Window
    {
        private readonly UsuarioViewModel  _viewModel;
        public RecordarContraseña()
        {
            InitializeComponent();
            _viewModel = UsuarioViewModel.Instance;
            DataContext = _viewModel;
        }

        private async void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            btnEnviar.IsEnabled = false;
            HttpResponseMessage response = await _viewModel.RecuperarPassword(txtEmail.Text);  

            if (response == null)
            {
                Notificacion not503 = new Notificacion("Error: ", SettingsData.Default._503);
                not503.Owner = this;
                not503.ShowDialog();
                btnEnviar.IsEnabled = true;
            }
            else if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(content);
                Notificacion not200 = new Notificacion(result.header.ToString(),result.message.ToString());
                not200.Owner = this;
                not200.ShowDialog();
                InicioDeSesion.LogIn log = new InicioDeSesion.LogIn();
                log.Show();
                this.Close();
            }
            else {
                var contentErrorr = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(contentErrorr);
                Notificacion not500 = new Notificacion(error.header.ToString(), error.message.ToString());
                not500.Owner = this;
                not500.ShowDialog();
                btnEnviar.IsEnabled = true;
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InicioDeSesion.LogIn log = new InicioDeSesion.LogIn();
            log.Show();
            this.Close();
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidarEmailUsuario();

        }

        private void TextBoxEmailConfirmar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidarEmailUsuario();
        }

        private void ValidarEmailUsuario()
        {
            // Obtengo los valores ingresados en los campos de texto para email
            string email = txtEmail.Text;
            string emailConfirmar = txtEmailConfirmar.Text;
            

            // Valido el email
            bool isEmailValid = IsValidEmail(email);
            bool isEmailConfirmVaid = IsValidEmail(emailConfirmar);
            bool isEquals = txtEmail.Text == emailConfirmar;    

            // Muestro u oculto el mensaje de error para el campos dependiendo de su validez
            ErrorTextEmail.Visibility = isEmailValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextEmailConfirmar.Visibility = isEmailConfirmVaid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextEquals.Visibility = isEquals ? Visibility.Collapsed : Visibility.Visible;

            // Habilito el botón de inicio de sesión solo si ambos campos son válidos
            btnEnviar.IsEnabled = isEmailValid && isEmailConfirmVaid && isEquals;
        }

        public bool IsValidEmail(string email)
        {

            //Si no es nula regreso false
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar un email, se agrega using System.Text.RegularExpressions;
            //Explicación del patter al final del documento.
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

    }
}

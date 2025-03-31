using System;
using System.Collections.Generic;
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
using app.Models.Usuarios;
using app.ViewModel.Usuarios;
using app.ViewModel.Usuarios.RegistroUsuarios;
using Newtonsoft.Json;

namespace app.View.Usuarios.RegistroUsuarios
{
    /// <summary>
    /// Lógica de interacción para CodigoDeVerificacion.xaml
    /// </summary>
    public partial class CodigoDeVerificacion : Window
    {
        private readonly UsuarioViewModel _viewModel;
        public string Email { get; set; }
        public string ID { get; set; }
        public string EmailApp { get; set; }

        public string Privileges { get; set; } 
        public CodigoDeVerificacion()
        {
            InitializeComponent();
            _viewModel = UsuarioViewModel.Instance;
            DataContext = _viewModel;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Asignar valores a los campos de texto al cargar el diálogo
            TextEmail.Text = Email;
        }


        private void txtCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCode();
        }
        private void ValidateCode()
        {
            
            string codigo = txtCodigo.Text;
          

            // Valido 
            bool isCodeValid = IsValidCode(codigo);
          

            // Muestro u oculto el mensaje de error para el campos dependiendo de su validez
            ErrorTextCodigo.Visibility = isCodeValid ? Visibility.Collapsed : Visibility.Visible;

            // Habilito el botón 
            btnEnviar.IsEnabled = isCodeValid;
        }

        public bool IsValidCode(string codigo)
        {
            
            //Si no es nula regreso false
            if (string.IsNullOrWhiteSpace(codigo))
                return false;

            if (!int.TryParse(codigo, out int val)) 
                return false;

            if (codigo.Length <= 5)
                return false;

            return true;
        }

        private async void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            var verificarUsuario = new RegistroUsuarioViewModel();

            Usuario usuario = new Usuario
            {
                emailApp = EmailApp,
                _id = ID,
                verificationCode = txtCodigo.Text

            };
            var response = await _viewModel.ValidarUsuario(usuario);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic responseData = JsonConvert.DeserializeObject<dynamic>(result);

                if (responseData != null) {
                    
                    RegistroPerfil perfil = new RegistroPerfil(){
                        EmailApp = responseData.data.emailApp,
                        IdUsuario = responseData.data.idUsuario,
                        Privileges = responseData.data.privileges,
                        TemporalToken = responseData.data.temporalToken,             
                    };
                    this.DialogResult = true;

                    perfil.Owner = this.Owner;
                    this.Hide();
                   
                    perfil.ShowDialog();
                    
                    this.Close();


                }
                else {
                    var error = JsonConvert.DeserializeObject<Exception>(result);
                    MessageBox.Show(error + " : WPF = ERROR 500");
                }
            }
            else 
            {
                var error = JsonConvert.DeserializeObject<dynamic>(result);
                MessageBox.Show(error+" : WPF : error 404");
            }

         
        }

        private async void btnCerrar_Click(object sender, RoutedEventArgs e)
        {

            if (Privileges == null)
            {
                var confirmacion = MessageBox.Show(
                "Estas seguro que quieres cerrar ?",
                "Se va a eliminar el usuario ya registrado...",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    var viewModelCodigoVerificacion = new CodigoVerificacionViewModel();

                    var response = await viewModelCodigoVerificacion.EliminarUsuario(EmailApp);

                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var status = JsonConvert.DeserializeObject(result);
                        MessageBox.Show(
                        $"usuario Eliminado. {status}",
                        "Status : 200",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                        );
                        this.Close();
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject(result);
                        MessageBox.Show(
                            $"Error al eliminar el usuario. {error}",
                            "Error : 500",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                            );

                    }

                }

            }
            else {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}

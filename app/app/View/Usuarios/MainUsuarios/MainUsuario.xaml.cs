﻿using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using app.Models.Usuarios;
using app.View.Home;
using app.View.Usuarios.EditarUsuarios;
using app.View.Usuarios.InformacionUsuarios;
using app.View.Usuarios.Notificaciones;
using app.View.Usuarios.Pre_Registros;
using app.View.Usuarios.RegistroUsuarios;
using app.ViewModel.Usuarios;
using Newtonsoft.Json;
using app.View.Reservas;
using app.View.Productos_BasketGlobal;

namespace app.View.Usuarios.MainUsuarios
{
    /// <summary>
    /// Lógica de interacción para MainUsuario.xaml
    /// </summary>
    public partial class MainUsuario : Window
    {
        //Instacia que guarda las variables que se usaran durante la ejecucion del programa
        private readonly UsuarioViewModel _viewModel;

        private MultipartFormDataContent _multipartFormDataContent;
        public MainUsuario()
        {
            InitializeComponent();
            
            _viewModel = UsuarioViewModel.Instance;
            this.DataContext = _viewModel;

            //Menu perfil
            txtUsuarioRol.Text = SettingsData.Default.rol;
            txtUsuarioSession.Text = SettingsData.Default.nombre;
        }


        //Pediente a eliminar 
        private void btnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registro = new RegistroUsuario();
            registro.Owner = this;
            registro.ShowDialog();
            
        }

        //OK
        private void btnPreRegistro_Click(object sender, RoutedEventArgs e)
        {
            Pre_Registro pre = new Pre_Registro();
            pre.Owner = this;
            pre.ShowDialog();
        }

        //OK
        private void btnRegistro_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registro = new RegistroUsuario();
            registro.Owner = this;
            registro.ShowDialog();
        }

        //Session OK
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargaUsuariosSession();
        }

        //Session OK
        private void ToggleButtonCambiarLista_Checked(object sender, RoutedEventArgs e)
        {
            DataGridPerfilUsuarios.Visibility = Visibility.Collapsed;
            ListViewPerfilUsuarios.Visibility = Visibility.Visible;
            btnToggleButtonCambiarLista.Content = "ListView";
            imgToggleButton.Source = new BitmapImage(new Uri("/View/Usuarios/MainUsuarios/imgListView.png", UriKind.RelativeOrAbsolute)); ;

            CargaUsuariosSession();
        }

        //Session OK
        private void ToggleButtonCambiarLista_Unchecked(object sender, RoutedEventArgs e)
        {
            ListViewPerfilUsuarios.Visibility = Visibility.Collapsed;
            DataGridPerfilUsuarios.Visibility = Visibility.Visible;
            btnToggleButtonCambiarLista.Content = "DataGrid";
            imgToggleButton.Source = new BitmapImage(new Uri("/View/Usuarios/MainUsuarios/imgDataGrid.png", UriKind.RelativeOrAbsolute)); ;

            CargaUsuariosSession();
        }

        //Session P
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioBase usuarioSeleccionado = null;
            if (DataGridPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)DataGridPerfilUsuarios.SelectedItem; }
            else if (ListViewPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)ListViewPerfilUsuarios.SelectedItem; }
            else { MessageBox.Show("Por favor, selecciona una usuario para editar.", "Error.",MessageBoxButton.OK,MessageBoxImage.Error); return; }


            if (sender is Button button && button.DataContext is UsuarioBase usuario)
            {
                // Se obtiene el usuario correcto de la fila del botón presionado
                EditarUsuario edit = new EditarUsuario(usuario._id, usuario.rol, _viewModel);
                edit.Owner = this;
                edit.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            CargaUsuariosSession();
        }

        //P
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            UsuarioBase usuarioSeleccionado = null;
            if (DataGridPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)DataGridPerfilUsuarios.SelectedItem; }
            else if (ListViewPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)ListViewPerfilUsuarios.SelectedItem; }
            else { MessageBox.Show("Por favor, selecciona una usuario para editar.", "Error.", MessageBoxButton.OK, MessageBoxImage.Error); return; }


            if (sender is Button button && button.DataContext is UsuarioBase usuario)
            {
                // Se obtiene el usuario correcto de la fila del botón presionado
                InformacionUsuario info = new InformacionUsuario(usuario._id, _viewModel);
                info.Owner = this;
                info.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        


        }

        //P
        private async void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            UsuarioBase usuarioSeleccionado = null;

            if (DataGridPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)DataGridPerfilUsuarios.SelectedItem; }
            else if (ListViewPerfilUsuarios.SelectedItem != null) { usuarioSeleccionado = (UsuarioBase)ListViewPerfilUsuarios.SelectedItem; }
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, selecciona una usuario para borrar.", "Error !", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show($"¿Estás seguro de que quieres eliminar la cuenta seleccionado ?", "Confirmar eliminación", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                var response = await _viewModel.EliminarPerfil(usuarioSeleccionado._id.ToString(),usuarioSeleccionado.rol.ToString());
                var resultEliminar = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var status = JsonConvert.DeserializeObject(resultEliminar);
                    MessageBox.Show("Correctamente...", "Usuario Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Hubo un error al eliminar el usuario . Por favor, intenta de nuevo.");
                }
            }
        }

        //Cambiar Contraseña P
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CambiarContraseña.CambiarContraseña noti = new CambiarContraseña.CambiarContraseña();
            noti.Owner = this;
            noti.ShowDialog();
   
        }


        //OK
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            this.Close();
        }

        //OJO
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _multipartFormDataContent = new MultipartFormDataContent(); 

            ComboBoxItem itemComboBox = (ComboBoxItem)txtRol.SelectedItem;
            
            if (itemComboBox != null) { 
                string contenidoSeleccionado = itemComboBox.Content.ToString();
                _multipartFormDataContent.Add(new StringContent(contenidoSeleccionado),"rol");
            }

            if (!string.IsNullOrEmpty(txtNombre.Text))
                _multipartFormDataContent.Add(new StringContent(txtNombre.Text.Trim()),"nombre");

            if (!string.IsNullOrEmpty(txtDni.Text))
                _multipartFormDataContent.Add(new StringContent(txtDni.Text.Trim()), "dni");

            if (!string.IsNullOrEmpty(txtDate.Text))
                _multipartFormDataContent.Add(new StringContent(txtDate.Text.Trim()), "date");
            if (txtBaja.IsChecked == true)
            {
                _multipartFormDataContent.Add(new StringContent("true"), "baja");
            }
      

            if (!_multipartFormDataContent.Any(c => c.Headers.ContentDisposition.Name.Trim('"') == "rol"))
            {
                MessageBox.Show("Se requiere un rol...", "Ojo...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                var rolContent = _multipartFormDataContent.FirstOrDefault(c => c.Headers.ContentDisposition.Name.Trim('"') == "rol");
                string rolValue = await rolContent.ReadAsStringAsync();

                _viewModel.Buscar(rolValue, _multipartFormDataContent);
                
            }

            ResetFields();

        }
        
        // Método para restablecer los campos
        private void ResetFields()
        {
            txtNombre.Text = string.Empty; // Vaciar el TextBox
            txtDni.Text = string.Empty; // Vaciar el TextBox
            txtBaja.IsChecked = false; // Vaciar el TextBox
            txtDate.SelectedDate = null; // Vaciar el DatePicker
            txtRol.SelectedIndex = -1; // Restablecer el ComboBox
        }

        //Session OK
        private async void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            var ex = await _viewModel.CargarTodosLosUsuarios();
            if (ex == SettingsData.Default._200)
                return;
            else if (ex != SettingsData.Default._419)
            {
                Notificacion notEx = new Notificacion("Error", ex);
                notEx.Owner = this;
                notEx.ShowDialog();

                InicioDeSesion.LogIn logEx = new InicioDeSesion.LogIn();
                logEx.Show();
                this.Close();
            }
            else
            {
                Notificacion not = new Notificacion("Session Terminada", "Por favor inicie session.");
                not.Owner = this;
                not.ShowDialog();
                InicioDeSesion.LogIn log = new InicioDeSesion.LogIn();
                log.Show();
                this.Close();
            }
        }

        //OK
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
           Inicio ini = new Inicio();
            ini.Show();
            this.Close();
        }


        //Pensar si se puede hacer algo
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            //Notificacion NOTI
            //   = new Notificacion();
            //NOTI.Owner = this;
            //NOTI.ShowDialog();
        }

        private async void CargaUsuariosSession() {


            var ex = await _viewModel.CargarTodosLosUsuarios();
            if (ex == SettingsData.Default._200)
                return;
            else if (ex != SettingsData.Default._419)
            {
                Notificacion notEx = new Notificacion("Error", ex);
                notEx.Owner = this;
                notEx.ShowDialog();
                InicioDeSesion.LogIn logEx = new InicioDeSesion.LogIn();
                logEx.Show();
                this.Close();
            }
            else
            {
                Notificacion not = new Notificacion("Session Terminada", "Por favor inicie session.");
                not.Owner = this;
                not.ShowDialog();
                
                InicioDeSesion.LogIn log = new InicioDeSesion.LogIn();
                log.Show();
                this.Close();
            }
        }



        private void MenuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            //_viewModel.IsViewVisible = true;
            SettingsData.Default.token = "";
            SettingsData.Default.appToken = "";
            SettingsData.Default.idPerfil = "";
            SettingsData.Default.Save();

            //// Verifica si LogIn ya existe y está oculto
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window is InicioDeSesion.LogIn logInView)
            //    {
            //        logInView.Show();
            //        this.Close();
            //        return;
            //    }
            //}

            // Si no encuentra LogIn, crea una nueva instancia (en caso de que se haya cerrado completamente)
            var newLogIn = new InicioDeSesion.LogIn();
            newLogIn.Show();
            this.Close();
            
            
        }

        private void MenuCambiarContraseña_Click(object sender, RoutedEventArgs e)
        {
            CambiarContraseña.CambiarContraseña cam = new CambiarContraseña.CambiarContraseña();
            cam.Owner = this;
            cam.ShowDialog();

        }

        private void MenuApagar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnReservas_Click(object sender, RoutedEventArgs e)
        {
            MainAdquisiciones mainReservas = new MainAdquisiciones();
            mainReservas.Show();
            this.Close();
        }

        private void btnHabitaciones_Click(object sender, RoutedEventArgs e)
        {
            BuscadorProductos mainHabit = new BuscadorProductos();
            mainHabit.Show();
            this.Close();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using app.View.Usuarios.RegistroUsuarios;
using app.ViewModel.Usuarios;
using Newtonsoft.Json;

namespace app.View.Usuarios.Pre_Registros
{
    /// <summary>
    /// Lógica de interacción para Pre_Registro.xaml
    /// </summary>
    public partial class Pre_Registro : Window
    {
        private readonly UsuarioViewModel _viewModel;
        public Pre_Registro()
        {
            InitializeComponent();
            _viewModel = UsuarioViewModel.Instance;
            DataContext = _viewModel;

            BindingExpression bindingEmailConfirmacion = txtEmailConfirmar.GetBindingExpression(TextBox.TextProperty);
            if (bindingEmailConfirmacion != null)
                bindingEmailConfirmacion?.UpdateSource();

            BindingExpression bindingEmail = txtEmail.GetBindingExpression(TextBox.TextProperty);
            if (bindingEmail != null)
                bindingEmail?.UpdateSource();

            BindingExpression binding = txtRol.GetBindingExpression(ComboBox.SelectedItemProperty);
            if (binding != null)
                binding?.UpdateSource();  // Forzar la validación al cargar

            ValidateForm();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.EmailPreregistroConfirmacion2 = String.Empty;
            _viewModel.EmailPreregistroConfirmacion = String.Empty;
            _viewModel.EmailPreRegistro = String.Empty;
            _viewModel.RolSeleccionado = null;

            this.Close();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
        }

        private void txtEmailConfirmar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
        }

        private void txtRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateForm();
        }

        public void ValidateForm() {

            bool errores = Validation.GetHasError(txtEmail) || Validation.GetHasError(txtEmailConfirmar) || Validation.GetHasError(txtRol);

            btPre_Registro.IsEnabled = !errores;
        }

        private async void btPre_Registro_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsData.Default.rol == "Administrador")
            {
                btPre_Registro.IsEnabled = false;
                string rolSeleccionado = txtRol.SelectedValue as string;

                Usuario preUsers = new Usuario
                {
                    email = txtEmail.Text,
                    privileges = rolSeleccionado == "Administrador",

                };

                MessageBox.Show(preUsers.email + " " + preUsers.privileges);

                try
                {
                    var response = await _viewModel.RegistrarUsuario(preUsers);
                    var content = await response.Content.ReadAsStringAsync();

                    Debug.WriteLine($"Status: {response.StatusCode}\n Content: {content}");

                    if (response.IsSuccessStatusCode)
                    {

                        // Convertir la respuesta JSON a un objeto dynamic
                        dynamic responseData = JsonConvert.DeserializeObject<dynamic>(content);
                        // Guardar el token y los datos del usuario en la sesión de usuario
                        MessageBox.Show($"Datos que se envian a Codigo verificacion \nEmail: {responseData.data.email} , ID: {responseData.data.id}, EmailApp: {responseData.data.emailApp}  privileges: {responseData.data.privigeles}");
                        this.Close();

                    }
                    else
                    {
                        // La solicitud no fue exitosa, manejar el error
                        var error = JsonConvert.DeserializeObject<dynamic>(content);
                        MessageBox.Show(error + " : WPF  = ERROR 4047");
                        btPre_Registro.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"WPF : Ocurrió un error al cargar los datos : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    btPre_Registro.IsEnabled = true;
                }
                finally
                {
                    _viewModel.EmailPreregistroConfirmacion2 = String.Empty;
                    _viewModel.EmailPreregistroConfirmacion = String.Empty;
                    _viewModel.EmailPreRegistro = String.Empty;
                    _viewModel.RolSeleccionado = null;
                    btPre_Registro.IsEnabled = true;
                }
            }
            else {
                MessageBox.Show("No tienes la autorización requerido.","Error de permisos",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            

            
        }
    }
}

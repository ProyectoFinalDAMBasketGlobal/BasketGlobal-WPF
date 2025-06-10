using app.Models.Adquisiciones_BasketGlobal;
using app.View.Productos_BasketGlobal;
using app.View.Home;
using app.View.Usuarios.MainUsuarios;
using app.ViewModel.Adquisiciones_BasketGlobal;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace app.View.Reservas
{
    /// <summary>
    /// Lógica de interacción para MainAdquisiciones.xaml
    /// </summary>
    public partial class MainAdquisiciones : Window
    {
        public ObservableCollection<Adquisicion> Adquisiciones { get; set; }
        private readonly AdquisicionesViewModel viewModel;
        public MainAdquisiciones()
        {
            InitializeComponent();
            viewModel = new AdquisicionesViewModel();
            this.DataContext = viewModel;

            txtUsuarioRol.Text = SettingsData.Default.rol;
            txtUsuarioSession.Text = SettingsData.Default.nombre;
        }

        private void btnBuscadorAdquisicion_Click(object sender, RoutedEventArgs e)
        {
            BuscadorAdquisiciones ventanaBuscador = new BuscadorAdquisiciones();
            ventanaBuscador.Show();
            this.Close();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            var selectAdquisicion = DataGridAdquisiciones.SelectedItem as Adquisicion;

            if (selectAdquisicion != null)
            {
                ModificarAdquisicion editarReservaWindow = new ModificarAdquisicion(selectAdquisicion, viewModel);

                if (editarReservaWindow.ShowDialog() == true)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var reservaModificada = new
                            {
                                selectAdquisicion.fecha_adquisicion,
                                selectAdquisicion.estado_adquisicion
                            };

                            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"http://localhost:3505/Adquisicion/modificarAdquisicion/{selectAdquisicion._id}")
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(reservaModificada), Encoding.UTF8, "application/json")
                            };

                            var response = await client.SendAsync(request);

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Adquisicion actualizada con éxito.");
                                viewModel.CargarTodasLasAdquisiciones();
                            }
                            else
                            {
                                var errorContent = await response.Content.ReadAsStringAsync();
                                MessageBox.Show($"Error al actualizar la adquisicion: {errorContent}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}\nDetalles: {ex.InnerException?.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona adquisicion para editar.");
            }
        }

        private async void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            var selectAdquisicion = DataGridAdquisiciones.SelectedItem as Adquisicion;

            if (selectAdquisicion != null)
            {
                var result = MessageBox.Show($"¿Estás seguro de que deseas eliminar esta adquisicion: '{selectAdquisicion._id}'?",
                                             "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var response = await client.DeleteAsync($"http://localhost:3505/Adquisicion/eliminarAdquisicion/{selectAdquisicion.id_usu}/{selectAdquisicion.id_prod}");
                        
                        
                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Adquisicion eliminada con éxito.");
                                viewModel.CargarTodasLasAdquisiciones();
                            }
                            else
                            {
                                MessageBox.Show($"Error al eliminar la adquisicion. Código de estado: {response.StatusCode}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una adquisicion para eliminar.");
            }

        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            var selectAdquisicion = DataGridAdquisiciones.SelectedItem as Adquisicion;

            if (selectAdquisicion != null)
            {
                InfoAdquisicion info = new InfoAdquisicion(selectAdquisicion);
                info.Owner = this;
                info.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una adquisicion.");
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
            var newLogIn = new app.View.Usuarios.InicioDeSesion.LogIn();
            newLogIn.Show();
            this.Close();


        }

        private void MenuCambiarContraseña_Click(object sender, RoutedEventArgs e)
        {
            app.View.Usuarios.CambiarContraseña.CambiarContraseña cam = new app.View.Usuarios.CambiarContraseña.CambiarContraseña();
            cam.Owner = this;
            cam.ShowDialog();

        }

        private void MenuApagar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.CargarTodasLasAdquisiciones();
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            Inicio ventanaMain = new Inicio();
            ventanaMain.Show();
            this.Close();
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            MainUsuario mainUser = new MainUsuario();
            mainUser.Show();
            this.Close();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            BuscadorProductos mainHabit = new BuscadorProductos();
            mainHabit.Show();
            this.Close();
        }
    }
}

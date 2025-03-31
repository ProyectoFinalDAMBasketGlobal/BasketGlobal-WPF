using app.Models.Adquisiciones_BasketGlobal;
using app.ViewModel.Adquisiciones_BasketGlobal;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace app.View.Reservas
{
    /// <summary>
    /// Lógica de interacción para ModificarAdquisicion.xaml
    /// </summary>
    public partial class ModificarAdquisicion : Window
    {
        private Adquisicion _adquisicion;
        private readonly AdquisicionesViewModel _modeloVista;
        public ModificarAdquisicion(Adquisicion selectedReservation, AdquisicionesViewModel modeloVista)
        {
            InitializeComponent();
            _modeloVista = modeloVista;
            _adquisicion = selectedReservation;

            usuRes.Text = _adquisicion.id_usu;
            prodRes.Text = _adquisicion.id_prod;

            dpFechaEntrada.SelectedDate = DateTime.Now;

            // Mostrar la cantidad actual en el TextBox
            dpCantidad.Text = _adquisicion.cantidad.ToString();

            // Buscar el estado en el ComboBox y seleccionarlo
            foreach (ComboBoxItem item in cbEstadoAdquisicion.Items)
            {
                if (item.Content.ToString() == _adquisicion.estado_adquisicion)
                {
                    cbEstadoAdquisicion.SelectedItem = item;
                    break;
                }
            }
        }

        private async void btnGuardarPerfil_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los valores editados
            var fechaEntrada = dpFechaEntrada.SelectedDate;
            var estadoAdquisicion = cbEstadoAdquisicion.SelectedItem as ComboBoxItem;
            var estado = estadoAdquisicion?.Content.ToString();
            var cantidad = dpCantidad.Text;

            // Verificar si las fechas y el estado están completos
            if (fechaEntrada.HasValue && !string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(cantidad))
            {
                // Actualizar los valores de la reserva
                _adquisicion.fecha_adquisicion = fechaEntrada.HasValue
                    ? fechaEntrada.Value.ToString("yyyy-MM-dd")
                    : null;

                _adquisicion.estado_adquisicion = estado;

                // Actualizar la cantidad
                if (int.TryParse(cantidad, out int cantidadModificada))
                {
                    _adquisicion.cantidad = cantidadModificada;
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese una cantidad válida.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                HttpResponseMessage response = await _modeloVista.EditarAdquisicion(_adquisicion._id, _adquisicion);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Adquisición editada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Mostrar mensaje de éxito y cerrar la ventana
                _modeloVista.CargarTodasLasAdquisiciones();
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

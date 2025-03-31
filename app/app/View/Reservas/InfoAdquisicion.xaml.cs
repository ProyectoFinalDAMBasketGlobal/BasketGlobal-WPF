using app.Models.Adquisiciones_BasketGlobal;
using System.Windows;

namespace app.View.Reservas
{
    /// <summary>
    /// Lógica de interacción para InfoAdquisicion.xaml
    /// </summary>
    public partial class InfoAdquisicion : Window
    {
        private Adquisicion _adquisicion; // Almacena la reserva seleccionada
        public InfoAdquisicion(Adquisicion adquisicion)
        {
            InitializeComponent();
            _adquisicion = adquisicion; // Almacena la reserva pasada desde el MainWindow

            // Asigna los valores de la reserva a los controles de la interfaz
            txtNombre.Text = $"A nombre de: {SettingsData.Default.nombre}";
            txtUsuario.Text = _adquisicion.id_usu;
            txtProducto.Text = _adquisicion.id_prod;
            txtFechaEntrada.Text = _adquisicion.fecha_adquisicion;
            int cantidad = _adquisicion.cantidad;
            txtCantidad.Text = cantidad.ToString();
            txtEstadoAdquisicion.Text = _adquisicion.estado_adquisicion;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

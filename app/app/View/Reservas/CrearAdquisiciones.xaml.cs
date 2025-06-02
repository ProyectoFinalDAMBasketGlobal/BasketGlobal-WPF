using app.Models.Productos_BasketGlobal;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using app.Models.Adquisiciones_BasketGlobal;
using System.IO;
using System.Windows.Media.Imaging;

namespace app.View.Reservas
{
    /// <summary>
    /// Lógica de interacción para CrearAdquisiciones.xaml
    /// </summary>
    public partial class CrearAdquisiciones : Window
    {
        private Producto _producto;
        public CrearAdquisiciones(Producto producto)
        {
            InitializeComponent();
            _producto = producto;

            if (!string.IsNullOrEmpty(_producto.imagenBase64))
            {
                imgProducto.Source = ConvertBase64ToImage(_producto.imagenBase64);
            }
            else
            {
                imgProducto.Source = new BitmapImage(new Uri("pack://application:,,,/View/Reservas/Logo-BasketGlobal.png", UriKind.Absolute));
                // Imagen predeterminada
            }
           
            txtNombreProducto.Text = _producto._id;
            txtPrecioProducto.Text = $"{_producto.precio}€";
        }

        // Método para actualizar el precio en el mismo TextBox de precio
        private void ActualizarPrecioTotal()
        {
            // Validar que la cantidad ingresada sea un número
            if (int.TryParse(txtCantidadProducto.Text, out int cantidadProducto))
            {
                if (cantidadProducto <= _producto.stock)
                {
                    // Actualizar el precio total en el mismo TextBox de precio
                    double precioTotal = _producto.precio * cantidadProducto;
                    txtPrecioProducto.Text = $"{precioTotal}€"; // Actualizar el precio
                }
                else
                {
                    // Si la cantidad excede el stock, mostrar un mensaje de error
                    MessageBox.Show($"La cantidad no puede ser mayor que el stock disponible: {_producto.stock}.", "Error de Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtCantidadProducto.Clear(); // Limpiar el campo de cantidad
                    txtPrecioProducto.Text = ""; // Limpiar el campo de precio total
                }
            }
            else
            {
                // Si no es un número válido, limpiar el campo de precio total
                txtPrecioProducto.Text = "";
            }
        }

        // Evento para que el precio se actualice cuando la cantidad cambie
        private void txtCantidadProducto_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ActualizarPrecioTotal();
        }

        private async void btnGuardarAdquisicion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar los campos
                if (string.IsNullOrEmpty(txtCantidadProducto.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validar que el valor ingresado en el TextBox sea un número
                if (!int.TryParse(txtCantidadProducto.Text, out int cantidadProducto))
                {
                    MessageBox.Show("Por favor, ingrese un número válido para la cantidad.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validar que la cantidad no sea mayor que la capacidad del producto
                bool capacidad = cantidadProducto <= _producto.stock;

                if (!capacidad)
                {
                    MessageBox.Show($"La cantidad debe ser igual o menor que {_producto.stock}", "Error de Capacidad", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener la fecha actual en formato dd-MM-yyyy
                string fechaActual = DateTime.Now.ToString("dd-MM-yyyy");

                // Crear la reserva
                var adquisicion = new Adquisicion
                {
                    id_usu = SettingsData.Default.idPerfil,
                    id_prod = _producto._id,
                    fecha_adquisicion = fechaActual,
                    cantidad = cantidadProducto,
                    estado_adquisicion = "Pendiente"
                };

                // Enviar a la API
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(adquisicion), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:3505/Adquisicion/crearAdquisicion", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Adquisicion elaborada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al realizar la adquisicion del producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private BitmapImage ConvertBase64ToImage(string base64String)
        {
            // Convertir Base64 a imagen
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream stream = new MemoryStream(imageBytes);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}

using IntermodularWPF;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace app.View.Productos_BasketGlobal
{
    /// <summary>
    /// Lógica de interacción para AñadirProducto.xaml
    /// </summary>
    public partial class AñadirProducto : Window
    {
        private string ImagenBase64 = string.Empty;
        public AñadirProducto()
        {
            InitializeComponent();
        }

        private async void btnCrearProducto_Click(object sender, RoutedEventArgs e)
        {

            // Validar que los campos de precio no estén vacíos
            if (string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrWhiteSpace(txtPrecioOriginal.Text))
            {
                MessageBox.Show("Valor de precio o precio original está vacío. Por favor corrígelo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Detener la ejecución si los campos están vacíos
            }

            double precio;
            if (!double.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio de la noche no es válido. Por favor ingresa un valor numérico.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Detener la ejecución si el precio no es válido
            }

            double precio_original;
            if (!double.TryParse(txtPrecioOriginal.Text, out precio_original))
            {
                MessageBox.Show("El precio original no es válido. Por favor ingresa un valor numérico.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Detener la ejecución si el precio original no es válido
            }

            var nombre = txtNom.Text;

            try
            {
                ValidarNombre(nombre);
                MessageBox.Show("Nombre validado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Recopilar datos del formulario
            var categoria = txtCategoria.SelectedItem != null ? (txtCategoria.SelectedItem as ComboBoxItem)?.Content.ToString() : string.Empty;

            var marca = txtMarca.Text;

            // Deshabilitar el campo de origen si la marca es "BasketGlobal"
            if (marca.Equals("BasketGlobal", StringComparison.OrdinalIgnoreCase))
            {
                txtOrigen.IsEnabled = false;  // Deshabilitar el campo de origen
            }
            else
            {
                txtOrigen.IsEnabled = true;  // Habilitar el campo de origen
            }

            var origen = txtOrigen.Text;

            try
            {
                ValidarNombre(origen);
                MessageBox.Show("Origen validado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var descripcion = txtDescripcion.Text;

            var estado = true; // Valor por defecto

            try
            {
                // Validación de la marca: No puede ser nula, vacía ni contener caracteres no alfabéticos
                if (string.IsNullOrEmpty(marca) || !marca.All(c => Char.IsLetter(c)))
                {
                    MessageBox.Show("La marca no puede ser nula, vacía ni contener caracteres no alfabéticos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validación de la categoría: No puede ser nula ni vacía
                if (string.IsNullOrEmpty(categoria))
                {
                    MessageBox.Show("La categoría no puede ser nula ni vacía", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error durante la validación: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar el estado
            try
            {
                if (!string.IsNullOrWhiteSpace(txtEstado.Text))
                {
                    if (txtEstado.Text.ToLower() == "true")
                        estado = true;
                    else if (txtEstado.Text.ToLower() == "false")
                        estado = false;
                    else
                    {
                        MessageBox.Show("Valor de estado no válido. Se usará 'true' por defecto.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        estado = true;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Error procesando el estado: {err.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool Oferta = false; // Definir si la habitación tiene una oferta
            if (precio < precio_original) // Esto es solo un ejemplo de cómo identificar si hay una oferta
            {
                Oferta = true;
            }

            // Crear el objeto que será enviado como JSON
            var nuevoProducto = new
            {
                nombre,
                categoria,
                marca,
                origen,
                descripcion,
                precio,
                precio_original,
                tieneOferta = Oferta,
                estado,
                imagenBase64 = ImagenBase64
            };

            // Convertir el objeto a JSON
            var json = JsonConvert.SerializeObject(nuevoProducto);

            MessageBox.Show($"JSON generado:\n{json}", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Enviar la solicitud HTTP POST
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Instance.Token);
                    var response = await client.PostAsync("http://127.0.0.1:3505/Producto/productos", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Producto agregado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Limpiar campos del formulario
                        txtNom.Text = "";
                        txtCategoria.Text = "";
                        txtMarca.Text = "";
                        txtOrigen.Text = "";
                        txtDescripcion.Text = "";
                        txtPrecio.Text = "";
                        txtPrecioOriginal.Text = "";
                        txtEstado.Text = "";
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error en la agregación del producto. Código de estado: {response.StatusCode}\nMensaje: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al enviar la solicitud: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
            {
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            BuscadorProductos bHabitacion = new BuscadorProductos();
            bHabitacion.Show();
            this.Close();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Crear un cuadro de diálogo para seleccionar el archivo de la imagen
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Imagenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"; // Filtrar solo imágenes

            // Mostrar el cuadro de diálogo
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                // Obtener la ruta de la imagen seleccionada
                string imagePath = openFileDialog.FileName;

                try
                {
                    // Leer la imagen como un array de bytes
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

                    // Convertir los bytes a una cadena Base64
                    ImagenBase64 = Convert.ToBase64String(imageBytes);

                    // Aquí puedes mostrar un mensaje de confirmación
                    MessageBox.Show("Imagen seleccionada y convertida a Base64.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

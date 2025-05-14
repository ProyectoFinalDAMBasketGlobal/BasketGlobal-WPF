using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace app.View.Productos_BasketGlobal
{
    /// <summary>
    /// Lógica de interacción para ModificarProducto.xaml
    /// </summary>
    public partial class ModificarProducto : Window
    {
        // Propiedades públicas para devolver los valores editados
        public string NuevoNombre { get; private set; }
        public string NuevaCategoria { get; private set; }
        public string NuevaMarca { get; private set; }
        public int NuevaCapacidad { get; private set; }
        public string NuevaDescripcion { get; private set; }
        public string NuevoOrigen { get; private set; }
        public double NuevoPrecio { get; private set; }
        public double NuevoPrecioOriginal { get; private set; }
        public bool Estado { get; private set; }
        public bool tieneOferta { get; private set; }
        public string ImagenBase64 { get; private set; }
        
        public ModificarProducto(string nombre, string categoria, string marca, int capacidad, string descripcion, string origen, double precio, double precioOriginal, bool estado, string imagenBase64)
        {
            InitializeComponent();
            // Rellenar los controles con los valores actuales
            NomTextBox.Text = nombre;
            // Establecer la categoria correctamente
            CategoriaComboBox.SelectedItem = CategoriaComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item =>
                    item.Content.ToString() == categoria
                );
            // Establecer la marca correctamente
            MarcaTextBox.Text = marca;
            // Establecer la capacidad correctamente
            CapacidadComboBox.SelectedItem = CapacidadComboBox.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item =>
                    int.TryParse(item.Content.ToString(), out int itemCapacidad) && itemCapacidad == capacidad
                );
            // Establecer la descripcion del producto correctamente
            txtDescripcion.Text = descripcion;
            // Establecer el origen del producto correctamente
            txtOrigen.Text = origen;
            // Establecer el precio, precio original y estado correctamente
            PrecioTextBox.Text = precio.ToString("F2"); // Aseguramos formato consistente
            PrecioOriginalTextBox.Text = precioOriginal.ToString("F2"); // Guardamos el precio original
            EstadoCheckBox.IsChecked = estado; // Mostrar el estado actual como texto

            // Si existe una imagen, establecerla
            ImagenBase64 = imagenBase64;

        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NomTextBox.Text) || !Regex.IsMatch(NomTextBox.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$"))
                {
                    MessageBox.Show("Fallo inminente, por favor corrígelo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener los valores editados
                NuevoNombre = NomTextBox.Text;
                NuevaCategoria = (CategoriaComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                NuevaMarca = MarcaTextBox.Text;
                NuevaDescripcion = txtDescripcion.Text;
                NuevoOrigen = txtOrigen.Text;
                // Validar y convertir el precio
                if (!double.TryParse(PrecioTextBox.Text, out double nuevoPrecio) || !double.TryParse(PrecioOriginalTextBox.Text, out double nuevoPrecioOriginal))
                {
                    MessageBox.Show("El precio ingresado no es válido. Por favor, ingresa un número.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NuevoPrecio = nuevoPrecio;
                NuevoPrecioOriginal = nuevoPrecioOriginal;

                if (nuevoPrecio >= nuevoPrecioOriginal)
                {
                    tieneOferta = false;
                }
                else
                {
                    tieneOferta = true;
                }

                // Si la imagen no se ha seleccionado, mostrar advertencia
                if (string.IsNullOrEmpty(ImagenBase64))
                {
                    MessageBox.Show("Por favor, selecciona una imagen antes de actualizar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int capacidad;
                NuevaCapacidad = int.TryParse((CapacidadComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(), out capacidad) ? capacidad : 0;

                // Validar y convertir el estado de forma más robusta
                Estado = EstadoCheckBox.IsChecked ?? false;

                // Cerrar la ventana con éxito
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar los cambios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana sin hacer cambios
            DialogResult = false;
            Close();
        }
    }
}

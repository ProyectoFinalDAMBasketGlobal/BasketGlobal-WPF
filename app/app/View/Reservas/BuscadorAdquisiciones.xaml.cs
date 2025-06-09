using app.Models.Productos_BasketGlobal;
using app.Models.Adquisiciones_BasketGlobal;
using app.ViewModel.Productos_BasketGlobal;
using app.ViewModel.Adquisiciones_BasketGlobal;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace app.View.Reservas
{
    /// <summary>
    /// Lógica de interacción para BuscadorAdquisiciones.xaml
    /// </summary>
    public partial class BuscadorAdquisiciones : Window
    {
        private readonly AdquisicionesViewModel _viewModelA;
        private readonly ProductosViewModel _viewModelP;
        private bool isPrecioOriginalVisible = false;
        public ObservableCollection<Producto> AllProductos { get; set; }
        public ObservableCollection<Adquisicion> AllAdquisiciones { get; set; }
        public BuscadorAdquisiciones()
        {
            InitializeComponent();
            _viewModelA = new AdquisicionesViewModel();
            this.DataContext = _viewModelA;

            _viewModelP = new ProductosViewModel();
            this.DataContext = _viewModelP;

            _ = _viewModelP.CargarTodosProductos();
            _viewModelA.CargarTodasLasAdquisiciones();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si las colecciones están inicializadas
                if (_viewModelP.AllProductos == null || !_viewModelP.AllProductos.Any())
                {
                    MessageBox.Show("No hay productos disponibles para buscar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_viewModelA.AllAdquisiciones == null)
                {
                    MessageBox.Show("No se han cargado las adquisiciones.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener el valor seleccionado de comboBoxCategorias
                string categoria = ((ComboBoxItem)comboBoxCategorias.SelectedItem).Content.ToString();

                // Precio máximo
                double precioMax = sliderPrecio.Value;

                // Filtrar habitaciones disponibles
                var productosDisponibles = _viewModelP.AllProductos
                .Where(h =>
                    h.categoria == categoria && // Filtrar por número de huéspedes
                    h.precio <= precioMax
                )
                .ToList();

                // Mostrar resultados en la interfaz
                listResultados.ItemsSource = productosDisponibles;

                // Manejo si no se encontraron resultados
                if (!productosDisponibles.Any())
                {
                    MessageBox.Show("No se encontraron productos que coincidan con los criterios.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOfertas_Click(object sender, RoutedEventArgs e)
        {
            if (isPrecioOriginalVisible)
            {
                listResultados.Columns.First(c => c.Header.ToString() == "Precio Original").Visibility = Visibility.Collapsed;
                listResultados.Columns.First(c => c.Header.ToString() == "Precio").Visibility = Visibility.Visible;
            }
            else
            {
                listResultados.Columns.First(c => c.Header.ToString() == "Precio").Visibility = Visibility.Collapsed;
                listResultados.Columns.First(c => c.Header.ToString() == "Precio Original").Visibility = Visibility.Visible;
            }

            // Alternar la bandera
            isPrecioOriginalVisible = !isPrecioOriginalVisible;
        }

        private void sliderPrecio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtPrecio.Text = $"Máximo: {sliderPrecio.Value:F0}€";
        }

        private void btnAdquirir_Click(object sender, RoutedEventArgs e)
        {
            var selectedProd = listResultados.SelectedItem as Producto;

            if (selectedProd != null)
            {
                if (selectedProd.stock == 0)
                {
                    MessageBox.Show("Este producto no está disponible.");
                }
                else
                {
                    CrearAdquisiciones ventanaCrear = new CrearAdquisiciones(selectedProd);
                    ventanaCrear.Owner = this;
                    ventanaCrear.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainAdquisiciones ventanaMain = new MainAdquisiciones();
            ventanaMain.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModelA.CargarTodasLasAdquisiciones();
            _ = _viewModelP.CargarTodosProductos();
        }
    }
}

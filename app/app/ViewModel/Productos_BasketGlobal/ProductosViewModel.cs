using app.Models.Productos_BasketGlobal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;


namespace app.ViewModel.Productos_BasketGlobal
{
    internal class ProductosViewModel : INotifyPropertyChanged
    {
        private const string ApiUrlProductos = "http://127.0.0.1:3505/Producto/Obtener-Productos";
        private ObservableCollection<Producto> allProductos;
        public ObservableCollection<Producto> AllProductos
        {
            get => allProductos;
            set { allProductos = value; OnPropertyChanged("AllProductos"); }
        }

        private ObservableCollection<Producto> productosDisponibles;
        public ObservableCollection<Producto> ProductosDisponibles
        {
            get => productosDisponibles;
            set { productosDisponibles = value; OnPropertyChanged("ProductosDisponibles"); }
        }

        public ProductosViewModel()
        {
            AllProductos = new ObservableCollection<Producto>();
        }

        private readonly HttpClient httpClient = new HttpClient();

        // Método para buscar productos según los criterios
        public async Task<List<Producto>> BuscarProductos(string categoria, bool cinco, bool diez, bool veinte, bool otros, double precioMaximo, bool soloOfertas)
        {
            try
            {
                // Obtener todas los productos
                var productosResponse = await httpClient.GetAsync("http://127.0.0.1:3505/Producto/Obtener-Productos");

                if (productosResponse.IsSuccessStatusCode)
                {
                    var productosJson = await productosResponse.Content.ReadAsStringAsync();

                    // Deserializar los productos directamente como una lista
                    var productos = JsonConvert.DeserializeObject<List<Producto>>(productosJson);

                    // Validar que los productos se cargaron correctamente
                    if (productos == null)
                    {
                        MessageBox.Show("Error al cargar productos desde la API.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return new List<Producto>(); // Retornar una lista vacía en caso de error
                    }

                    // Filtrar productos según los criterios proporcionados
                    var productosFiltrados = productos
                        .Where(h => h.categoria == categoria) // Filtra por categoría
                        .Where(h =>
                            (cinco && h.stock == 5) ||
                            (diez && h.stock == 10) ||
                            (veinte && h.stock == 20) ||
                            (otros && h.stock != 5 && h.stock != 10 && h.stock != 20))
                        .Where(h => h.precio <= precioMaximo) // Filtra por precio
                        .Where(h => h.tieneOferta == soloOfertas) // Filtra por ofertas si 'soloOfertas' es true
                        .ToList();

                    // Mostrar cuántos productos quedan disponibles
                    MessageBox.Show($"Productos disponibles tras filtro: {productosFiltrados.Count}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);

                    return productosFiltrados;
                }
                else
                {
                    MessageBox.Show("Error al obtener productos. No se pudo conectar con la API.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<Producto>(); // Retornar una lista vacía en caso de error
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones y mostrar mensajes de error
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Producto>(); // Retornar una lista vacía en caso de error
            }
        }

        // Método async Task en lugar de async void
        public async Task CargarTodosProductos()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(ApiUrlProductos);
                    var productos = JsonConvert.DeserializeObject<List<Producto>>(response);

                    AllProductos = new ObservableCollection<Producto>(); // Actualiza la colección

                    foreach (var producto in productos)
                    {
                        if (producto.estado == true && !string.IsNullOrEmpty(producto._id))
                        {
                            AllProductos.Add(new Producto
                            {
                                _id = producto._id,
                                nombre = producto.nombre,
                                categoria = producto.categoria,
                                marca = producto.marca,
                                esImportado = producto.esImportado,
                                origen = producto.origen,
                                descripcion = producto.descripcion,
                                precio = producto.precio,
                                precio_original = producto.precio_original,
                                tieneOferta = producto.tieneOferta,
                                stock = producto.stock,
                                estado = producto.estado
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

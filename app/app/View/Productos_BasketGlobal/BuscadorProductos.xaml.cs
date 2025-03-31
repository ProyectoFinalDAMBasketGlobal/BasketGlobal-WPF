using app.Models.Productos_BasketGlobal;
using app.Models.Adquisiciones_BasketGlobal;
using app.View.Home;
using app.ViewModel.Productos_BasketGlobal;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace app.View.Productos_BasketGlobal
{
    /// <summary>
    /// Lógica de interacción para BuscadorProductos.xaml
    /// </summary>
    public partial class BuscadorProductos : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private List<Adquisicion> adquisiciones;
        public BuscadorProductos()
        {
            InitializeComponent();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            txtUsuarioRol.Text = SettingsData.Default.rol;
            txtUsuarioSession.Text = SettingsData.Default.nombre;

            // Cargar las adquisiciones de la API
            CargarAdquisiciones();
        }

        private async void CargarAdquisiciones()
        {
            try
            {
                var response = await httpClient.GetAsync("http://127.0.0.1:3505/Adquisicion/getAll");
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta usando ApiResponse<List<Adquisicion>>
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Adquisicion>>>(jsonContent);

                    // Accedemos a la lista de adquisiciones
                    adquisiciones = apiResponse.adquisiciones;

                    // Si quieres, puedes verificar la lista de adquisiciones aquí
                    foreach (var adquisicion in adquisiciones)
                    {
                        Console.WriteLine($"ID Producto: {adquisicion.id_prod}, Estado: {adquisicion.estado_adquisicion}");
                    }
                }
                else
                {
                    MessageBox.Show("Error al cargar las adquisiciones.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las adquisiciones: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Evento del botón "Añadir Producto"
        private void btn_AddHabitacion_Click(object sender, RoutedEventArgs e)
        {
            AñadirProducto employeeWindow = new AñadirProducto();
            employeeWindow.Show();
            Close();
        }

        // Evento del botón "Buscar"
        private async void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            // Recoger los valores del formulario
            string categoria = ((ComboBoxItem)comboBoxHuespedes.SelectedItem).Content.ToString();
            double precioMaximo = Convert.ToDouble(sliderPreu.Value);

            // Verificar si los valores son correctos
            if (string.IsNullOrEmpty(categoria))
            {
                MessageBox.Show("El campo de categoria está vacio.");
                return;
            }

            // Recoger los valores de los CheckBox (CamaExtra y Cuna)
            bool cinco = txtOp.IsChecked == true;
            bool diez = txtOp2.IsChecked == true;
            bool veinte = txtOp3.IsChecked == true;
            bool otros = txtOp4.IsChecked == true;

            var viewModel = new ProductosViewModel();


            // Buscar habitaciones usando la API
            var habitacionesEncontradas = await viewModel.BuscarProductos(categoria, cinco, diez, veinte, otros, precioMaximo, false);

            // Mostrar resultados
            MostrarResultados(habitacionesEncontradas);
        }


        private void MostrarResultados(List<Producto> productos)
        {
            // Cambiamos StackPanel por WrapPanel
            var wrapPanelResultados = new WrapPanel
            {
                Orientation = Orientation.Horizontal, // Los elementos fluyen horizontalmente
                HorizontalAlignment = HorizontalAlignment.Center
            };

            stackPanelResultados.Children.Clear();
            stackPanelResultados.Children.Add(wrapPanelResultados);

            if (productos.Count == 0)
            {
                var noResultados = new Label
                {
                    Content = "No se encontraron ningun producto con esos criterios.",
                    FontSize = 25,
                    Foreground = Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 100, 0, 0)
                };
                wrapPanelResultados.Children.Add(noResultados);
                return;
            }

            foreach (var producto in productos)
            {
                // Usamos un Border para permitir Padding
                var borderHabitacion = new Border
                {
                    Width = 200, // Definimos un ancho para que se ajusten mejor en el WrapPanel
                    Margin = new Thickness(10),
                    Background = Brushes.LightGray,
                    Padding = new Thickness(10), // Aquí sí es válido el Padding
                    CornerRadius = new CornerRadius(8), // Opcional: esquinas redondeadas
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1)
                };

                var stackPanelHabitacion = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                // Cargar la imagen de la habitación desde Base64 (si existe)
                Image imagen = new Image { Width = 100, Height = 100 };  // Aumenté un poco el tamaño para mejor visualización

                if (!string.IsNullOrEmpty(producto.imagenBase64))
                {
                    imagen.Source = ConvertBase64ToImage(producto.imagenBase64);
                }
                else
                {
                    imagen.Source = new BitmapImage(new Uri("pack://application:,,,/Image/habitacion.png", UriKind.Absolute));
                    // Imagen predeterminada
                }

                var nombre = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 20,
                    Margin = new Thickness(0, 10, 0, 5),
                    Content = producto.nombre
                };

                var precio = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center, Orientation = Orientation.Horizontal };

                if (producto.tieneOferta && producto.precio_original.HasValue)
                {
                    var precioTachado = new TextBlock
                    {
                        Text = $"{producto.precio_original}€",
                        TextDecorations = TextDecorations.Strikethrough,
                        Foreground = Brushes.Red,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                    precio.Children.Add(precioTachado);
                }

                var precioActual = new TextBlock
                {
                    FontSize = 15,
                    Foreground = Brushes.DarkGreen,
                    Text = $"{producto.precio} €"
                };
                precio.Children.Add(precioActual);

                // Estado de la habitación
                var estadoLabel = new Label
                {
                    Content = producto.estado ? "Estado: Activa" : "Estado: Baja",
                    FontSize = 14,
                    Foreground = producto.estado ? Brushes.Green : Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                // Crear un StackPanel para los botones y establecer la orientación horizontal
                var stackPanelBotones = new StackPanel
                {
                    Orientation = Orientation.Horizontal,  // Establecemos la orientación horizontal para los botones
                    HorizontalAlignment = HorizontalAlignment.Center,  // Alineamos los botones al centro (opcional)
                    VerticalAlignment = VerticalAlignment.Center  // Alineamos los botones al centro verticalmente (opcional)
                };

                // Botón para editar con símbolo y forma circular
                var botonEditar = new Button
                {
                    Content = "\u270E",  // Símbolo de lápiz (✎)
                    Height = 40,  // Ajusta el tamaño del botón
                    Width = 40,   // Ajusta el tamaño del botón para hacerlo circular
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush(Color.FromRgb(150, 200, 250)),
                    BorderBrush = Brushes.DarkBlue,
                    FontSize = 18,  // Aumentamos el tamaño para que sea visible
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    BorderThickness = new Thickness(2),
                    // Hacemos el borde completamente redondeado
                    Clip = new EllipseGeometry(new Point(20, 20), 20, 20)  // Clip en forma de círculo
                };
                botonEditar.Click += (sender, e) =>
                {
                    // Verificar si el producto tiene alguna adquisición pendiente antes de permitir la edición
                    if (adquisiciones.Any(a => a.id_prod == producto._id && a.estado_adquisicion == "Pendiente"))
                    {
                        MessageBox.Show("No puedes editar el producto porque tiene adquisiciones con estado 'Pendiente'.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Llamar a la función de edición solo si no hay adquisiciones pendientes
                    EditarHabitacion(producto);
                };

                // Botón para eliminar con símbolo y forma circular
                var botonEliminar = new Button
                {
                    Content = "🗑️",  // Símbolo de papelera (🗑️)
                    Height = 40,  // Ajusta el tamaño del botón
                    Width = 40,   // Ajusta el tamaño del botón para hacerlo circular
                    Margin = new Thickness(5),
                    Background = Brushes.Red,
                    Foreground = Brushes.White,
                    FontSize = 18,  // Aumentamos el tamaño para que sea visible
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    BorderThickness = new Thickness(2),
                    // Hacemos el borde completamente redondeado
                    Clip = new EllipseGeometry(new Point(20, 20), 20, 20)  // Clip en forma de círculo
                };
                botonEliminar.Click += (sender, e) =>
                {
                    // Verificar si el producto tiene alguna adquisición pendiente antes de permitir la eliminación
                    if (adquisiciones.Any(a => a.id_prod == producto._id && a.estado_adquisicion == "Pendiente"))
                    {
                        MessageBox.Show("No puedes eliminar el producto porque tiene adquisiciones con estado 'Pendiente'.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Llamar a la función de eliminación solo si no hay adquisiciones pendientes
                    EliminarHabitacion(producto);
                };

                // Añadir los botones al StackPanel
                stackPanelBotones.Children.Add(botonEditar);
                stackPanelBotones.Children.Add(botonEliminar);

                // Añadir los otros controles al StackPanel principal
                stackPanelHabitacion.Children.Add(imagen);
                stackPanelHabitacion.Children.Add(nombre);
                stackPanelHabitacion.Children.Add(precio);
                stackPanelHabitacion.Children.Add(estadoLabel);
                stackPanelHabitacion.Children.Add(stackPanelBotones);  // Agregar el StackPanel con los botones

                // Añadir el StackPanel al Border
                borderHabitacion.Child = stackPanelHabitacion;

                // Añadir el Border al WrapPanel
                wrapPanelResultados.Children.Add(borderHabitacion);
            }

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

        // Método para editar una habitación
        private async void EditarHabitacion(Producto producto)
        {
            double precio = (double)producto.precio;
            double precioOriginal = (double)producto.precio_original; // Asumiendo que tienes un campo para el precio original
            bool estado = producto.estado;

            // Crear y mostrar la ventana de edición
            var editarWindow = new ModificarProducto(
                producto.nombre,
                producto.categoria,
                producto.marca,
                producto.stock,
                producto.descripcion,
                producto.origen,
                precio,
                precioOriginal,
                estado,
                producto.imagenBase64
            );

            if (editarWindow.ShowDialog() == true)
            {
                // Si el usuario aceptó los cambios
                var nuevoNombre = editarWindow.NuevoNombre;
                var nuevaCategoria = editarWindow.NuevaCategoria;
                var nuevaMarca = editarWindow.NuevaMarca;
                var nuevoStock = editarWindow.NuevaCapacidad;
                var nuevaDescripcion = editarWindow.NuevaDescripcion;
                var nuevoOrigen = editarWindow.NuevoOrigen;
                var nuevoPrecio = editarWindow.NuevoPrecio;
                var nuevoPrecioOriginal = editarWindow.NuevoPrecioOriginal;
                bool tieneOferta = editarWindow.tieneOferta;
                bool nuevoEstado = editarWindow.Estado;
                var nuevaImagen = editarWindow.ImagenBase64;

                if (string.IsNullOrEmpty(producto._id))
                {
                    MessageBox.Show("El id del producto no es válido o está vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Salir del método si el _id no es válido
                }
                else
                {
                    MessageBox.Show($"Id del producto: {producto._id}", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Crear objeto con los datos actualizados
                producto.nombre = nuevoNombre;
                producto.categoria = nuevaCategoria;
                producto.marca = nuevaMarca;
                producto.stock = nuevoStock;
                producto.descripcion = nuevaDescripcion;
                producto.origen = nuevoOrigen;
                producto.tieneOferta = tieneOferta;
                producto.imagenBase64 = nuevaImagen;

                // Actualizar precio y estado en MongoDB
                var connectionString = "mongodb+srv://jlorentediaz9:kaX56fOm9vwSkVke@cluster0.u8b7w.mongodb.net/BasketGlobal"; // Cambia la URL si es necesario
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("BasketGlobal"); // Cambia el nombre de la base de datos
                var collection = database.GetCollection<BsonDocument>("Productos");

                var filtro = Builders<BsonDocument>.Filter.Eq("_id", producto._id);
                var actualizacionMongo = Builders<BsonDocument>.Update
                    .Set("precio", nuevoPrecio)
                    .Set("precio_original", nuevoPrecioOriginal)
                    .Set("estado", nuevoEstado);

                try
                {
                    var resultadoMongo = await collection.UpdateOneAsync(filtro, actualizacionMongo);
                    if (resultadoMongo.ModifiedCount > 0)
                    {
                        MessageBox.Show("Precios y estado actualizados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el precio y estado en MongoDB.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar en MongoDB: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Realizar la actualización del resto de los datos en la API
                var jsonContent = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"http://127.0.0.1:3505/Producto/Actualizar-Producto/{producto._id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("El resto de los datos del producto se actualizaron correctamente en la API.");
                    // Aquí puedes recargar la lista de habitaciones si es necesario
                    Button_Click(null, null);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al actualizar los datos en la API: {errorContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private async void EliminarHabitacion(Producto producto)
        {

            if (producto == null)
            {
                MessageBox.Show("No se ha seleccionado ningun producto para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (producto.estado == true)
            {
                MessageBox.Show("No puedes eliminar el producto del sistema", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Confirmar la eliminación
                var confirmacion = MessageBox.Show($"¿Estás seguro de que deseas eliminar el producto con ID: {producto._id}?", "Eliminar Producto", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmacion != MessageBoxResult.Yes) return;

                // Enviar la solicitud DELETE a la API para eliminar la habitación
                var response = await httpClient.DeleteAsync($"http://127.0.0.1:3505/Producto/Eliminar-Producto/{producto._id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Producto con ID: {producto._id} eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Actualizar la lista de habitaciones
                    btn_Buscar_Click(null, null); // Asegúrate de que este método recargue la lista correctamente
                }
                else
                {
                    MessageBox.Show($"Error al eliminar el producto: {producto._id}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la habitación: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Evento del botón "Ofertas"
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            string categoria = ((ComboBoxItem)comboBoxHuespedes.SelectedItem).Content.ToString();
            double precioMaximo = Convert.ToDouble(sliderPreu.Value);

            if (string.IsNullOrEmpty(categoria))
            {
                MessageBox.Show("El campo de categoria está vacio.");
                return;
            }

            // Establecemos soloOfertas en true para filtrar solo habitaciones con ofertas
            bool cinco = txtOp.IsChecked == true;
            bool diez = txtOp2.IsChecked == true;
            bool veinte = txtOp3.IsChecked == true;
            bool otros = txtOp4.IsChecked == true;

            var viewModel = new ProductosViewModel();

            // Buscar habitaciones solo con ofertas
            var habitacionesEncontradas = await viewModel.BuscarProductos(categoria, cinco, diez, veinte, otros, precioMaximo, true);

            // Mostrar resultados
            MostrarResultados(habitacionesEncontradas);
        }

        private void btn_Volver_Click(object sender, RoutedEventArgs e)
        {
            Inicio user = new Inicio();
            user.Show();
            this.Close();
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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
using app.ViewModel.Usuarios;

namespace app.View.Usuarios.InformacionUsuarios
{
    /// <summary>
    /// Lógica de interacción para InformacionUsuario.xaml
    /// </summary>
    public partial class InformacionUsuario : Window
    {
        private readonly UsuarioViewModel _viewModel;
        private readonly string ID;
        public InformacionUsuario(string id,UsuarioViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            ID = id;
        }



        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UsuarioBase infoPerfil = _viewModel.AllPerfiles.FirstOrDefault(item => item._id == ID);
            Usuario infoUsuario = _viewModel.AllUsers.FirstOrDefault(item => item._id == infoPerfil.idUsuario);
            
            txtNombre.Text = infoPerfil.nombre;
            txtApellido.Text = infoPerfil.apellido;
            txtCiudad.Text = infoPerfil.ciudad;
            txtRol.Text = infoPerfil.rol;
            txtDni.Text = infoPerfil.dni;
            txtAlta.Text = infoPerfil.registro;
            txtBaja.Text = infoPerfil.baja;
            txtEmail.Text = infoUsuario.email;
            string imageUrl = infoPerfil.rutaFoto; // Ruta de la API
            BitmapImage bitmap = new BitmapImage();

            try
            {
                // Cargar la imagen desde la URL
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // Asegura que la imagen se carga completamente
                bitmap.EndInit();

                // Crear un ImageBrush y asignarlo al Ellipse
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmap;
                imageBrush.Stretch = Stretch.UniformToFill; // Asegura que la imagen llena el Ellipse correctamente
                imgPerfil.Fill = imageBrush;
                //Guardo la imagen en una variable para despues usarla
               
        
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using app.Models.Usuarios;
using app.ViewModel.Usuarios;

namespace app.View.Usuarios.EditarUsuarios
{
    /// <summary>
    /// Lógica de interacción para EditarUsuario.xaml
    /// </summary>
    public partial class EditarUsuario : Window
    {
        private readonly UsuarioViewModel _viewModel;
        UsuarioBase usuarioEdita;
        Usuario usuarioDeEdita;
        private readonly string ID;
        private readonly string ROL;
        private byte[] imagenCargadaBytes;
        private MultipartFormDataContent formContent = new MultipartFormDataContent();
        bool isNombreValid = false;
        bool isApellidoValid =false;
        bool isEmailValid = false;
        bool isCiudadValid = false;
        bool isDniValid = false;
        bool isFechaValid = false;
        bool isRolValid = false;
        bool isImgChange = false;

        bool rutaImagen = false;
        bool rutaEmail = false;
        bool rutaRol = false;
        public EditarUsuario(string id,string rol,UsuarioViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            ID = id;
            ROL = rol;
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            usuarioEdita = _viewModel.AllPerfiles.FirstOrDefault(item => item._id == ID);
            usuarioDeEdita = _viewModel.AllUsers.FirstOrDefault(item => item._id == usuarioEdita.idUsuario);

            txtNombre.Text = usuarioEdita.nombre;
            txtApellidos.Text = usuarioEdita.apellido;
            txtRol.SelectedIndex = usuarioEdita.rol == "Administrador" ? 0 : usuarioEdita.rol == "Empleado" ? 1 : usuarioEdita.rol == "Cliente" ? 2: -1;    
            Usuario usuarioDelPerfil = _viewModel.AllUsers.First(item => item._id == usuarioEdita.idUsuario);
            txtEmail.Text = usuarioDelPerfil.email;
            txtDni.Text = usuarioEdita.dni;
            if (DateTime.TryParseExact(usuarioEdita.date, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, 
                out DateTime parsedDate))
            {
                txtDate.SelectedDate = parsedDate;
            }
            txtCiudad.Text = usuarioEdita.ciudad;
            rbH.IsChecked = usuarioEdita.sexo == "Hombre" ? true : false;
            rbM.IsChecked = usuarioEdita.sexo == "Mujer" ? true : false;
            rb49.IsChecked = usuarioEdita.sexo == "Indeterminado" ? true : false;
            string imageUrl = usuarioEdita.rutaFoto; // Ruta de la API
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
                miEllipse.Fill = imageBrush;
                //Guardo la imagen en una variable para despues usarla
                // Guardar la imagen en la variable imagenCargadaBytes
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    imagenCargadaBytes = stream.ToArray(); // Convertir el stream en un arreglo de bytes
                }
                ValidateForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();

        }

        private void txtApellidos_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();

        }

        private void txtRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateForm();

            
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
            PosibleCambioDeEmail();  
        }

        public async void PosibleCambioDeEmail() {
             
            bool succes = await _viewModel.EmailDisponible(txtEmail.Text);
            if(succes )
                ErrorTextEmailChange.Visibility= Visibility.Collapsed;
            else 
                if(txtEmail.Text == usuarioDeEdita.email)
                    ErrorTextEmailChange.Visibility = Visibility.Collapsed;
                else
                    ErrorTextEmailChange.Visibility =Visibility.Visible;

            ValidateForm();
        }

        private void txtDni_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();

        }

        private void txtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateForm();

        }

        private void txtCiudad_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();

        }

        private void btnCargarImg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png"; // Filtro de extensiones para el cuadro de diálogo
            dlg.Filter = "Archivos de imagen (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    // Obtener la ruta del archivo seleccionado por el usuario
                    string filePath = dlg.FileName;

                    // Leer los datos del archivo
                    imagenCargadaBytes = File.ReadAllBytes(filePath);
                    




                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(dlg.FileName);
                    bitmapImage.EndInit();
                    miEllipse.Fill = new ImageBrush(bitmapImage);
                    isImgChange = true;
                    ValidateForm();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }

        private void ValidateForm()
        {
            // Obtengo los valores ingresados en los campos
            string nombre = txtNombre.Text;
            string apellido = txtApellidos.Text;
            string email = txtEmail.Text;
            string ciudad = txtCiudad.Text;
            string dni = txtDni.Text;
            DateTime? fechaSeleccionada = txtDate.SelectedDate;
            bool isRolSelected = txtRol.SelectedItem != null;
            bool isFotoCargada = imagenCargadaBytes != null;

            // Valido
            isNombreValid = IsValidField(nombre);
            isApellidoValid = IsValidField(apellido);
            isEmailValid = IsValidEmail(email);
            isCiudadValid = IsValidField(ciudad);
            isDniValid = IsValidDniNie(dni);
            isFechaValid = fechaSeleccionada.HasValue;
            isRolValid = isRolSelected;

            // Muestro u oculto el mensaje de error para el campos dependiendo de su validez
            ErrorTextNombre.Visibility = isNombreValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextApellidos.Visibility = isApellidoValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextEmail.Visibility = isEmailValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextCiudad.Visibility = isCiudadValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextDate.Visibility = isFechaValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextRol.Visibility = isRolValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextDni.Visibility = isDniValid ? Visibility.Collapsed : Visibility.Visible;
            ErrorTextFoto.Visibility = isFotoCargada ? Visibility.Collapsed : Visibility.Visible;

            

            // Habilito el botón de inicio de sesión solo si ambos campos son válidos
            btnEditarPerfil.IsEnabled = isNombreValid && isApellidoValid && isEmailValid && isCiudadValid && isDniValid && isFechaValid && isRolValid && isFotoCargada && ErrorTextEmailChange.Visibility  == Visibility.Collapsed;

        }

        public bool IsValidField(string field)
        {

            //Si no es nula regreso false
            if (string.IsNullOrWhiteSpace(field))
                return false;

            //Si al menos tiene 3 letras el campo es valido
            return field.Length > 2 ? true : false;
        }

        public bool IsValidEmail(string email)
        {

            //Si no es nula regreso false
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar un email, se agrega using System.Text.RegularExpressions;
            //Explicación del patter al final del documento.
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsValidDniNie(string input)
        {
            // Patrón para DNI: 8 dígitos seguidos de una letra
            string patronDni = @"^\d{8}[A-Za-z]$";

            // Patrón para NIE: Letra inicial (X, Y, Z), 7 dígitos y una letra
            string patronNie = @"^[XYZ]\d{7}[A-Za-z]$";

            // Verificar si cumple con el formato de DNI o NIE
            if (Regex.IsMatch(input, patronDni) || Regex.IsMatch(input, patronNie))
            {
                // Validar la letra del DNI o NIE
                return ValidarLetraDniNie(input);
            }
            return false;
        }

        private static bool ValidarLetraDniNie(string input)
        {
            // Extraer números del DNI o NIE
            string numeros;
            if (input[0] == 'X')
                numeros = "0" + input.Substring(1, 7); // Reemplazar X por 0
            else if (input[0] == 'Y')
                numeros = "1" + input.Substring(1, 7); // Reemplazar Y por 1
            else if (input[0] == 'Z')
                numeros = "2" + input.Substring(1, 7); // Reemplazar Z por 2
            else
                numeros = input.Substring(0, 8); // Es un DNI

            // Calcular letra esperada
            string letras = "TRWAGMYFPDXBNJZSQVHLCKE";
            int indice = int.Parse(numeros) % 23;
            char letraEsperada = letras[indice];

            // Comparar con la letra del DNI o NIE
            return char.ToUpper(input.Substring(input.Length - 1, 1)[0]) == letraEsperada;

        }

        private async void btnEditarPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (isNombreValid && txtNombre.Text.Trim() != usuarioEdita.nombre)
                formContent.Add(new StringContent(txtNombre.Text.Trim()), "nombre");

            if (isApellidoValid && txtApellidos.Text.Trim() != usuarioEdita.apellido)
                formContent.Add(new StringContent(txtApellidos.Text.Trim()), "apellido");

            ComboBoxItem itemSeleccionado = (ComboBoxItem)txtRol.SelectedItem;
            string contenidoSeleccionado = itemSeleccionado.Content.ToString();
            if (isRolValid && contenidoSeleccionado != usuarioEdita.rol)
            {
                formContent.Add(new StringContent(contenidoSeleccionado), "rol");
                rutaRol = true;

            }
            if (isEmailValid && txtEmail.Text.Trim() != usuarioDeEdita.email) { 
                formContent.Add(new StringContent(txtEmail.Text.Trim()), "email");
                rutaEmail = true;
            }

            if (isDniValid && txtDni.Text.Trim() != usuarioEdita.dni)
                formContent.Add(new StringContent(txtDni.Text.Trim()), "dni");

            if (isFechaValid && txtDate.Text.Trim() != usuarioEdita.date)
                formContent.Add(new StringContent(txtDate.Text.Trim()), "date");

            if (isCiudadValid && txtCiudad.Text.Trim() != usuarioEdita.ciudad)
                formContent.Add(new StringContent(txtCiudad.Text.Trim()), "ciudad");

            string nuevoSexo = rbH.IsChecked == true ? "Hombre" :
                               rbM.IsChecked == true ? "Mujer" :
                               rb49.IsChecked == true ? "Indeterminado" : null;

            if (!string.IsNullOrEmpty(nuevoSexo) && nuevoSexo != usuarioEdita.sexo)
                formContent.Add(new StringContent(nuevoSexo), "sexo");
            

            if (isImgChange)
            {
                // Convertir los datos de la imagen a ByteArrayContent
                ByteArrayContent imagenContent = new ByteArrayContent(imagenCargadaBytes);
                // Agregar la imagen al contenido
                formContent.Add(imagenContent, "picture", "imagen.jpg");
                rutaImagen = true;
            }

            if (formContent.Count() == 0)
            {
                MessageBox.Show("No has cambiado nada.", "Ojo...", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Caso 1: No cambia nada
            if (!rutaRol && !rutaEmail)
            {
                //No se realizan cambios sensibles put a perfil
                try
                {
                    HttpResponseMessage respuesta = await _viewModel.EditarPerfil(ID, ROL, formContent);
                    var respuestaContenido = await respuesta.Content.ReadAsStringAsync();

                    if (respuesta.IsSuccessStatusCode )
                    {
                       
                        MessageBox.Show($"Perfil Editado!", "WPF... ", MessageBoxButton.OK, MessageBoxImage.Information);
  
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error result no success: {respuestaContenido}", "WPF... ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                catch (Exception ePerfil) { MessageBox.Show($"Error Exeption : {ePerfil.Message}", "WPF... ", MessageBoxButton.OK, MessageBoxImage.Error); }
            }

            // Caso 2: Solo se cambia el rol
            else if (isRolValid && !isEmailValid)
            {
                // Eliminar el perfil actual y crear uno nuevo con el nuevo rol
            }

            // Caso 3: Solo se cambia el email
            else if (!isRolValid && isEmailValid)
            {
                // Actualizar el email del usuario
            }


            // Caso 4: Se cambia el rol y el email 
            else if (isRolValid && isEmailValid)
            {
                // Eliminar el perfil actual, crear uno nuevo, actualizar el email 
            }
            else {
                MessageBox.Show("Cabo suelto");
            }


            //try
            //{
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var part in formContent)
            //    {
            //        var contentDisposition = part.Headers.ContentDisposition;

            //        // Obtener el nombre del campo
            //        string name = contentDisposition.Name.Trim('"');

            //        if (contentDisposition.FileName == null) // Procesar campos normales (texto)
            //        {
            //            string value = await part.ReadAsStringAsync();
            //            sb.AppendLine($"{name}: {value}");
            //        }
            //        else // Procesar archivos (imagen)
            //        {
            //            string fileName = contentDisposition.FileName.Trim('"');
            //            byte[] fileBytes = await part.ReadAsByteArrayAsync();

            //            // Guardar la imagen temporalmente
            //            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);
            //            // object value = await File.WriteAllBytes(tempPath, fileBytes); // Escribir el archivo de forma asíncrona

            //            sb.AppendLine($"{name} (Imagen): {fileName}");
            //            sb.AppendLine($"Ruta temporal: {tempPath}");
            //        }
            //    }

            //    // Mostrar el contenido en un MessageBox
            //    MessageBox.Show(sb.ToString(), "Contenido del MultipartFormDataContent", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}




        }
    }
}

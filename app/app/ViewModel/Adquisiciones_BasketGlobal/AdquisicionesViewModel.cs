using app.Models.Adquisiciones_BasketGlobal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace app.ViewModel.Adquisiciones_BasketGlobal
{
    public class AdquisicionesViewModel : INotifyPropertyChanged
    {
        private const string ApiUrlAllAdquisiciones = "http://127.0.0.1:3505/Adquisicion/getAll";
        private const string ApiUrlEliminarAdquisiciones = "http://127.0.0.1:3505/Adquisicion/eliminarAdquisicion";
        private const string ApiUrlModificarAdquisiciones = "http://127.0.0.1:3505/Adquisicion/modificarAdquisicion";

        private ObservableCollection<Adquisicion> allAdquisiciones;
        public ObservableCollection<Adquisicion> AllAdquisiciones
        {
            get => allAdquisiciones;
            set { allAdquisiciones = value; OnPropertyChanged("AllAdquisiciones"); }
        }

        public AdquisicionesViewModel()
        {
            AllAdquisiciones = new ObservableCollection<Adquisicion>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void CargarTodasLasAdquisiciones()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responseAdquisiciones = await client.GetAsync(ApiUrlAllAdquisiciones);

                    if (responseAdquisiciones.IsSuccessStatusCode)
                    {
                        var jsonAdquisiciones = await responseAdquisiciones.Content.ReadAsStringAsync();
                        var adquisicionesResponse = JsonConvert.DeserializeObject<ApiResponse<List<Adquisicion>>>(jsonAdquisiciones);

                        if (adquisicionesResponse.adquisiciones == null || adquisicionesResponse.adquisiciones.Count == 0)
                        {
                            MessageBox.Show("No se encontraron ninguna adquisicion.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        AllAdquisiciones = new ObservableCollection<Adquisicion>();

                        // Filtra solo las adquisiciones que pertenecen al usuario actual
                        string idUsuarioActual = SettingsData.Default.idPerfil;
                        bool tieneAdquisiciones = false;

                        foreach (var reserva in adquisicionesResponse.adquisiciones)
                        {
                            if (!string.IsNullOrEmpty(reserva._id) && reserva.id_usu == idUsuarioActual)
                            {
                                AllAdquisiciones.Add(new Adquisicion
                                {
                                    _id = reserva._id,
                                    id_usu = reserva.id_usu,
                                    id_prod = reserva.id_prod,
                                    fecha_adquisicion = reserva.fecha_adquisicion,
                                    cantidad = reserva.cantidad,
                                    estado_adquisicion = reserva.estado_adquisicion
                                });
                                tieneAdquisiciones = true;
                            }
                        }

                        // Si no hay adquisiciones del usuario, mostramos un mensaje
                        if (!tieneAdquisiciones)
                        {
                            MessageBox.Show("Este usuario no ha realizado ninguna adquisición hasta el momento.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        OnPropertyChanged("AllAdquisiciones");
                    }
                    else if (responseAdquisiciones.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("No se encontraron adquisiciones en la base de datos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Error al obtener reservas: {responseAdquisiciones.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error inesperado: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        //using (var client = new HttpClient())
        //{
        //    //try
        //    //{
        //    //    var response = await client.GetStringAsync(ApiUrlHabitaciones);

        //    //    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Habitacion>>>(response);

        //    //    var habitaciones = apiResponse.habitaciones;

        //    //    //if (responseHabitaciones.IsSuccessStatusCode)
        //    //    //{
        //    //    //    var jsonHabitaciones = await responseHabitaciones.Content.ReadAsStringAsync();
        //    //    //    var habitacionResponse = JsonConvert.DeserializeObject<ApiResponse<List<Habitacion>>>(jsonHabitaciones);

        //    //    //    AllHabitaciones = new ObservableCollection<Habitacion>();

        //    //    //    foreach (var habitacion in habitacionResponse.habitaciones)
        //    //    //    {
        //    //    //        AllHabitaciones.Add(new Habitacion
        //    //    //        {
        //    //    //            _id = habitacion._id,
        //    //    //            num_planta = habitacion.num_planta,
        //    //    //            tipo = habitacion.tipo,
        //    //    //            capacidad = habitacion.capacidad,
        //    //    //            descripcion = habitacion.descripcion,
        //    //    //            opciones = new Opciones
        //    //    //            {
        //    //    //                CamaExtra = habitacion.opciones.CamaExtra,
        //    //    //                Cuna = habitacion.opciones.Cuna
        //    //    //            },
        //    //    //            precio_noche = habitacion.precio_noche,
        //    //    //            precio_noche_original = habitacion.precio_noche_original,
        //    //    //            tieneOferta = habitacion.tieneOferta,
        //    //    //            estado = habitacion.estado
        //    //    //        });
        //    //    //    }

        //    //    //    OnPropertyChanged("AllHabitaciones");
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    throw new Exception($"Error al obtener habitaciones: {responseHabitaciones.StatusCode}");
        //    //    //}
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    MessageBox.Show($"Error: {e.Message}");
        //    //}
        //}

        public async Task<HttpResponseMessage> EditarAdquisicion(string id, Adquisicion adquisicion)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = JsonConvert.SerializeObject(adquisicion);
                    var json = new StringContent(content, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{ApiUrlModificarAdquisiciones}/{id}", json);
                    return response;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e.Message}");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message };
                }
            }
        }

        public async Task<HttpResponseMessage> EliminarAdquisicion(string id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.DeleteAsync($"{ApiUrlEliminarAdquisiciones}/{id}");
                    return response;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e.Message}");
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError) { ReasonPhrase = e.Message };
                }
            }
        }
    }
}
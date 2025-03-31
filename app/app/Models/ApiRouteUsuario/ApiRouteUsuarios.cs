using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models.ApiRouteUsuario
{
    public static class ApiRouteUsuarios
    {
        private static readonly string BaseUrl = "http://127.0.0.1:3505";

        public static class Administrador
        {
            public static readonly string GetAll = $"{BaseUrl}/Administrador/getAllAdministradores";
            public static readonly string Eliminar = $"{BaseUrl}/Administrador/eliminarAdministrador";
            public static readonly string Editar = $"{BaseUrl}/Administrador/editarAdministrador";
            public static readonly string Buscar = $"{BaseUrl}/Administrador/buscarAdministrador";
        }

        public static class Empleado
        {
            public static readonly string GetAll = $"{BaseUrl}/Empleado/getAllEmpleados";
            public static readonly string Eliminar = $"{BaseUrl}/Empleado/eliminarEmpleado";
            public static readonly string Editar = $"{BaseUrl}/Empleado/editarEmpleado";
            public static readonly string Buscar = $"{BaseUrl}/Empleado/buscarEmpleado";
        }

        public static class Cliente
        {
            public static readonly string GetAll = $"{BaseUrl}/Cliente/getAllClientes";
            public static readonly string Eliminar = $"{BaseUrl}/Cliente/eliminarCliente";
            public static readonly string Editar = $"{BaseUrl}/Cliente/editarCliente";
            public static readonly string Buscar = $"{BaseUrl}/Cliente/buscarCliente";
        }

        public static class Usuario
        {
            public static readonly string TodosLosUsuarios = $"{BaseUrl}/Usuario/todosLosUsuarios";
            public static readonly string EmailDisponible = $"{BaseUrl}/Usuario/emailDisponible";
            public static readonly string LogIn = $"{BaseUrl}/Usuario/logIn";
            public static readonly string AccessToken = $"{BaseUrl}/Usuario/accessToken";
            public static readonly string Registro = $"{BaseUrl}/Usuario/registroUsuario";
            public static readonly string Verificar = $"{BaseUrl}/Usuario/verificarUsuario";
            public static readonly string RecuperarPassword = $"{BaseUrl}/Usuario/recuperarPassword";
            public static readonly string CambiarPassword = $"{BaseUrl}/Usuario/cambiarPassword";
        }
    }
}

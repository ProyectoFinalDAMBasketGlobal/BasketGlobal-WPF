using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using app.Models.Usuarios;

namespace app.Models.IUsuariosRepository
{
    public interface IUsuarioRepository
    {
        //El principio de inversion de dependencias estable que los detalles deben depender de las abstracciones, en terminos de arquitectura, acceso a datos debe depender de dominio pero no al revés, por ello se define esta interfaz abstracta del repositorio en el modelo y la capa de acceso sera quien implemente el detalle. Esto permite el acoplamiento flexoble entre el modelo al acceso a datos y el modelo no depende del acceso a datos. (Cuestion: los modelos pertenecen a la capa dominio y los repositorios a la capa de acceso a datos)
        Task<dynamic> AuthenticateUser(NetworkCredential credential);
        void Add(Usuario usuario);
        void Update(string id, string rol, MultipartFormDataContent usuarioEditar);
        Task<bool> Delete(string id, string role);
        bool Access(string token);

        IEnumerable<Usuario> GetAll();

    }
}

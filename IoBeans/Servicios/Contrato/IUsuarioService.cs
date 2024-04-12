using IoBeans.Models;

namespace IoBeans.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Login> GetUsuario(string usuario, string clave);
        Task<Login> SaveUsuario(Login modelo);
    }
}

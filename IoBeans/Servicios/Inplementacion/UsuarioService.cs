using IoBeans.Models;
using IoBeans.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;

namespace IoBeans.Servicios.Inplementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IoBeansContext _dbContext;
        public UsuarioService(IoBeansContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Login> GetUsuario(string usuario, string clave)
        {
            Login usuario_encontrado = await _dbContext.Logins.Where(u => u.Username == usuario && u.Password == clave).FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Login> SaveUsuario(Login modelo)
        {
            _dbContext.Logins.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}

using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using PROY.ViewModels;
using System.Diagnostics;

namespace PROY.Repositories
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly AppDBContext _dbContext;

        public UsuarioRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Rol>> GetAllRoles()
        {
            List<Rol> listado = await _dbContext.Roles.Where(u => u.Restrictiva == 0).ToListAsync();

            return listado;
        }

        public async Task<List<Catalogo>> GetAllDeptos()
        {
            List<Catalogo> listado = await _dbContext.Catalogos.Where(u => u.Restrictiva == 0 && u.Tipo=="UNIDAD").ToListAsync();

            return listado;
        }


        public async Task<List<Permiso>> GetAllPermisos()
        {
            List<Permiso> listado= await _dbContext.Permisos.Where(p=>p.Restrictiva==0).ToListAsync();

            return listado;
        }

        /*public async Task<List<PermisoRolVM>> GestionarPermisos()
        {
            var permisos = await _dbContext.Permisos.ToListAsync();
            var roles = await _dbContext.Roles.ToListAsync();

            var model = permisos.Select(p => new PermisoRolVM
            {
                IdPermiso = p.IdPermiso,
                NombrePermiso = p.Nombre,
                RolesPermisos = roles.Select(r => new RolPermisoVM
                {
                    IdRol = r.IdRol,
                    NombreRol = r.Nombre,
                    TienePermiso = _dbContext.RolesPermisos.Any(rp => rp.PermisoId == p.IdPermiso && rp.RolId == r.IdRol)
                }).ToList()
            }).ToList();

            return model;
        }*/

        //-----------------------------------------------------------------
        public async Task<List<Permiso>> ObtenerPermisos()
        {
            var listado= await _dbContext.Permisos.ToListAsync();
            return listado;
        }

        public async Task<List<Rol>> ObtenerRoles()
        {
            return  await _dbContext.Roles.ToListAsync();
        }

        public bool TienePermiso(Int64 idPermiso, Int64 idRol)
        {
            return  _dbContext.RolesPermisos.Any(rp => rp.PermisoId == idPermiso && rp.RolId == idRol);
        }
        /*public async Task<bool> TienePermiso(Int64 idPermiso, Int64 idRol)
        {
            return await _dbContext.RolesPermisos.AnyAsync(rp => rp.PermisoId == idPermiso && rp.RolId == idRol);
        }*/
        
        /*public void AgregarRolPermiso(Int64 idPermiso, Int64 idRol)
        {
            _dbContext.RolesPermisos.Add(new RolPermiso { PermisoId = idPermiso, RolId = idRol });
        }*/
        public async Task AgregarRolPermiso(Int64 idPermiso, Int64 idRol)
        {
            await _dbContext.RolesPermisos.AddAsync(new RolPermiso { PermisoId = idPermiso, RolId = idRol });
        }
        /*public void EliminarRolPermiso(Int64 idPermiso, Int64 idRol)
        {
            var permisoRol = _dbContext.RolesPermisos
                .FirstOrDefault(rp => rp.PermisoId == idPermiso && rp.RolId == idRol);

            if (permisoRol != null)
            {
                _dbContext.RolesPermisos.Remove(permisoRol);
            }
        }*/

        public async Task EliminarRolPermiso(Int64 idPermiso, Int64 idRol)
        {
            try
            {
                var permisoRol =  _dbContext.RolesPermisos
                    .Where(rp => rp.PermisoId == idPermiso && rp.RolId == idRol).FirstOrDefault();
                //.FirstOrDefaultAsync(rp => rp.PermisoId == idPermiso && rp.RolId == idRol);

                if (permisoRol != null)
                {
                    _dbContext.RolesPermisos.Remove(permisoRol);
                    /*var sql = "DELETE FROM RolPermiso WHERE IdPermiso = @p0 AND IdRol = @p1";
                    await _dbContext.Database.ExecuteSqlRawAsync(sql, idPermiso, idRol);*/
                    //await _dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error al eliminar Permiso-Rol: {ex.Message}");
                throw; // Lanza la excepción nuevamente para permitir su manejo en capas superiores si es necesario.
            }
            
        }
        /*public void GuardarCambios()
        {
            _dbContext.SaveChanges();
        }*/

        public async Task GuardarCambios()
        {
            await _dbContext.SaveChangesAsync();
        }
        //-----------------------------------------------------------------
        public async Task<List<Usuario>> GetAll()
        {
            List<Usuario> listado = await _dbContext.Usuarios.Where(u=>u.Restrictiva==0).ToListAsync();    
            
            return listado;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave && u.Restrictiva == 0).Include("Rol").Where(r => r.Restrictiva==0).FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> GetUsuario(Int64 id)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios.FirstAsync(u => u.IdUsuario == id && u.Restrictiva == 0);

            return usuario_encontrado;
        }

        public async Task<Int64> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                /*maxID = _dbContext.Usuarios
                .FromSqlRaw("SELECT NVL(MAX(IdUsuario), 0) AS IdUsuario FROM Usuario")
                .Select(u => u.IdUsuario)
                .FirstOrDefault();*/

                maxID = await _dbContext.Usuarios
                .Select(u => u.IdUsuario)                
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }
            
            return maxID;
        }
        

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            await _dbContext.Usuarios.AddAsync(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Usuario> UpdateUsuario(Usuario modelo)
        {
            _dbContext.Usuarios.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        
    }
}

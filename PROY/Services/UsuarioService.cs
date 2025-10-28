using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;

using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Helpers;
using PROY.Models;
using PROY.Repositories;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
//using Web.Extensions;

namespace PROY.Services
{
    public class UsuarioService : IUsuarioService
    {
        //private readonly AppDBContext _dbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            //_dbContext = dbContext;
            _usuarioRepository = usuarioRepository;
        }

        /*public ServiceResponse<string> DoSomething(int input)
        {
            var response = new ServiceResponse<string>();

            if (input > 0)
            {
                response.Data = "Operación exitosa";
                response.Message = "La operación se realizó correctamente.";
            }
            else
            {
                response.Success = false;
                response.Message = "El valor de entrada debe ser mayor que 0.";
            }

            return response;
        }*/

        public async Task<List<Rol>> GetAllRoles()
        {
            List<Rol> listado = await _usuarioRepository.GetAllRoles();

            return listado;
        }

        public async Task<List<Catalogo>> GetAllDeptos()
        {
            List<Catalogo> listado = await _usuarioRepository.GetAllDeptos();

            return listado;
        }

        public async Task<List<Permiso>> GetAllPermisos()
        {
            List<Permiso> listado= await _usuarioRepository.GetAllPermisos();

            return listado;
        }

        public async Task<List<PermisoRolVM>> ObtenerPermisosConRoles()
        {
            var permisos = await _usuarioRepository.ObtenerPermisos();
            var roles = await _usuarioRepository.ObtenerRoles();

            return permisos.Select(p => new PermisoRolVM
            {
                IdPermiso = p.IdPermiso,
                NombrePermiso = p.Nombre,
                RolesPermisos = roles.Select(r => new RolPermisoVM
                {
                    IdRol = r.IdRol,
                    NombreRol = r.Nombre,
                    TienePermiso = _usuarioRepository.TienePermiso(p.IdPermiso, r.IdRol)
                }).ToList()
            }).ToList();
        }

        public async Task GuardarPermisos(List<PermisoRolVM> permisosRol)
        {
            foreach (var permisoRol in permisosRol)
            {
                foreach (var rolPermiso in permisoRol.RolesPermisos)
                {
                    bool tienePermiso = _usuarioRepository.TienePermiso(permisoRol.IdPermiso, rolPermiso.IdRol);

                    if (rolPermiso.TienePermiso)
                    {
                        if (!tienePermiso)
                        {
                            await _usuarioRepository.AgregarRolPermiso(permisoRol.IdPermiso, rolPermiso.IdRol);
                            //await _usuarioRepository.GuardarCambios();
                        }
                    }
                    else
                    {
                        if (tienePermiso)
                        {
                            await _usuarioRepository.EliminarRolPermiso(permisoRol.IdPermiso, rolPermiso.IdRol);
                        }
                    }
                    /*if (rolPermiso.TienePermiso)
                    {
                        if (!_usuarioRepository.TienePermiso(permisoRol.IdPermiso, rolPermiso.IdRol))
                        {
                            _usuarioRepository.AgregarRolPermiso(permisoRol.IdPermiso, rolPermiso.IdRol);
                        }
                    }
                    else
                    {
                        if (_usuarioRepository.TienePermiso(permisoRol.IdPermiso, rolPermiso.IdRol))
                        {
                            _usuarioRepository.EliminarRolPermiso(permisoRol.IdPermiso, rolPermiso.IdRol);
                        }
                    }*/
                }
            }

            await _usuarioRepository.GuardarCambios();
        }
        public async Task<List<Usuario>> GetAll()
        {
            List<Usuario> listado= await _usuarioRepository.GetAll();

            return listado;
        }

        public async Task<Usuario> GetUsuario(Int64 id)
        {
            Usuario usuario_encontrado = await _usuarioRepository.GetUsuario(id);

            return usuario_encontrado;
        }
        public async Task<ServiceResponse<Usuario>> GetUsuario(string correo, string clave)
        {
            var response = new ServiceResponse<Usuario>();

            try
            {
                //Usuario usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave && u.Restrictiva == 0).FirstOrDefaultAsync();
                Usuario usuario_encontrado = await _usuarioRepository.GetUsuario(correo,clave);

                if (usuario_encontrado != null)
                {
                    response.Data = usuario_encontrado;
                    response.Message = "Usuario encontrado exitosamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error usuario no encontrado";
                }
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = $"Ocurrió un error al buscar el usuario: {ex.Message}";
            }            

            return (response);
        }

        public async Task<ServiceResponse<Usuario>> SaveUsuario(UsuarioVM modelo)
        {
            var response = new ServiceResponse<Usuario>();

            try
            {
                // Encriptar claves
                modelo.Clave = Utilidades.Encriptar(modelo.Clave);
                modelo.ConfirmarClave = Utilidades.Encriptar(modelo.ConfirmarClave);

                // Validar claves
                if (modelo.Clave != modelo.ConfirmarClave)
                {
                    response.Success = false;
                    response.Message = "Las contraseñas no coinciden";
                    return response;
                }

                // Crear objeto Usuario
                Usuario usuario = new Usuario()
                {
                    IdUsuario= await _usuarioRepository.Cuenta(),
                    Nombre = modelo.Nombre,
                    Apellidos = modelo.Apellidos,
                    Correo = modelo.Correo,
                    Depto=modelo.Depto,
                    Clave = modelo.Clave,
                    Restrictiva = 0,
                    RolId = modelo.RolId
                };

                // Guardar el usuario en la base de datos
                _usuarioRepository.SaveUsuario(usuario);
                //_dbContext.Usuarios.Add(usuario);
                //await _dbContext.SaveChangesAsync();

                response.Data = usuario;
                response.Message = "Usuario creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el usuario: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Usuario>> UpdateUsuario(UsuarioVM modelo)
        {
            var response = new ServiceResponse<Usuario>();
            Usuario usuario_encontrado = await _usuarioRepository.GetUsuario(modelo.IdUsuario);

            try
            {
                // Encriptar claves
                //modelo.Clave = Utilidades.Encriptar(modelo.Clave);
                //modelo.ConfirmarClave = Utilidades.Encriptar(modelo.ConfirmarClave);

                // Validar claves
                if (modelo.Clave != modelo.ConfirmarClave)
                {
                    response.Success = false;
                    response.Message = "Las contraseñas no coinciden";
                    return response;
                }

                // Crear objeto Usuario
                //Usuario usuario = new Usuario()
                {
                    usuario_encontrado.Nombre = modelo.Nombre;
                    usuario_encontrado.Apellidos = modelo.Apellidos;
                    usuario_encontrado.Correo = modelo.Correo;
                    //usuario_encontrado.Clave = Utilidades.Encriptar(modelo.Clave);
                    usuario_encontrado.Depto = modelo.Depto;
                    usuario_encontrado.RolId = modelo.RolId;
                    //usuario_encontrado.Restrictiva = 0
                };
                _usuarioRepository.UpdateUsuario(usuario_encontrado);

                response.Data = usuario_encontrado;
                response.Message = "Usuario actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el usuario: {ex.Message}";
            }
            
            return response;
        }

        public async Task<ServiceResponse<Usuario>> DeleteUsuario(Int64 id)
        {
            var response = new ServiceResponse<Usuario>();
            Usuario usuario_encontrado = await _usuarioRepository.GetUsuario(id);

            try
            {               
              
                {
                    usuario_encontrado.Restrictiva = 1;
                };
                _usuarioRepository.UpdateUsuario(usuario_encontrado);

                response.Data = usuario_encontrado;
                response.Message = "Usuario eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el usuario: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Usuario>> ReiniciarClaveUsuario(UsuarioVM modelo)
        {
            var response = new ServiceResponse<Usuario>();
            Usuario usuario_encontrado = await _usuarioRepository.GetUsuario(modelo.IdUsuario);

            try
            {
                // Encriptar claves
                //modelo.Clave = Utilidades.Encriptar(modelo.Clave);
                //modelo.ConfirmarClave = Utilidades.Encriptar(modelo.ConfirmarClave);

                // Validar claves
                if (modelo.Clave != modelo.ConfirmarClave)
                {
                    response.Success = false;
                    response.Message = "Las contraseñas no coinciden";
                    return response;
                }

                // Crear objeto Usuario
                //Usuario usuario = new Usuario()
                {
                    //usuario_encontrado.Nombre = modelo.Nombre;
                    //usuario_encontrado.Apellidos = modelo.Apellidos;
                    //usuario_encontrado.Correo = modelo.Correo;
                    usuario_encontrado.Clave = Utilidades.Encriptar(modelo.Clave);
                    //usuario_encontrado.Depto = modelo.Depto;
                    //usuario_encontrado.RolId = modelo.RolId;
                    //usuario_encontrado.Restrictiva = 0
                };
                _usuarioRepository.UpdateUsuario(usuario_encontrado);

                response.Data = usuario_encontrado;
                response.Message = "Contraseña actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el usuario: {ex.Message}";
            }

            return response;
        }
    }    

    /*public async Task<ServiceResponse<Usuario>> SaveUsuario(Usuario modelo)
        {
            var response = new ServiceResponse<Usuario>();

            try
            {
                await _dbContext.Usuarios.AddAsync(modelo);
                await _dbContext.SaveChangesAsync();
            
                response.Data = modelo;
                response.Message = "Usuario creado exitosamente";
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = $"Error al crear el usuario {ex.Message}";
            }

            //return modelo;
            return response;
        }*/
    //}
}

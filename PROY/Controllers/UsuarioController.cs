using Microsoft.AspNetCore.Mvc;
using PROY.Data;
using PROY.Models;
using PROY.Services;
using Microsoft.EntityFrameworkCore;
using PROY.ViewModels;
using PROY.Helpers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PROY.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        //private readonly ICatalogoService _catalogoService;
        private readonly AppDBContext _dbContext;
        public UsuarioController(IUsuarioService usuarioService, AppDBContext dbContext)
        {
            _usuarioService = usuarioService;
            //_catalogoService = catalogoService;
            _dbContext = dbContext;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            List<Usuario> listado = await _usuarioService.GetAll();
            return View(listado);
            //return View();
        }

        public async Task<IActionResult> Permisos()
        {
            
            List<Permiso> listado= await _usuarioService.GetAllPermisos();

            return View(listado);
        }

        [HttpGet]
        public async Task<IActionResult> GestionarPermisos()
        {
            var model = await _usuarioService.ObtenerPermisosConRoles();
            return View(model);
        }

        [HttpPost]
        public IActionResult GuardarPermisos(List<PermisoRolVM> model)
        {
            _usuarioService.GuardarPermisos(model);
            return RedirectToAction("GestionarPermisos");
        }
        /*public IActionResult GestionarPermisos()
        {
            var permisos = _context.Permisos.ToList();
            var roles = _context.Roles.ToList();

            var model = permisos.Select(p => new PermisoRolViewModel
            {
                IdPermiso = p.IdPermiso,
                NombrePermiso = p.NombrePermiso,
                RolesPermisos = roles.Select(r => new RolPermisoViewModel
                {
                    IdRol = r.IdRol,
                    NombreRol = r.NombreRol,
                    TienePermiso = _context.RolesPermisos.Any(rp => rp.IdPermiso == p.IdPermiso && rp.IdRol == r.IdRol)
                }).ToList()
            }).ToList();

            return View(model);
        }*/

        public async Task<IActionResult> Registrarse() 
        {
            var roles = await _usuarioService.GetAllRoles();
            ViewBag.Roles = new SelectList(roles, "IdRol", "Nombre");

            var deptos = await _usuarioService.GetAllDeptos();
            ViewBag.Deptos = new SelectList(deptos, "IdCatalogo", "Descripcion");

            TempData["Mensaje"] = null;

            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        {
            modelo.Clave = Utilidades.Encriptar(modelo.Clave);
            modelo.ConfirmarClave = Utilidades.Encriptar(modelo.ConfirmarClave);

            if (modelo.Clave != modelo.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            Usuario usuario = new Usuario()
            {
                Nombre = modelo.Nombre,
                Apellidos = modelo.Apellidos,
                Correo = modelo.Correo,
                Clave = modelo.Clave,
                Restrictiva=0
            };

            Usuario usuario_creado=await _usuarioService.SaveUsuario(usuario);

            if (usuario_creado.IdUsuario>0)
                return RedirectToAction("Login", "Usuario");

            ViewData["Mensaje"] = "Error no se pudo crear el usuario";

            return View();
        }*/

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        {
            var roles = await _usuarioService.GetAllRoles();
            ViewBag.Roles = new SelectList(roles, "RolId", "Nombre");

            
            
            var result = await _usuarioService.SaveUsuario(modelo);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelo);
            }
        }

        [Authorize]
        public async Task<IActionResult> Editar (int id)
        {
            Usuario usuario = await _usuarioService.GetUsuario(id);
            var roles = await _usuarioService.GetAllRoles();
            ViewBag.Roles = new SelectList(roles, "IdRol", "Nombre");

            var deptos=await _usuarioService.GetAllDeptos();
            ViewBag.Deptos = new SelectList(deptos, "IdCatalogo", "Descripcion");     

            TempData["Mensaje"] = null;
            UsuarioVM modelo = new UsuarioVM()
            {
                IdUsuario = id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Depto = usuario.Depto,
                //Clave = usuario.Clave,
                //ConfirmarClave=usuario.Clave,
                RolId = usuario.RolId                
            };
            return View(modelo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioVM usuario)
        {
            var result = await _usuarioService.UpdateUsuario(usuario);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(usuario);
            }            
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _usuarioService.DeleteUsuario(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Start", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            //Usuario usuario_encontrado=await _usuarioService.GetUsuario(correo,Utilidades.Encriptar(clave));
            var result = await _usuarioService.GetUsuario(correo, Utilidades.Encriptar(clave));

            /*if (result.Success)
            {
                return Ok(result.Data); // Retorna el usuario si fue encontrado
            }
            else*/
            if (result.Success==false)
            {
                //return NotFound(result.Message); // Retorna un 404 Not Found con el mensaje de error
                TempData["Mensaje"] = result.Message;
                return View();
            }

            string depto = null;
            try
            {
                depto = await _dbContext.Catalogos
                        .Where(d => d.IdCatalogo.ToString() == result.Data.Depto)
                        .Select(d => d.Descripcion)
                        .FirstOrDefaultAsync();
            }
            catch
            {
                depto = "DAAN";
                result.Data.Depto = "1";
            }
            
            if (result.Data.Rol.Nombre == "Admin")
            {
                depto = "DAAN";
                result.Data.Depto = "1";
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, result.Data.Nombre + " " + result.Data.Apellidos),
                new Claim(ClaimTypes.Role, result.Data.Rol.Nombre),
                new Claim("idDepto",result.Data.Depto),
                new Claim("Depto", depto),
                new Claim("Usuario", result.Data.Correo),
                new Claim("IDuser", result.Data.IdUsuario.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            //Registrar inicio de sesion
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );
            
            return RedirectToAction("Start", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario"); // Redirige al formulario de login después de cerrar sesión
        }

        public async Task<IActionResult> ReiniciarClave(string id)
        {            
            if (User.Identity!.IsAuthenticated)
            {
                Usuario usuario = await _usuarioService.GetUsuario(Convert.ToInt64(id));

                UsuarioVM modelo = new UsuarioVM()
                {
                    IdUsuario = Convert.ToInt64(id)
                    //Nombre = usuario.Nombre,
                    //Apellidos = usuario.Apellidos,
                    //Correo = usuario.Correo,
                    //Depto = usuario.Depto,
                    //Clave = usuario.Clave,
                    //ConfirmarClave=usuario.Clave,
                    //RolId = usuario.RolId
                };

                return View(modelo);
            }
                
            
            return RedirectToAction("Login", "Usuario");            
        }

        [HttpPost]
        public async Task<IActionResult> ReiniciarClave(UsuarioVM usuario)
        {
            var result = await _usuarioService.ReiniciarClaveUsuario(usuario);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Start", "Home");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(usuario);
            }
        }

    }
}

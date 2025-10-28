using PROY.Data;
using PROY.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PROY.Repositories;
using Oracle.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

/*builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnx"));
});*/
//Configurar sesiones
builder.Services.AddDistributedMemoryCache(); // Necesario para usar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Duracion de la sesion inactiva
    options.Cookie.HttpOnly = true; // Hace que la cookie de sesion sea accesible solo desde el servidor
    options.Cookie.IsEssential = true; // Requerido para cumplir con las poloticas de GDPR si usas cookies esenciales
});
//------------------------------

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("cnx"));
});

//Permitir usar el servicio usuario dentro de cualquier controlador
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISolicitudRepository, SolicitudRepository>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<ITecnicoRepository, TecnicoRepository>();
builder.Services.AddScoped<ITecnicoService, TecnicoService>();
builder.Services.AddScoped<IEvaluacionRepository, EvaluacionRepository>();
builder.Services.AddScoped<IEvaluacionService, EvaluacionService>();
builder.Services.AddScoped<IProgramacionRepository, ProgramacionRepository>();
builder.Services.AddScoped<IProgramacionService, ProgramacionService>();
builder.Services.AddScoped<IEjecucionRepository, EjecucionRepository>();
builder.Services.AddScoped<IEjecucionService, EjecucionService>();
builder.Services.AddScoped<IDespachoRepository, DespachoRepository>();
builder.Services.AddScoped<IDespachoService, DespachoService>();

// Home
builder.Services.AddScoped<ITableroDeDatosRepository, TableroDeDatosRepository>();
builder.Services.AddScoped<ITableroDeDatosService, TableroDeDatosService>();

// Modulo de Catalogos
builder.Services.AddScoped<IBodegaRepository, BodegaRepository>();
builder.Services.AddScoped<IBodegaService, BodegaService>();
builder.Services.AddScoped<IProgramaRepository, ProgramaRepository>();
builder.Services.AddScoped<IProgramaService, ProgramaService>();
builder.Services.AddScoped<IConvenioRepository, ConvenioRepository>();
builder.Services.AddScoped<IConvenioService, ConvenioService>();
builder.Services.AddScoped<IPresentacionPARepository, PresentacionPARepository>();
builder.Services.AddScoped<IPresentacionPAService, PresentacionPAService>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();
builder.Services.AddScoped<IProductoAlimenticioRepository, ProductoAlimenticioRepository>();
builder.Services.AddScoped<IProductoAlimenticioService, ProductoAlimenticioService>();
builder.Services.AddScoped<IRacionRepository, RacionRepository>();
builder.Services.AddScoped<IRacionService, RacionService>();
builder.Services.AddScoped<ITipoBodegaRepository, TipoBodegaRepository>();
builder.Services.AddScoped<ITipoBodegaService, TipoBodegaService>();
builder.Services.AddScoped<IPuestoRepository, PuestoRepository>();
builder.Services.AddScoped<IPuestoService, PuestoService>();
builder.Services.AddScoped<IRenglonPresupuestarioRepository, RenglonPresupuestarioRepository>();
builder.Services.AddScoped<IRenglonPresupuestarioService, RenglonPresupuestarioService>();
builder.Services.AddScoped<IPersonalBodegaRepository, PersonalBodegaRepository>();
builder.Services.AddScoped<IPersonalBodegaService, PersonalBodegaService>();
builder.Services.AddScoped<ISubBodegaRepository, SubBodegaRepository>();
builder.Services.AddScoped<ISubBodegaService, SubBodegaService>();
builder.Services.AddScoped<IEstibaRepository, EstibaRepository>();
builder.Services.AddScoped<IEstibaService, EstibaService>();
builder.Services.AddScoped<ITransportistaRepository, TransportistaRepository>();
builder.Services.AddScoped<ITransportistaService, TransportistaService>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IPilotoRepository, PilotoRepository>();
builder.Services.AddScoped<IPilotoService, PilotoService>();
builder.Services.AddScoped<ITipoAsistenciaRepository, TipoAsistenciaRepository>();
builder.Services.AddScoped<ITipoAsistenciaService, TipoAsistenciaService>();
builder.Services.AddScoped<ITipoIntervencionRepository, TipoIntervencionRepository>();
builder.Services.AddScoped<ITipoIntervencionService, TipoIntervencionService>();
builder.Services.AddScoped<IUsuarioPersonaRepository, UsuarioPersonaRepository>();
builder.Services.AddScoped<IUsuarioPersonaService, UsuarioPersonaService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();

// M�dulo de Procesos
builder.Services.AddScoped<IIngresoAlimentosRepository, IngresoAlimentosRepository>();
builder.Services.AddScoped<IIngresoAlimentosService, IngresoAlimentosService>();
builder.Services.AddScoped<IEgresoAlimentosRepository, EgresoAlimentosRepository>();
builder.Services.AddScoped<IEgresoAlimentosService, EgresoAlimentosService>();
builder.Services.AddScoped<IReingresoAlimentosRepository, ReingresoAlimentosRepository>();
builder.Services.AddScoped<IReingresoAlimentosService, ReingresoAlimentosService>();
builder.Services.AddScoped<IJuntaDeRecepcionRepository, JuntaDeRecepcionRepository>();
builder.Services.AddScoped<IJuntaDeRecepcionService, JuntaDeRecepcionService>();

// M�dulo de Reportes
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();
builder.Services.AddScoped<IReporteService, ReporteService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// Agrega servicios al contenedor.
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });*/

var app = builder.Build();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120), // Ajustar el tiempo de keep-alive seg�n sea necesario
});

//--------------------------------------
//builder.Services.AddCors(options =>
//{
//   options.AddPolicy("AllowAllOrigins", builder =>
//  {
//        builder.AllowAnyOrigin()
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//    });
//});

app.UseCors("AllowAllOrigins");
//---------------------------------------
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();



app.UseRouting();

//-------------------------------------------
app.UseSession();
//-------------------------------------------

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

//--------------------------------------
/*app.Use(async (context, next) =>
{
    if (context.Request.Path == "/PROY")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            // L�gica para manejar la conexi�n WebSocket
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});*/
//--------------------------------

app.Run();

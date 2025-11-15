using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using PROY.Models;

namespace PROY.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolesPermisos { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<AsignaTecnico> AsignaTecnicos { get; set; }
        public DbSet<DetalleSolicitud> DetalleSolicitud { get; set; }
        public DbSet<DetalleSolicitudAPA> DetalleSolicitudAPA { get; set; }
        public DbSet<DetalleEvaluacion> DetalleEvaluacion {  get; set; }
        public DbSet<DetalleEvaluacionAPA> DetalleEvaluacionAPA { get; set; }
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<AsignaRacion> AsignaRaciones { get; set; }
        public DbSet<AsignaRacionV2> AsignaRacionV2 { get; set; }
        public DbSet<AsignaTecnicoV2> AsignaTecnicoV2 { get; set; }
        public DbSet<TipoIntervencion> TipoIntervencion { get; set; }
        public DbSet<EstadoSolicitud> EstadoSolicitud { get; set; }

        
        // Módulo de Bodegas
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<TipoBodega> TipoBodegas { get; set; }
        public DbSet<SubBodega> SubBodegas { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<RenglonPresupuestario> RenglonesPresupuestarios { get; set; }
        public DbSet<PersonalBodega> PersonalBodegas { get; set; }
        public DbSet<Convenio> Convenio { get; set; }
        public DbSet<Programa> Programa { get; set; }
        public DbSet<ProductoAlimenticio> ProductoAlimenticio { get; set; }
        public DbSet<RacionMaestro> RacionMaestro { get; set; }
        public DbSet<RacionDetalle> RacionDetalle { get; set; }
        public DbSet<PresentacionPA> PresentacionPA { get; set; }
        public DbSet<UnidadMedida> UnidadMedida { get; set; }
        public DbSet<EquivalenciasMedida> EquivalenciasMedida { get; set; }
        public DbSet<Estiba> Estibas { get; set; }
        public DbSet<Transportista> Transportistas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Piloto> Pilotos { get; set; }
        public DbSet<JuntaReceptora> JuntaReceptora { get; set; }
        public DbSet<DetalleJuntaReceptora> DetalleJuntaReceptora { get; set; }
        public DbSet<TipoNombramiento> TipoNombramiento { get; set; }
        public DbSet<EstadoIngresos> EstadoIngresos { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<IngresoAlimentos> IngresoAlimentos { get; set; }
        public DbSet<Kardex> Kardex { get; set; }
        public DbSet<DetalleBoletaAnalisis> DetalleBoletaAnalisis { get; set; }
        public DbSet<ProductosIngreso> ProductosIngreso { get; set; }
        public DbSet<MovimientosProductos> MovimientosProductos { get; set; }
        public DbSet<EgresoAlimentos> EgresoAlimentos { get; set; }
        public DbSet<DetalleEgresoAlimentos> DetalleEgresoAlimentos { get; set; }
        public DbSet<SolicitudSalidaAlimentos> SolicitudSalidaAlimentos { get; set; }
        public DbSet<DetalleSalidaAlimentos> DetalleSalidaAlimentos { get; set; }
        public DbSet<EstadoEgresos> EstadoEgresos { get; set; }
        public DbSet<ReingresoAlimentos> ReingresoAlimentos { get; set; }
        public DbSet<EstadoReingresos> EstadoReingresos { get; set; }
        public DbSet<DetalleReingresoAlimentos> DetalleReingresoAlimentos { get; set; }
        public DbSet<TrasladoAlimentos> TrasladoAlimentos { get; set; }
        public DbSet<EstadoTraslados> EstadoTraslados { get; set; }
        public DbSet<TipoAsistencia> TipoAsistencia { get; set; }
        public DbSet<DetalleWaybill> DetalleWaybill { get; set; }
        public DbSet<UsuarioPersona> UsuarioPersona { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Nino> Ninos { get; set; }

        public DbSet<Proyecto> Proyecto { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            // Configuración de Usuario con Rol (Uno a Muchos)
            
            modelBuilder.HasAnnotation("Relational:DefaultSchema", ControlGlobal.Esquema);
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<Usuario>()
                .HasOne(r => r.Rol)
                .WithMany(p => p.Usuarios);

            modelBuilder.Entity<Usuario>(tb =>
            {
                /*tb.HasOne(u => u.Rol)
                  .WithMany(r => r.Usuarios)
                  .HasForeignKey(u => u.RolId)
                  .OnDelete(DeleteBehavior.Restrict);*/

                tb.HasKey(col => col.IdUsuario);
                tb.HasIndex(col => col.IdUsuario).IsUnique();
                //tb.Property(col => col.IdUsuario).HasColumnName("IDUSUARIO");.HasDefaultValueSql("ISEQ$$_IDUSUARIO.NEXTVAL"); ;
                //tb.Property(col => col.IdUsuario).HasDefaultValueSql("ISEQ$$_IDUSUARIO.NEXTVAL");
                tb.Property(col => col.IdUsuario).ValueGeneratedOnAdd().HasColumnName("IDUSUARIO");
                tb.Property(col => col.Nombre).HasMaxLength(150).HasColumnName("NOMBRE");
                tb.Property(col => col.Apellidos).HasMaxLength(150).HasColumnName("APELLIDO");
                tb.Property(col => col.Correo).HasMaxLength(150).HasColumnName("CORREO");
                tb.Property(col => col.Clave).HasMaxLength(256).HasColumnName("CLAVE");
                tb.Property(col => col.Depto).HasMaxLength(5).HasColumnName("DEPTO");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.Property(col => col.RolId).HasColumnName("IDROL");
                

                /*modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)          // Un Usuario tiene un Rol
            .WithMany(r => r.Usuarios)    // Un Rol tiene muchos Usuarios
            .HasForeignKey(u => u.RolId); // RolId es la clave foránea en Usuario*/
            });

            modelBuilder.Entity<Rol>().ToTable("ROL");
            modelBuilder.Entity<Rol>()
                .HasMany(p => p.Permisos)
                .WithMany(r => r.Roles)
                .UsingEntity<RolPermiso>(
                    rp=>rp.HasOne(prop=>prop.Permiso).WithMany().HasForeignKey(prop=>prop.PermisoId),
                    rp=>rp.HasOne(prop=>prop.Rol).WithMany().HasForeignKey(prop=>prop.RolId),
                    rp => 
                    {
                        rp.HasKey(prop => new { prop.RolId, prop.PermisoId });
                    }
                );

            modelBuilder.Entity<Rol>(tb =>
            {
                tb.HasKey(col => col.IdRol);
                tb.HasIndex(col => col.IdRol).IsUnique();
                tb.Property(col => col.IdRol).ValueGeneratedOnAdd().HasColumnName("IDROL");
                tb.Property(col => col.Nombre).HasMaxLength(150).HasColumnName("NOMBRE");
                tb.Property(col => col.Descripcion).HasMaxLength(500).HasColumnName("DESCRIPCION");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.HasMany(r => r.Usuarios);
                  
            });

            modelBuilder.Entity<Permiso>().ToTable("PERMISO");
            modelBuilder.Entity<Permiso>(tb =>
            {
                tb.HasKey(col => col.IdPermiso);
                tb.HasIndex(col => col.IdPermiso).IsUnique();
                tb.Property(col => col.IdPermiso).ValueGeneratedOnAdd().HasColumnName("IDPERMISO");
                tb.Property(col => col.Nombre).HasMaxLength(150).HasColumnName("NOMBRE");                
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.HasMany(r => r.Roles);
            });

            // Configuración de RolPermiso (Muchos a Muchos)
            /*modelBuilder.Entity<RolPermiso>().ToTable("Rol_Permiso");
            modelBuilder.Entity<RolPermiso>(tb =>
            {
                tb.HasKey(col => col.IdRolPermiso);
                tb.Property(col => col.IdRolPermiso).UseIdentityColumn().ValueGeneratedOnAdd().HasColumnName("idRolPermiso");
                //tb.Property(col => col.RolId).HasColumnName("idRol");
                //tb.Property(col => col.PermisoId).HasColumnName("idPermiso");
            });*/


            /*modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });*/

            /*modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.RolId);

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.PermisoId);*/

            modelBuilder.Entity<RolPermiso>().ToTable("ROLPERMISO");
            modelBuilder.Entity<RolPermiso>().HasKey(rp => new { rp.PermisoId, rp.RolId });  // Clave primaria compuesta
            modelBuilder.Entity<RolPermiso>().HasIndex(rp=> new { rp.PermisoId, rp.RolId });
            modelBuilder.Entity<RolPermiso>(tb =>
            {                 
                tb.Property(col => col.RolId).HasColumnName("IDROL");
                tb.Property(col => col.PermisoId).HasColumnName("IDPERMISO");
            });
            //tb.Property(col => col.PermisoId).HasColumnName("idPermiso");
            /*modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => new { rp.RolId, rp.PermisoId });  // Clave primaria compuesta

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.RolId);

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.PermisoId);*/

            modelBuilder.Entity<Tecnico>(tb=>
            {
                tb.ToTable("TECNICO");
                tb.HasKey(col => col.IdTecnico);
                tb.HasIndex(col => col.IdTecnico).IsUnique();
                tb.Property(col => col.IdTecnico).ValueGeneratedOnAdd().HasColumnName("ID_TECNICO");
                tb.Property(col => col.Nombre).HasColumnName("NOMBRE");
                tb.Property(col => col.Telefono).HasColumnName("TELEFONO");                
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
            });

            modelBuilder.Entity<AsignaTecnico>(tb =>
            {
                tb.ToTable("ASIGNA_TECNICO");
                tb.HasKey(col => col.IdAsignaTecnico);
                tb.HasIndex(col => col.IdAsignaTecnico).IsUnique();
                tb.Property(col => col.IdAsignaTecnico).ValueGeneratedOnAdd().HasColumnName("ID_ASIGNA_TECNICO");
                tb.Property(col => col.TecnicoId).HasColumnName("ID_TECNICO");
                tb.Property(col => col.SolicitudId).HasColumnName("ID_SOLICITUD");
                tb.Property(col => col.DepartamentoId).HasColumnName("ID_DEPARTAMENTO");
                tb.Property(col => col.IdMunicipio).HasColumnName("ID_MUNICIPIO");
                tb.Property(col => col.DireccionEntrega).HasColumnName("DIRECCION_ENTREGA");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                //tb.HasOne(at => at.Solicitud)
                //  .WithMany(s => s.AsignaTecnicos)
                //  .HasForeignKey(at => at.SolicitudId);  // Asegúrate que esto coincide con la columna en la BD

                //tb.HasOne(at => at.Tecnico)
                //  .WithMany(t => t.AsignaTecnicos)
                //  .HasForeignKey(at => at.TecnicoId);
            });

            modelBuilder.Entity<AsignaTecnicoV2>(tb =>
            {
                tb.ToTable("ASIGNA_TECNICO_V2");
                tb.HasKey(col => col.IdAsignaTecnico);
                tb.HasIndex(col => col.IdAsignaTecnico).IsUnique();
                tb.Property(col => col.IdAsignaTecnico).ValueGeneratedOnAdd().HasColumnName("ID_ASIGNA_TECNICO");                
                tb.Property(col => col.IdSolicitud).HasColumnName("ID_SOLICITUD").IsRequired();
                tb.Property(col => col.IdTecnico).HasColumnName("ID_TECNICO").IsRequired();
                tb.Property(col => col.IdDepartamento).HasColumnName("ID_DEPARTAMENTO").IsRequired();
                tb.Property(col => col.IdMunicipio).HasColumnName("ID_MUNICIPIO").IsRequired();
                tb.Property(col => col.DireccionEntrega).HasColumnName("DIRECCION_ENTREGA").HasMaxLength(250).IsRequired();
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA").IsRequired();
                tb.HasOne(d => d.Tecnico)
                    .WithMany()
                    .HasForeignKey(d => d.IdTecnico)
                    .HasConstraintName("ASIGNA_TECNICO_V2_TECNICO_FK");
            });

            modelBuilder.Entity<AsignaRacion>(tb =>
            {
                tb.ToTable("ASIGNA_RACION");
                tb.HasKey(col => col.IdAsignaRacion);
                tb.HasIndex(col => col.IdAsignaRacion).IsUnique();
                tb.Property(col => col.IdAsignaRacion).ValueGeneratedOnAdd().HasColumnName("ID_ASIGNA_RACION");
                tb.Property(col => col.SolicitudId).HasColumnName("ID_SOLICITUD");
                tb.Property(col => col.DepartamentoId).HasColumnName("ID_DEPARTAMENTO");
                tb.Property(col => col.IdMunicipio).HasColumnName("ID_MUNICIPIO");
                tb.Property(col => col.Comunidad).HasColumnName("COMUNIDAD");
                tb.Property(col => col.RacionAsignada).HasColumnName("RACION_ASIGNADA");
                tb.Property(col => col.RacionSaldo).HasColumnName("RACION_SALDO");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                //tb.HasOne(at => at.Solicitud)
                //  .WithMany(s => s.AsignaTecnicos)
                //  .HasForeignKey(at => at.SolicitudId);  // Asegúrate que esto coincide con la columna en la BD

                //tb.HasOne(at => at.Tecnico)
                //  .WithMany(t => t.AsignaTecnicos)
                //  .HasForeignKey(at => at.TecnicoId);
            });
            
            modelBuilder.Entity<AsignaRacionV2>(tb =>
            {
                tb.ToTable("ASIGNA_RACION_V2");
                tb.HasKey(col => col.IdAsignaRacion);
                tb.HasIndex(col => col.IdAsignaRacion).IsUnique();
                tb.Property(col => col.IdAsignaRacion).ValueGeneratedOnAdd().HasColumnName("ID_ASIGNA_RACION");                
                tb.Property(col => col.IdSolicitud).HasColumnName("ID_SOLICITUD").IsRequired();
                tb.Property(col => col.IdDepartamento).HasColumnName("ID_DEPARTAMENTO").IsRequired();
                tb.Property(col => col.IdMunicipio).HasColumnName("ID_MUNICIPIO").IsRequired();
                tb.Property(col => col.Comunidad).HasColumnName("COMUNIDAD").HasMaxLength(250).IsRequired();
                tb.Property(col => col.RacionAsignada).HasColumnName("RACION_ASIGNADA").IsRequired();
                tb.Property(col => col.RacionSaldo).HasColumnName("RACION_SALDO").IsRequired();
                tb.Property(col => col.UsuarioRegistro).HasColumnName("USUARIO_REGISTRO").HasMaxLength(30).IsRequired();
                tb.Property(col => col.FechaRegistro).HasColumnName("FECHA_REGISTRO").IsRequired();
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA").IsRequired();
            });

            modelBuilder.Entity<Deptos>(tb =>
            {
                tb.ToTable("DEPTOS");
                tb.HasKey(col => col.Id);
                tb.HasIndex(col => col.Id).IsUnique();
                tb.Property(col => col.Id).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreDepto).HasColumnName("NOMBRE_DEPTO");
                tb.Property(col => col.Abreviatura).HasColumnName("ABREVIATURA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
                tb.ToTable("DEPTOS", "SGI");
                tb.HasKey(e => e.Id);
            });

            modelBuilder.Entity<TipoIntervencion>(tb =>
            {
                tb.ToTable("TIPO_INTERVENCION");
                tb.HasKey(col => col.IdTipoIntervencion);
                tb.HasIndex(col => col.IdTipoIntervencion).IsUnique();
                tb.Property(col => col.IdTipoIntervencion).ValueGeneratedOnAdd().HasColumnName("ID_TIPO_INTERVENCION");
                tb.Property(col => col.Nombre).HasColumnName("NOMBRE").HasMaxLength(255).IsRequired();
                tb.Property(col => col.IdDepto).HasColumnName("ID_DEPTO").IsRequired();
                tb.Property(col => col.Estado).HasColumnName("ESTADO").IsRequired().HasDefaultValue(0);
                tb.HasOne(e => e.Depto)
                  .WithMany()
                  .HasForeignKey(e => e.IdDepto)
                  .HasConstraintName("TIPO_INTERVENCION_DEPTOS_FK");
            });
            
            modelBuilder.Entity<EstadoSolicitud>(tb =>
            {
                tb.ToTable("ESTADO_SOLICITUD");
                tb.HasKey(col => col.IdEstadoSolicitud);
                tb.HasIndex(col => col.IdEstadoSolicitud).IsUnique();
                tb.Property(col => col.IdEstadoSolicitud).ValueGeneratedOnAdd().HasColumnName("ID_ESTADO_SOLICITUD");
                tb.Property(col => col.Nombre).HasColumnName("NOMBRE").HasMaxLength(75).IsRequired();
                tb.Property(col => col.IdEstadoSolicitudAnterior).HasColumnName("ID_ESTADO_SOLICITUD_ANTERIOR");
                tb.Property(col => col.IdEstadoSolicitudSiguiente).HasColumnName("ID_ESTADO_SOLICITUD_SIGUIENTE");
                tb.Property(col => col.IdDepto).HasColumnName("ID_DEPTO").IsRequired();
                tb.HasOne(d => d.EstadoSolicitudAnterior)
                    .WithMany()
                    .HasForeignKey(d => d.IdEstadoSolicitudAnterior)
                    .HasConstraintName("ESTADO_SOLICITUD_ANTERIOR_FK");
                tb.HasOne(d => d.EstadoSolicitudSiguiente)
                    .WithMany()
                    .HasForeignKey(d => d.IdEstadoSolicitudSiguiente)
                    .HasConstraintName("ESTADO_SOLICITUD_SIGUIENTE_FK");
                tb.HasOne(e => e.Depto)
                    .WithMany()
                    .HasForeignKey(e => e.IdDepto)
                    .HasConstraintName("ESTADO_SOLICITUD_DEPTO_FK");
            });

            modelBuilder.Entity<Solicitud>(tb =>
            {
                tb.ToTable("SOLICITUD");
                tb.HasKey(col => col.IdSolicitud);
                tb.HasIndex(col => col.IdSolicitud).IsUnique();
                tb.Property(col => col.IdSolicitud).ValueGeneratedOnAdd().HasColumnName("IDSOLICITUD");
                tb.Property(col => col.IdIntervencion).HasColumnName("IDINTERVENCION");
                tb.Property(col => col.EstadoEgreso).HasColumnName("ESTADOEGRESO");
                tb.Property(col => col.Correlativo).HasMaxLength(25).HasColumnName("CORRELATIVO"); //NoSolicitud escrita usuario
                tb.Property(col => col.Codigo).HasMaxLength(20).HasColumnName("CODIGO");
                tb.Property(col => col.FechaIngresoDespacho).HasColumnName("FECHAINGRESODESPACHO");
                tb.Property(col => col.FechaIngresoUnidad).HasColumnName("FECHAINGRESOUNIDAD");
                tb.Property(col => col.IdUnidad).HasColumnName("IDUNIDAD");                
                tb.Property(col => col.Archivo).HasColumnName("NOMBREARCHIVO");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
                tb.Property(col => col.Fallecidos).HasColumnName("FALLECIDOS");
                tb.Property(col => col.Duplicados).HasColumnName("DUPLICADOS");
                tb.Property(col => col.SinDatos).HasColumnName("SINDATOS");
                tb.Property(col => col.Gobierno).HasColumnName("GOBIERNO");
                tb.Property(col => col.Depto).HasMaxLength(5).HasColumnName("DEPTO");
                tb.Property(col => col.IdInstitucion).HasColumnName("IDINSTITUCION");
                tb.Property(col => col.IdProyecto).HasColumnName("IDPROYECTO");
                tb.Property(col => col.Descripcion).HasColumnName("DESCRIPCION");
                tb.Property(col => col.FInicioEntrega).HasColumnName("FINICIO_ENTREGA");
                tb.Property(col => col.FFinEntrega).HasColumnName("FFINAL_ENTREGA");
                tb.Property(col => col.DireccionEntrega).HasColumnName("DIRECCION_ENTREGA");
                tb.Property(col => col.TecnicoNombre).HasColumnName("TECNICO_ASIGNADO");
                tb.Property(col => col.TecnicoTelefono).HasColumnName("TECNICO_TELEFONO");
                tb.Property(col => col.TecnicoObservaciones).HasColumnName("OBSERVACIONES_ENTREGA");
                tb.Property(col => col.Raciones).HasColumnName("RACIONES");
                tb.Property(col => col.Departamento).HasColumnName("DEPARTAMENTO");
                tb.Property(col => col.Municipio).HasColumnName("MUNICIPIO");
                tb.Property(col => col.Comunidad).HasColumnName("COMUNIDAD");
                tb.Property(col => col.FechaRegistro).HasColumnName("FECHA_REGISTRO");
                tb.Property(col => col.UsuarioRegistro).HasColumnName("USUARIO_REGISTRO");
                tb.Property(col => col.FechaIngresoDireccion).HasColumnName("FECHAINGRESODIRECCION");
                tb.Property(col => col.NombreCocode).HasColumnName("NOMBRE_COCODE");
                tb.Property(col => col.ComunidadCocode).HasColumnName("COMUNIDAD_COCODE");
                tb.Property(col => col.TelefonoCocode).HasColumnName("TELEFONO_COCODE");
                tb.Property(col => col.TelefonoCocode).HasColumnName("TELEFONO_COCODE");
                tb.Property(col => col.IdEstadoSolicitud).HasColumnName("ID_ESTADO_SOLICITUD");
                tb.Property(col => col.Rechazada).HasColumnName("RECHAZADA");
                tb.Property(col => col.ComentarioRechazo).HasColumnName("COMENTARIO_RECHAZO");
                tb.HasOne(e => e.EstadoSolicitud)
                    .WithMany()
                    .HasForeignKey(e => e.IdEstadoSolicitud)
                    .HasConstraintName("SOLICITUD_ESTADO_SOLICITUD_FK");
            });

            modelBuilder.Entity<DetalleSolicitud>(tb =>
            {
                tb.ToTable("DETALLESOLICITUD_DAAN");
                tb.HasKey(col => col.IdDetalleSolicitud);
                tb.HasIndex(col => col.IdDetalleSolicitud).IsUnique();
                tb.Property(col => col.IdDetalleSolicitud).ValueGeneratedOnAdd().HasColumnName("IDDETALLESOLICITUD");
                tb.Property(col => col.IdSolicitud).HasColumnName("IDSOLICITUD");
                tb.Property(col => col.Correlativo).HasColumnName("CORRELATIVO");
                tb.Property(col => col.CodComunidad).HasColumnName("CODCOMUNIDAD");

                //tb.Property(col => col.IdIntervencion).HasColumnName("IDINTERVENCION");
                tb.Property(col => col.IdDepartamento).HasColumnName("IDDEPARTAMENTO");
                tb.Property(col => col.IdMunicipio).HasColumnName("IDMUNICIPIO");
                tb.Property(col => col.Comunidad).HasMaxLength(250).HasColumnName("COMUNIDAD");
                tb.Property(col => col.Nombre1Esposo).HasColumnName("NOMBRE1_ESPOSO");
                tb.Property(col => col.Nombre2Esposo).HasColumnName("NOMBRE2_ESPOSO");
                tb.Property(col => col.Nombre3Esposo).HasColumnName("NOMBRE3_ESPOSO");
                tb.Property(col => col.Apellido1Esposo).HasColumnName("APELLIDO1_ESPOSO");
                tb.Property(col => col.Apellido2Esposo).HasColumnName("APELLIDO2_ESPOSO");
                tb.Property(col => col.CuiEsposo).HasColumnName("CUI_ESPOSO");
                tb.Property(col => col.FNacimientoEsposo).HasColumnName("FNACIMIENTO_ESPOSO");

                tb.Property(col => col.Nombre1Esposa).HasColumnName("NOMBRE1_ESPOSA");
                tb.Property(col => col.Nombre2Esposa).HasColumnName("NOMBRE2_ESPOSA");
                tb.Property(col => col.Nombre3Esposa).HasColumnName("NOMBRE3_ESPOSA");
                tb.Property(col => col.Apellido1Esposa).HasColumnName("APELLIDO1_ESPOSA");
                tb.Property(col => col.Apellido2Esposa).HasColumnName("APELLIDO2_ESPOSA");
                tb.Property(col => col.ApellidoCasada).HasColumnName("APELLIDO_CASADA");
                tb.Property(col => col.CuiEsposa).HasColumnName("CUI_ESPOSA");
                tb.Property(col => col.FNacimientoEsposa).HasColumnName("FNACIMIENTO_ESPOSA");

                tb.Property(col => col.EstadoCivil).HasColumnName("ESTADO_CIVIL");
                tb.Property(col => col.Embarazada).HasColumnName("EMBARAZADA");
                tb.Property(col => col.NiñoDA).HasColumnName("NIÑO_DA");
                tb.Property(col => col.GrupoEtnico).HasColumnName("GRUPO_ETNICO");
                tb.Property(col => col.Hombre0a2Años).HasColumnName("HOMBRE_0A2AÑOS");
                tb.Property(col => col.Mujer0a2Años).HasColumnName("MUJER_0A2AÑOS");
                tb.Property(col => col.Hombre2a5Años).HasColumnName("HOMBRE_2A5AÑOS");
                tb.Property(col => col.Mujer2a5Años).HasColumnName("MUJER_2A5AÑOS");
                tb.Property(col => col.Hombre6a17Años).HasColumnName("HOMBRE_6A17AÑOS");
                tb.Property(col => col.Mujer6a17Años).HasColumnName("MUJER_6A17AÑOS");

                tb.Property(col => col.TipoPersona).HasColumnName("TIPO_PERSONA");
                tb.Property(col => col.AreaMaiz).HasColumnName("AREA_MAIZ");
                tb.Property(col => col.QuintalesMaiz).HasColumnName("QUINTALES_MAIZ");
                tb.Property(col => col.AreaFrijol).HasColumnName("AREA_FRIJOL");
                tb.Property(col => col.QuintalesFrijol).HasColumnName("QUINTALES_FRIJOL");

                tb.Property(col => col.TipoAgriculturaPractica).HasColumnName("TIPO_AGRICULTURA_PRACTICA");
                tb.Property(col => col.NombreCOCODE).HasColumnName("NOMBRE_COCODE");

                tb.Property(col => col.EstadoFamilia).HasColumnName("ESTADO_FAMILIA");
                tb.Property(col => col.CantPersonasHogar).HasColumnName("CANTIDAD_PERSONAS_HOGAR");
                tb.Property(col => col.CantHombres).HasColumnName("CANTIDAD_HOMBRES");
                tb.Property(col => col.CantMujeres).HasColumnName("CANTIDAD_MUJERES");
                tb.Property(col => col.DañoFamiliar).HasColumnName("DANO_FAMILIAR");
                tb.Property(col => col.DañoVivienda).HasColumnName("DANO_VIVIENDA");
                tb.Property(col => col.PerdidaCultivo).HasColumnName("PERDIDA_CULTIVO");
                tb.Property(col => col.PerdidaAnimalesCrianza).HasColumnName("PERDIDA_ANIMALES_CRIANZA");
                tb.Property(col => col.UbicacionAlbergue).HasColumnName("UBICACION_ALBERGUE");

                tb.Property(col => col.NombreCompletoPersona1).HasColumnName("NOMBRECOMPLETO_PERSONA1");
                tb.Property(col => col.CuiPersona1).HasColumnName("CUI_PERSONA1");
                tb.Property(col => col.EdadPersona1).HasColumnName("EDAD_PERSONA1");
                tb.Property(col => col.GeneroPersona1).HasColumnName("GENERO_PERSONA1");
                tb.Property(col => col.Nombre1Persona1).HasColumnName("NOMBRE1PERSONA1");
                tb.Property(col => col.Nombre2Persona1).HasColumnName("NOMBRE2PERSONA1");
                tb.Property(col => col.Nombre3Persona1).HasColumnName("NOMBRE3PERSONA1");
                tb.Property(col => col.Apellido1Persona1).HasColumnName("APELLIDO1PERSONA1");
                tb.Property(col => col.Apellido2Persona1).HasColumnName("APELLIDO2PERSONA1");
                tb.Property(col => col.ApellidoCasadaPersona1).HasColumnName("APELLIDOCASADAPERSONA1");
                tb.Property(col => col.FNacimientoPersona1).HasColumnName("FNACIMIENTOPERSONA1");

                tb.Property(col => col.NombreCompletoPersona2).HasColumnName("NOMBRECOMPLETO_PERSONA2");
                tb.Property(col => col.CuiPersona2).HasColumnName("CUI_PERSONA2");
                tb.Property(col => col.EdadPersona2).HasColumnName("EDAD_PERSONA2");
                tb.Property(col => col.GeneroPersona2).HasColumnName("GENERO_PERSONA2");
                tb.Property(col => col.Nombre1Persona2).HasColumnName("NOMBRE1PERSONA2");
                tb.Property(col => col.Nombre2Persona2).HasColumnName("NOMBRE2PERSONA2");
                tb.Property(col => col.Nombre3Persona2).HasColumnName("NOMBRE3PERSONA2");
                tb.Property(col => col.Apellido1Persona2).HasColumnName("APELLIDO1PERSONA2");
                tb.Property(col => col.Apellido2Persona2).HasColumnName("APELLIDO2PERSONA2");
                tb.Property(col => col.ApellidoCasadaPersona2).HasColumnName("APELLIDOCASADAPERSONA2");
                tb.Property(col => col.FNacimientoPersona2).HasColumnName("FNACIMIENTOPERSONA2");

                tb.Property(col => col.CodigoNiño).HasColumnName("CODIGONIÑO");
                tb.Property(col => col.Nombre1Niño).HasColumnName("NOMBRE1NIÑO");
                tb.Property(col => col.Nombre2Niño).HasColumnName("NOMBRE2NIÑO");
                tb.Property(col => col.Nombre3Niño).HasColumnName("NOMBRE3NIÑO");
                tb.Property(col => col.Apellido1Niño).HasColumnName("APELLIDO1NIÑO");
                tb.Property(col => col.Apellido2Niño).HasColumnName("APELLIDO2NIÑO");
                tb.Property(col => col.CuiNiño).HasColumnName("CUINIÑO");
                tb.Property(col => col.Sexo).HasColumnName("SEXO");
                tb.Property(col => col.FNacimientoNiño).HasColumnName("FNACIMIENTONIÑO");
                tb.Property(col => col.EdadNiño).HasColumnName("EDADNIÑO");
                tb.Property(col => col.Diagnostico).HasColumnName("DIAGNOSTICO");
                tb.Property(col => col.Peso).HasColumnName("PESO");
                tb.Property(col => col.Talla).HasColumnName("TALLA");
                tb.Property(col => col.Parentesco).HasColumnName("PARENTESCO");
                tb.Property(col => col.CantNiños).HasColumnName("CANTNIÑOS");
                tb.Property(col => col.CantNiñas).HasColumnName("CANTNIÑAS");


                tb.Property(col => col.NombrePersonaRegistro).HasColumnName("NOMBRE_PERSONA_REGISTRO");
                tb.Property(col => col.InstitucionRepresenta).HasColumnName("INSTITUCION_REPRESENTA");

                tb.Property(col => col.IdAnulacion).HasColumnName("IDANULACION");
                tb.Property(col => col.Proceso).HasColumnName("PROCESO");  //En que proceso se anulo

                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
                tb.HasOne(d => d.Solicitud)
                  .WithMany(s => s.DetallesSolicitud)
                  .HasForeignKey(d => d.IdSolicitud)
                  .OnDelete(DeleteBehavior.Cascade);
                tb.HasOne(c => c.Catalogo)
                  .WithMany()
                  .HasForeignKey(c => c.IdAnulacion); // Clave foránea hacia la tabla Catalogo
            });

            modelBuilder.Entity<Nino>(tb =>
            {
                tb.ToTable("NINO");
                tb.HasKey(col => col.IdNino);
                tb.Property(col => col.IdNino).HasColumnName("IDNINO");
                tb.Property(col => col.IdDetalleSolicitud).HasColumnName("IDDETALLESOLICITUD");
                tb.Property(col => col.NombreCompleto).HasColumnName("NOMBRECOMPLETO");
                tb.Property(col => col.Nombre1).HasColumnName("NOMBRE1");
                tb.Property(col => col.Nombre2).HasColumnName("NOMBRE2");
                tb.Property(col => col.Nombre3).HasColumnName("NOMBRE3");
                tb.Property(col => col.Apellido1).HasColumnName("APELLIDO1");
                tb.Property(col => col.Apellido2).HasColumnName("APELLIDO2");
                tb.Property(col => col.FechaNacimiento).HasColumnName("FECHANACIMIENTO");
                tb.Property(col => col.CUI).HasColumnName("CUI");
                tb.Property(col => col.Edad).HasColumnName("EDAD");
                tb.HasOne(n => n.DetalleSolicitud)
                  .WithMany(d => d.Ninos)
                  .HasForeignKey(n => n.IdDetalleSolicitud)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleSolicitudAPA>(tb =>
            {
                tb.ToTable("DETALLESOLICITUD_DAPA");
                tb.HasKey(col => col.IdDetalleSolicitud);
                tb.HasIndex(col => col.IdDetalleSolicitud).IsUnique();
                tb.Property(col => col.IdDetalleSolicitud).ValueGeneratedOnAdd().HasColumnName("IDDETALLESOLICITUD");
                tb.Property(col => col.IdSolicitud).HasColumnName("IDSOLICITUD");
                tb.Property(col => col.Correlativo).HasColumnName("CORRELATIVO");
                //tb.Property(col => col.IdIntervencion).HasColumnName("IDINTERVENCION");
                tb.Property(col => col.IdDepartamento).HasColumnName("IDDEPARTAMENTO");
                tb.Property(col => col.IdMunicipio).HasColumnName("IDMUNICIPIO");
                tb.Property(col => col.Comunidad).HasMaxLength(250).HasColumnName("COMUNIDAD");
                tb.Property(col => col.Nombre1Esposo).HasColumnName("NOMBRE1_ESPOSO");
                tb.Property(col => col.Nombre2Esposo).HasColumnName("NOMBRE2_ESPOSO");
                tb.Property(col => col.Nombre3Esposo).HasColumnName("NOMBRE3_ESPOSO");
                tb.Property(col => col.Apellido1Esposo).HasColumnName("APELLIDO1_ESPOSO");
                tb.Property(col => col.Apellido2Esposo).HasColumnName("APELLIDO2_ESPOSO");
                tb.Property(col => col.CuiEsposo).HasColumnName("CUI_ESPOSO");
                tb.Property(col => col.FNacimientoEsposo).HasColumnName("FNACIMIENTO_ESPOSO");

                tb.Property(col => col.Nombre1Esposa).HasColumnName("NOMBRE1_ESPOSA");
                tb.Property(col => col.Nombre2Esposa).HasColumnName("NOMBRE2_ESPOSA");
                tb.Property(col => col.Nombre3Esposa).HasColumnName("NOMBRE3_ESPOSA");
                tb.Property(col => col.Apellido1Esposa).HasColumnName("APELLIDO1_ESPOSA");
                tb.Property(col => col.Apellido2Esposa).HasColumnName("APELLIDO2_ESPOSA");
                tb.Property(col => col.ApellidoCasada).HasColumnName("APELLIDO_CASADA");
                tb.Property(col => col.CuiEsposa).HasColumnName("CUI_ESPOSA");
                tb.Property(col => col.FNacimientoEsposa).HasColumnName("FNACIMIENTO_ESPOSA");

                tb.Property(col => col.EstadoCivil).HasColumnName("ESTADO_CIVIL");
                tb.Property(col => col.Embarazada).HasColumnName("EMBARAZADA");
                tb.Property(col => col.NiñoDA).HasColumnName("NIÑO_DA");
                tb.Property(col => col.GrupoEtnico).HasColumnName("GRUPO_ETNICO");
                tb.Property(col => col.Hombre0a2Años).HasColumnName("HOMBRE_0A2AÑOS");
                tb.Property(col => col.Mujer0a2Años).HasColumnName("MUJER_0A2AÑOS");
                tb.Property(col => col.Hombre2a5Años).HasColumnName("HOMBRE_2A5AÑOS");
                tb.Property(col => col.Mujer2a5Años).HasColumnName("MUJER_2A5AÑOS");
                tb.Property(col => col.Hombre6a17Años).HasColumnName("HOMBRE_6A17AÑOS");
                tb.Property(col => col.Mujer6a17Años).HasColumnName("MUJER_6A17AÑOS");

                tb.Property(col => col.TipoPersona).HasColumnName("TIPO_PERSONA");
                tb.Property(col => col.AreaMaiz).HasColumnName("AREA_MAIZ");
                tb.Property(col => col.QuintalesMaiz).HasColumnName("QUINTALES_MAIZ");
                tb.Property(col => col.AreaFrijol).HasColumnName("AREA_FRIJOL");
                tb.Property(col => col.QuintalesFrijol).HasColumnName("QUINTALES_FRIJOL");

                tb.Property(col => col.TipoAgriculturaPractica).HasColumnName("TIPO_AGRICULTURA_PRACTICA");
                tb.Property(col => col.NombreCOCODE).HasColumnName("NOMBRE_COCODE");
                tb.Property(col => col.NombrePersonaRegistro).HasColumnName("NOMBRE_PERSONA_REGISTRO");
                tb.Property(col => col.InstitucionRepresenta).HasColumnName("INSTITUCION_REPRESENTA");

                tb.Property(col => col.Genero).HasColumnName("GENERO");

                tb.Property(col => col.IdAnulacion).HasColumnName("IDANULACION");
                tb.Property(col => col.Proceso).HasColumnName("PROCESO");  //En que proceso se anulo

                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
                tb.HasOne(d => d.Solicitud)
                  .WithMany(s => s.DetallesSolicitudAPA)
                  .HasForeignKey(d => d.IdSolicitud)
                  .OnDelete(DeleteBehavior.Cascade);                
                tb.HasOne(c => c.Catalogo)
                  .WithMany()
                  .HasForeignKey(c => c.IdAnulacion); // Clave foránea hacia la tabla Catalogo
            });

            modelBuilder.Entity<Catalogo>(tb =>
            {
                tb.ToTable("CATALOGO");
                tb.HasKey(col => col.IdCatalogo);
                tb.HasIndex(col => col.IdCatalogo).IsUnique();
                tb.Property(col => col.IdCatalogo).ValueGeneratedOnAdd().HasColumnName("IDCATALOGO");
                tb.Property(col => col.Descripcion).HasColumnName("DESCRIPCION");
                tb.Property(col => col.Tipo).HasColumnName("TIPO");
                tb.Property(col => col.Orden).HasColumnName("ORDEN");                
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");                
            });

            modelBuilder.Entity<DetalleEvaluacion>(tb =>
            {
                tb.ToTable("DETALLEEVALUACION");
                tb.HasKey(col => col.IdDetalleEvaluacion);
                tb.HasIndex(col => col.IdDetalleEvaluacion).IsUnique();
                tb.Property(col => col.IdDetalleEvaluacion).ValueGeneratedOnAdd().HasColumnName("IDDETALLEEVALUACION");
                tb.Property(col => col.IdSolicitud).HasColumnName("IDSOLICITUD");                
                tb.Property(col => col.IdTipoValidacion).HasColumnName("IDTIPOVALIDACION");
                tb.Property(col => col.Archivo).HasMaxLength(200).HasColumnName("ARCHIVO");
                tb.Property(col => col.Observaciones).HasMaxLength(3000).HasColumnName("OBSERVACIONES");
                tb.Property(col => col.Proceso).HasColumnName("PROCESO");
                tb.Property(col => col.FechaRegistro).HasColumnName("FECHA_REGISTRO");
                tb.Property(col => col.UsuarioRegistro).HasColumnName("USUARIO_REGISTRO");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");                
                tb.HasOne(d => d.Solicitud)
                  .WithMany(s => s.DetallesEvaluacion)
                  .HasForeignKey(d => d.IdSolicitud)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleEvaluacionAPA>(tb =>
            {
                tb.ToTable("DETALLEEVALUACION_DAPA");
                tb.HasKey(col => col.IdDetalleEvaluacion);
                tb.HasIndex(col => col.IdDetalleEvaluacion).IsUnique();
                tb.Property(col => col.IdDetalleEvaluacion).ValueGeneratedOnAdd().HasColumnName("IDDETALLEEVALUACION");
                tb.Property(col => col.IdSolicitud).HasColumnName("IDSOLICITUD");
                //tb.Property(col => col.Descripcion).HasMaxLength(500).HasColumnName("DESCRIPCION");
                tb.Property(col => col.IdTipoValidacion).HasColumnName("IDTIPOVALIDACION");
                tb.Property(col => col.Archivo).HasMaxLength(200).HasColumnName("ARCHIVO");
                tb.Property(col => col.Observaciones).HasMaxLength(3000).HasColumnName("OBSERVACIONES");
                tb.Property(col => col.Proceso).HasColumnName("PROCESO");
                tb.Property(col => col.FechaRegistro).HasColumnName("FECHA_REGISTRO");
                tb.Property(col => col.UsuarioRegistro).HasColumnName("USUARIO_REGISTRO");
                tb.Property(col => col.Restrictiva).HasColumnName("RESTRICTIVA");
                tb.HasOne(d => d.Solicitud)
                  .WithMany(s =>s.DetallesEvaluacionAPA)
                  .HasForeignKey(d => d.IdSolicitud)
                  .OnDelete(DeleteBehavior.Cascade);
            });


            // Módulo de Bodegas
            #region Estructura de Catálogos

            // SGI.BODEGA
            modelBuilder.Entity<Bodega>(tb =>
            {
                tb.ToTable("BODEGA");
                tb.HasKey(col => col.IdBodega);
                tb.HasIndex(col => col.IdBodega).IsUnique();
                tb.Property(col => col.IdBodega).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreBodega).HasColumnName("NOMBRE_BODEGA");
                tb.Property(col => col.DireccionBodega).HasColumnName("DIRECCION_BODEGA");
                tb.Property(col => col.IdTipoBodega).HasColumnName("TIPO_BODEGA_ID");
                tb.Property(col => col.IdDepartamento).HasColumnName("DEPARTMENTO_ID");
                tb.Property(col => col.IdMunicipio).HasColumnName("MUNICIPIO_ID");
                tb.Property(col => col.IdUsuario).HasColumnName("USER_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PROYECTO
            modelBuilder.Entity<Proyecto>(tb =>
            {
                tb.ToTable("PROYECTO");
                tb.HasKey(col => col.IdProyecto);
                tb.HasIndex(col => col.IdProyecto).IsUnique();
                tb.Property(col => col.IdProyecto).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreProyecto).HasColumnName("PROYECTO");
                tb.Property(col => col.Anio).HasColumnName("ANIO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });


            // SGI.TIPO_BODEGA
            modelBuilder.Entity<TipoBodega>(tb =>
            {
                tb.ToTable("TIPO_BODEGA");
                tb.HasKey(col => col.IdTipoBodega);
                tb.HasIndex(col => col.IdTipoBodega).IsUnique();
                tb.Property(col => col.IdTipoBodega).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreTipoBodega).HasColumnName("TIPO_BODEGA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.SUB_BODEGA
            modelBuilder.Entity<SubBodega>(tb =>
            {
                tb.ToTable("SUB_BODEGA");
                tb.HasKey(col => col.IdSubBodega);
                tb.HasIndex(col => col.IdSubBodega).IsUnique();
                tb.Property(col => col.IdSubBodega).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreSubBodega).HasColumnName("NOMBRE_SUB_BODEGA");
                tb.Property(col => col.IdBodega).HasColumnName("BODEGA_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PUESTO
            modelBuilder.Entity<Puesto>(tb =>
            {
                tb.ToTable("PUESTO");
                tb.HasKey(col => col.IdPuesto);
                tb.HasIndex(col => col.IdPuesto).IsUnique();
                tb.Property(col => col.IdPuesto).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombrePuesto).HasColumnName("NOMBRE_PUESTO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.RENGLON_PRESUPUESTARIO
            modelBuilder.Entity<RenglonPresupuestario>(tb =>
            {
                tb.ToTable("RENGLON_PRESUPUESTARIO");
                tb.HasKey(col => col.IdRenglonPresupuestario);
                tb.HasIndex(col => col.IdRenglonPresupuestario).IsUnique();
                tb.Property(col => col.IdRenglonPresupuestario).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.Renglon).HasColumnName("RENGLON_PRESUPUESTARIO");
                tb.Property(col => col.Descripcion).HasColumnName("DESCRIPCION");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PERSONAL_BODEGAS
            modelBuilder.Entity<PersonalBodega>(tb =>
            {
                tb.ToTable("PERSONAL_BODEGAS");
                tb.HasKey(col => col.IdPersonalBodega);
                tb.HasIndex(col => col.IdPersonalBodega).IsUnique();
                tb.Property(col => col.IdPersonalBodega).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.Dpi).HasColumnName("DPI");
                tb.Property(col => col.Nit).HasColumnName("NIT");
                tb.Property(col => col.Nombres).HasColumnName("NOMBRES");
                tb.Property(col => col.Apellidos).HasColumnName("APELLIDOS");
                tb.Property(col => col.IdPuesto).HasColumnName("PUESTO_ID");
                tb.Property(col => col.IdRenglonPresupuestario).HasColumnName("RENGLON_ID");
                tb.Property(col => col.Email).HasColumnName("EMAIL");
                tb.Property(col => col.Telefono).HasColumnName("TELEFONO");
                tb.Property(col => col.IdBodega).HasColumnName("BODEGA_ID");
                tb.Property(col => col.IdUsuario).HasColumnName("USER_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });


            // SGI.CONVENIOS
            modelBuilder.Entity<Convenio>(tb =>
            {
                tb.ToTable("CONVENIOS");
                tb.HasKey(col => col.IdConvenio);
                tb.HasIndex(col => col.IdConvenio).IsUnique();
                tb.Property(col => col.IdConvenio).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreConvenio).HasColumnName("CONVENIO");
                tb.Property(col => col.Descripcion).HasColumnName("DESCRIPCION");
                tb.Property(col => col.VidaUtil).HasColumnName("VIDA_UTIL");
                tb.Property(col => col.TotalRaciones).HasColumnName("TOTAL_RACIONES");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });


            // SGI.PROGRAMA
            modelBuilder.Entity<Programa>(tb =>
            {
                tb.ToTable("PROGRAMAS");
                tb.HasKey(col => col.IdPrograma);
                tb.HasIndex(col => col.IdPrograma).IsUnique();
                tb.Property(col => col.IdPrograma).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombrePrograma).HasColumnName("NOMBRE_PROGRAMA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PRODUCTO_ALIMENTICIO
            modelBuilder.Entity<ProductoAlimenticio>(tb =>
            {
                tb.ToTable("PRODUCTO_ALIMENTICIO");
                tb.HasKey(col => col.IdProductoAlimenticio);
                tb.HasIndex(col => col.IdProductoAlimenticio).IsUnique();
                tb.Property(col => col.IdProductoAlimenticio).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombrePA).HasColumnName("NOMBRE_PA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PRESENTACION_PRODUCTO_ALIMENTICIO
            modelBuilder.Entity<PresentacionPA>(tb =>
            {
                tb.ToTable("PRESENTACION_PRODUCTO_ALIMENTICIO");
                tb.HasKey(col => col.IdPresentacionPA);
                tb.HasIndex(col => col.IdPresentacionPA).IsUnique();
                tb.Property(col => col.IdPresentacionPA).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombrePresentacionPA).HasColumnName("PRESENTACION_PA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.TIPO_ASISTENCIA
            modelBuilder.Entity<TipoAsistencia>(tb =>
            {
                tb.ToTable("TIPO_ASISTENCIA");
                tb.HasKey(col => col.IdTipoAsistencia);
                tb.HasIndex(col => col.IdTipoAsistencia).IsUnique();
                tb.Property(col => col.IdTipoAsistencia).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreTipoAsistencia).HasColumnName("TIPO_ASISTENCIA");
                tb.Property(col => col.Abreviatura).HasColumnName("ABREVIATURA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.UNIDAD_MEDIDA
            modelBuilder.Entity<UnidadMedida>(tb =>
            {
                tb.ToTable("UNIDAD_MEDIDA");
                tb.HasKey(col => col.IdUnidadMedida);
                tb.HasIndex(col => col.IdUnidadMedida).IsUnique();
                tb.Property(col => col.IdUnidadMedida).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreUnidadMedida).HasColumnName("UNIDAD_MEDIDA");
                tb.Property(col => col.UMPlural).HasColumnName("UM_PLURAL");
                tb.Property(col => col.Abreviatura).HasColumnName("ABREVIATURA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.RACION_MAESTRO
            modelBuilder.Entity<RacionMaestro>(tb =>
            {
                tb.ToTable("RACION_MAESTRO");
                tb.HasKey(col => col.IdRacionMaestro);
                tb.HasIndex(col => col.IdRacionMaestro).IsUnique();
                tb.Property(col => col.IdRacionMaestro).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdPrograma).HasColumnName("PROGRAMA_ID");
                tb.Property(col => col.IdConvenio).HasColumnName("CONVENIO_ID");
                tb.Property(col => col.TotalKilocalorias).HasColumnName("TOTAL_KILOCALORIAS");
                tb.Property(col => col.TotalCarbohidratos).HasColumnName("TOTAL_CARBOHIDRATOS");
                tb.Property(col => col.TotalProteinas).HasColumnName("TOTAL_PROTEINAS");
                tb.Property(col => col.TotalGrasas).HasColumnName("TOTAL_GRASAS");
                tb.Property(col => col.PorcentajeKilocalorias).HasColumnName("PORCENTAJE_KILOCALORIAS");
                tb.Property(col => col.PorcentajeCarbohidratos).HasColumnName("PORCENTAJE_CARBOHIDRATOS");
                tb.Property(col => col.PorcentajeProteinas).HasColumnName("PORCENTAJE_PROTEINAS");
                tb.Property(col => col.PorcentajeGrasas).HasColumnName("PORCENTAJE_GRASAS");
                tb.Property(col => col.IdUsuario).HasColumnName("USER_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
                tb.Ignore(r => r.RacionDetalle);
            });

            // SGI.RACION_DETALLE
            modelBuilder.Entity<RacionDetalle>(tb =>
            {
                tb.ToTable("RACION_DETALLE");
                tb.HasKey(col => col.IdRacionDetalle);
                tb.HasIndex(col => col.IdRacionDetalle).IsUnique();
                tb.Property(col => col.IdRacionDetalle).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdRacionMaestro).HasColumnName("RACION_MAESTRO_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ID");
                tb.Property(col => col.CantidadMedida).HasColumnName("CANTIDAD_MEDIDA");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.Kilocalorias).HasColumnName("KILOCALORIAS");
                tb.Property(col => col.Carbohidratos).HasColumnName("CARBOHIDRATOS");
                tb.Property(col => col.Proteinas).HasColumnName("PROTEINAS");
                tb.Property(col => col.Grasas).HasColumnName("GRASAS");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.EQUIVALENCIAS_MEDIDAS
            modelBuilder.Entity<EquivalenciasMedida>(tb =>
            {
                tb.ToTable("EQUIVALENCIAS_MEDIDAS");
                tb.HasKey(col => col.IdEquivalenciasMedida);
                tb.HasIndex(col => col.IdEquivalenciasMedida).IsUnique();
                tb.Property(col => col.IdEquivalenciasMedida).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.CantidadDesde).HasColumnName("CANTIDAD_DESDE");
                tb.Property(col => col.IdUnidadMedidaDesde).HasColumnName("UNIDAD_MEDIDA_DESDE_ID");
                tb.Property(col => col.CantidadHasta).HasColumnName("CANTIDAD_HASTA");
                tb.Property(col => col.IdUnidadMedidaDesde).HasColumnName("UNIDAD_MEDIDA_HASTA_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.ESTIBA
            modelBuilder.Entity<Estiba>(tb =>
            {
                tb.ToTable("ESTIBA");
                tb.HasKey(col => col.IdEstiba);
                tb.HasIndex(col => col.IdEstiba).IsUnique();
                tb.Property(col => col.IdEstiba).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdBodega).HasColumnName("BODEGA_ID");
                tb.Property(col => col.IdSubBodega).HasColumnName("SUB_BODEGA_ID");
                tb.Property(col => col.CodigoEstiba).HasColumnName("CODIGO_ESTIBA");
                tb.Property(col => col.DescripcionEstiba).HasColumnName("DESCRIPCION_ESTIBA");
                tb.Property(col => col.IdProducto).HasColumnName("PRODUCTO_ID");
                tb.Property(col => col.MetrosCuadrados).HasColumnName("METROS_CUADRADOS");
                tb.Property(col => col.IdUsuario).HasColumnName("USER_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.TRANSPORTISTA
            modelBuilder.Entity<Transportista>(tb =>
            {
                tb.ToTable("TRANSPORTISTA");
                tb.HasKey(col => col.IdTransportista);
                tb.HasIndex(col => col.IdTransportista).IsUnique();
                tb.Property(col => col.IdTransportista).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreTransportista).HasColumnName("NOMBRE_TRANSPORTISTA");
                tb.Property(col => col.Telefono).HasColumnName("TELEFONO");
                tb.Property(col => col.NombreContacto).HasColumnName("NOMBRE_CONTACTO");
                tb.Property(col => col.IdUsuario).HasColumnName("USER_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.VEHICULO
            modelBuilder.Entity<Vehiculo>(tb =>
            {
                tb.ToTable("VEHICULO");
                tb.HasKey(col => col.IdVehiculo);
                tb.HasIndex(col => col.IdVehiculo).IsUnique();
                tb.Property(col => col.IdVehiculo).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdTransportista).HasColumnName("TRANSPORTISTA_ID");
                tb.Property(col => col.PlacaCabezal).HasColumnName("PLACA_CABEZAL");
                tb.Property(col => col.PlacaPlataforma).HasColumnName("PLACA_PLATAFORMA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PILOTO
            modelBuilder.Entity<Piloto>(tb =>
            {
                tb.ToTable("PILOTO");
                tb.HasKey(col => col.IdPiloto);
                tb.HasIndex(col => col.IdPiloto).IsUnique();
                tb.Property(col => col.IdPiloto).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdTransportista).HasColumnName("TRANSPORTISTA_ID");
                tb.Property(col => col.NombresPiloto).HasColumnName("NOMBRES_PILOTO");
                tb.Property(col => col.ApellidosPiloto).HasColumnName("APELLIDOS_PILOTO");
                tb.Property(col => col.NoLicencia).HasColumnName("NO_LICENCIA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.SUB_BODEGA
            modelBuilder.Entity<UsuarioPersona>(tb =>
            {
                tb.ToTable("USUARIO_PERSONA");
                tb.HasKey(col => col.IdUsuarioPersona);
                tb.HasIndex(col => col.IdUsuarioPersona).IsUnique();
                tb.Property(col => col.IdUsuarioPersona).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdUsuario).HasColumnName("USUARIO_ID");
                tb.Property(col => col.IdPersona).HasColumnName("PERSONA_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PUESTO
            modelBuilder.Entity<Proveedor>(tb =>
            {
                tb.ToTable("PROVEEDORES");
                tb.HasKey(col => col.IdProveedor);
                tb.HasIndex(col => col.IdProveedor).IsUnique();
                tb.Property(col => col.IdProveedor).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreProveedor).HasColumnName("PROVEEDOR");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });


            #endregion

            // Módulo Ingreso de Alimentos
            // Módulo Egreso de Alimentos
            // Módulo Reingreso de Alimentos
            // Módulo Junta Receptora
            #region Estructura de Administración de Bodegas

            #region Ingreso de Alimentos

            // SGI.DETALLE_WAYBILL
            modelBuilder.Entity<DetalleWaybill>(tb =>
            {
                tb.ToTable("DETALLE_WAYBILL");
                tb.HasKey(col => col.IdDetalleWaybill);
                tb.HasIndex(col => col.IdDetalleWaybill).IsUnique();
                tb.Property(col => col.IdDetalleWaybill).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdIngresoAlimentos).HasColumnName("INGRESO_ALIMENTOS_ID");
                tb.Property(col => col.NoBoleta).HasColumnName("NO_BOLETA");
                tb.Property(col => col.FechaBoleta).HasColumnName("FECHA_BOLETA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.TIPO_NOMBRAMIENTO
            modelBuilder.Entity<TipoNombramiento>(tb =>
            {
                tb.ToTable("TIPO_NOMBRAMIENTO");
                tb.HasKey(col => col.IdTipoNombramiento);
                tb.HasIndex(col => col.IdTipoNombramiento).IsUnique();
                tb.Property(col => col.IdTipoNombramiento).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NombreTipoNombramiento).HasColumnName("TIPO_NOMBRAMIENTO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.ESTADOS_INGRESO
            modelBuilder.Entity<EstadoIngresos>(tb =>
            {
                tb.ToTable("ESTADOS_INGRESO");
                tb.HasKey(col => col.IdEstadoIngresos);
                tb.HasIndex(col => col.IdEstadoIngresos).IsUnique();
                tb.Property(col => col.IdEstadoIngresos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.EstadoIngreso).HasColumnName("ESTADO_INGRESO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.BITACORA
            modelBuilder.Entity<Bitacora>(tb =>
            {
                tb.ToTable("BITACORA");
                tb.HasKey(col => col.IdBitacora);
                tb.HasIndex(col => col.IdBitacora).IsUnique();
                tb.Property(col => col.IdBitacora).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.Accion).HasColumnName("ACCION");
                tb.Property(col => col.IdRegistro).HasColumnName("REGISTRO_ID");
                tb.Property(col => col.NombreTabla).HasColumnName("NOMBRE_TABLA");
                tb.Property(col => col.IdUsuario).HasColumnName("USUARIO_ID");
                tb.Property(col => col.FechaHora).HasColumnName("FECHA_HORA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.INGRESO_ALIMENTOS
            modelBuilder.Entity<IngresoAlimentos>(tb =>
            {
                tb.ToTable("INGRESO_ALIMENTOS"); 
                tb.HasKey(col => col.IdIngresoAlimentos); 
                tb.HasIndex(col => col.IdIngresoAlimentos).IsUnique(); 
                tb.Property(col => col.IdIngresoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdPrograma).HasColumnName("PROGRAMA_ID");
                tb.Property(col => col.IdConvenio).HasColumnName("CONVENIO_ID");
                tb.Property(col => col.IdBodegaRecibe).HasColumnName("BODEGA_RECIBE_ID");
                tb.Property(col => col.IdJuntaReceptora).HasColumnName("JUNTA_RECEPTORA_ID");
                tb.Property(col => col.NoWaybill).HasColumnName("NO_WAYBILL");
                tb.Property(col => col.FechaWaybill).HasColumnName("FECHA_WAYBILL");
                tb.Property(col => col.NoLti).HasColumnName("NO_LTI");
                tb.Property(col => col.ArchivoWaybill).HasColumnName("ARCHIVO_WAYBILL");
                tb.Property(col => col.FirmaSelloEncargadoWaybill).HasColumnName("FIRMA_SELLO_ENCARGADO_WAYBILL");
                tb.Property(col => col.NombreResponsableIndeca).HasColumnName("NOMBRE_RESPONSABLE_INDECA");
                tb.Property(col => col.NoActa).HasColumnName("NO_ACTA");
                tb.Property(col => col.FolioInicialActa).HasColumnName("FOLIO_INICIAL_ACTA");
                tb.Property(col => col.FolioFinalActa).HasColumnName("FOLIO_FINAL_ACTA");
                tb.Property(col => col.DocumentoActa).HasColumnName("DOCUMENTO_ACTA");
                tb.Property(col => col.FirmaMiembrosJuntaActa).HasColumnName("FIRMA_MIEMBROS_JUNTA_ACTA");
                tb.Property(col => col.FirmaEncargadoBodegaActa).HasColumnName("FIRMA_ENCARGADO_BODEGA_ACTA");
                tb.Property(col => col.NoForma1h).HasColumnName("NO_FORMA_1H");
                tb.Property(col => col.FechaForma1h).HasColumnName("FECHA_FORMA_1H");
                tb.Property(col => col.IdProveedor1H).HasColumnName("PROVEEDOR_1H_ID");
                tb.Property(col => col.ArchivoWaybillFinanciero).HasColumnName("ARCHIVO_WAYBILL_FINANCIERO");
                tb.Property(col => col.ArchivoActaFinanciero).HasColumnName("ARCHIVO_ACTA_FINANCIERO");
                tb.Property(col => col.ArchivoForma1hFinanciero).HasColumnName("ARCHIVO_FORMA1H_FINANCIERO");
                tb.Property(col => col.ArchivoBoletaAnalisis).HasColumnName("ARCHIVO_BOLETA_ANALISIS");
                tb.Property(col => col.IdUsuarioJefeBodega).HasColumnName("USUARIO_JEFE_BODEGA_ID");
                tb.Property(col => col.IdEstadoIngreso).HasColumnName("ESTADO_INGRESO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.KARDEX
            modelBuilder.Entity<Kardex>(tb =>
            {
                tb.ToTable("KARDEX"); 
                tb.HasKey(col => col.IdKardex); 
                tb.HasIndex(col => col.IdKardex).IsUnique(); 
                tb.Property(col => col.IdKardex).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.FechaMovimiento).HasColumnName("FECHA_MOVIMIENTO");
                tb.Property(col => col.IdBodega).HasColumnName("BODEGA_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ALIMENTICIO_ID");
                tb.Property(col => col.Estiba).HasColumnName("ESTIBA");
                tb.Property(col => col.IdConvenio).HasColumnName("CONVENIO_ID");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.IdRegistro).HasColumnName("REGISTRO_ID");
                tb.Property(col => col.Tabla).HasColumnName("TABLA");
                tb.Property(col => col.Ingreso).HasColumnName("INGRESO");
                tb.Property(col => col.Egreso).HasColumnName("EGRESO");
                tb.Property(col => col.Saldo).HasColumnName("SALDO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.DETALLE_BOLETA_ANALISIS
            modelBuilder.Entity<DetalleBoletaAnalisis>(tb =>
            {
                tb.ToTable("DETALLE_BOLETA_ANALISIS");
                tb.HasKey(col => col.IdDetalleBoletaAnalisis);
                tb.HasIndex(col => col.IdDetalleBoletaAnalisis).IsUnique();
                tb.Property(col => col.IdDetalleBoletaAnalisis).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdIngresoAlimentos).HasColumnName("INGRESO_ALIMENTOS_ID");
                tb.Property(col => col.NoBoletaAnalisis).HasColumnName("NO_BOLETA_ANALISIS");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.PRODUCTOS_INGRESO
            modelBuilder.Entity<ProductosIngreso>(tb =>
            {
                tb.ToTable("PRODUCTOS_INGRESO"); 
                tb.HasKey(col => col.IdProductosIngreso); 
                tb.HasIndex(col => col.IdProductosIngreso).IsUnique(); 
                tb.Property(col => col.IdProductosIngreso).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdIngresoAlimentos).HasColumnName("INGRESO_ALIMENTOS_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ALIMENTICIO_ID");
                tb.Property(col => col.Estiba).HasColumnName("ESTIBA");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.CantidadRecibir).HasColumnName("CANTIDAD_RECIBIR");
                tb.Property(col => col.IdBoletaWaybill).HasColumnName("BOLETA_WAYBILL_ID");
                tb.Property(col => col.FechaIngreso).HasColumnName("FECHA_INGRESO");
                tb.Property(col => col.FechaVencimiento).HasColumnName("FECHA_VENCIMIENTO");
                tb.Property(col => col.FolioLibroAlmacen).HasColumnName("FOLIO_LIBRO_ALMACEN");
                tb.Property(col => col.PrecioUnidad).HasColumnName("PRECIO_UNIDAD");
                tb.Property(col => col.ValorTotal).HasColumnName("VALOR_TOTAL");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.MOVIMIENTOS_PRODUCTOS
            modelBuilder.Entity<MovimientosProductos>(tb =>
            {
                tb.ToTable("MOVIMIENTOS_PRODUCTOS"); 
                tb.HasKey(col => col.IdMovimientosProductos); 
                tb.HasIndex(col => col.IdMovimientosProductos).IsUnique(); 
                tb.Property(col => col.IdMovimientosProductos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdBodega).HasColumnName("BODEGA_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ALIMENTICIO_ID");
                tb.Property(col => col.Estiba).HasColumnName("ESTIBA");
                tb.Property(col => col.IdConvenio).HasColumnName("CONVENIO_ID");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.Existencias).HasColumnName("EXISTENCIAS");
                tb.Property(col => col.FechaVencimiento).HasColumnName("FECHA_VENCIMIENTO");
                tb.Property(col => col.IdRegistro).HasColumnName("REGISTRO_ID");
                tb.Property(col => col.Tabla).HasColumnName("TABLA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            #endregion

            #region Egreso de Alimentos

            // SGI.EGRESO_ALIMENTOS
            modelBuilder.Entity<EgresoAlimentos>(tb =>
            {
                tb.ToTable("EGRESO_ALIMENTOS");
                tb.HasKey(col => col.IdEgresoAlimentos);
                tb.HasIndex(col => col.IdEgresoAlimentos).IsUnique();
                tb.Property(col => col.IdEgresoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NoDespachoAlmacen).HasColumnName("NO_DESPACHO_ALMACEN");
                tb.Property(col => col.IdPrograma).HasColumnName("PROGRAMA_ID");
                tb.Property(col => col.IdConvenio).HasColumnName("CONVENIO_ID");
                tb.Property(col => col.FechaSolicitud).HasColumnName("FECHA_SOLICITUD");
                tb.Property(col => col.TotalRaciones).HasColumnName("TOTAL_RACIONES");
                tb.Property(col => col.IdTipoAsistencia).HasColumnName("TIPO_ASISTENCIA_ID");
                tb.Property(col => col.IdTipoSalidaAlimento).HasColumnName("TIPO_SALIDA_ALIMENTO_ID");
                tb.Property(col => col.IdDepartamentoCarga).HasColumnName("DEPARTAMENTO_CARGA_ID");
                tb.Property(col => col.IdBodegaCarga).HasColumnName("BODEGA_CARGA_ID");
                tb.Property(col => col.IdBodegaDestino).HasColumnName("BODEGA_DESTINO_ID");
                tb.Property(col => col.NoAutorizacion).HasColumnName("NO_AUTORIZACION");
                tb.Property(col => col.IdDepartamentoDestino).HasColumnName("DEPARTAMENTO_DESTINO_ID");
                tb.Property(col => col.IdMunicipioDestino).HasColumnName("MUNICIPIO_DESTINO_ID");
                tb.Property(col => col.DireccionFinal).HasColumnName("DIRECCION_FINAL");
                tb.Property(col => col.Comentarios).HasColumnName("COMENTARIOS");
                tb.Property(col => col.FirmaSelloDirectorVisan).HasColumnName("FIRMA_SELLO_DIRECTOR_VISAN");
                tb.Property(col => col.FirmaSelloJefeDepartamento).HasColumnName("FIRMA_SELLO_JEFE_DEPARTAMENTO");
                tb.Property(col => col.ObservacionesAutorizacion).HasColumnName("OBSERVACIONES_AUTORIZACION");
                tb.Property(col => col.ObservacionesRechazo).HasColumnName("OBSERVACIONES_RECHAZO");
                tb.Property(col => col.FirmaSelloJefeBodegas).HasColumnName("FIRMA_SELLO_JEFE_BODEGAS");
                tb.Property(col => col.ArchivoDespachoAlmacen).HasColumnName("ARCHIVO_DESPACHO_ALMACEN");
                tb.Property(col => col.ObservCondSanitariaTransp).HasColumnName("OBSERV_COND_SANITARIA_TRANSP");
                tb.Property(col => col.IdUsuarioLogistica).HasColumnName("USUARIO_LOGISTICA_ID");
                tb.Property(col => col.IdUsuarioEncargadoBodega).HasColumnName("USUARIO_ENCARGADO_BODEGA_ID");
                tb.Property(col => col.IdUsuarioRecibeTraslado).HasColumnName("USUARIO_RECIBE_TRASLADO_ID");
                tb.Property(col => col.IdEstadoEgreso).HasColumnName("ESTADO_EGRESO_ALIMENTOS");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.DETALLE_EGRESO_ALIMENTOS
            modelBuilder.Entity<DetalleEgresoAlimentos>(tb =>
            {
                tb.ToTable("DETALLE_EGRESO_ALIMENTOS");
                tb.HasKey(col => col.IdDetalleEgresoAlimentos);
                tb.HasIndex(col => col.IdDetalleEgresoAlimentos).IsUnique();
                tb.Property(col => col.IdDetalleEgresoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdEgresoAlimentos).HasColumnName("EGRESO_ALIMENTOS_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ID");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.NoUnidades).HasColumnName("NO_UNIDADES");
                tb.Property(col => col.EstibaEgreso).HasColumnName("ESTIBA_EGRESO");
                tb.Property(col => col.EstibaTraslado).HasColumnName("ESTIBA_TRASLADO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.SOLICITUD_SALIDA_ALIMENTOS
            modelBuilder.Entity<SolicitudSalidaAlimentos>(tb =>
            {
                tb.ToTable("SOLICITUD_SALIDA_ALIMENTOS");
                tb.HasKey(col => col.IdSolicitudSalidaAlimentos);
                tb.HasIndex(col => col.IdSolicitudSalidaAlimentos).IsUnique();
                tb.Property(col => col.IdSolicitudSalidaAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdEgresoAlimentos).HasColumnName("EGRESO_ALIMENTOS_ID");
                tb.Property(col => col.NoConstancia).HasColumnName("NO_CONSTANCIA");
                tb.Property(col => col.FechaDespacho).HasColumnName("FECHA_DESPACHO");
                tb.Property(col => col.IdPrograma).HasColumnName("PROGRAMA_ID");
                tb.Property(col => col.IdTransportista).HasColumnName("TRANSPORTISTA_ID");
                tb.Property(col => col.IdPiloto).HasColumnName("PILOTO_ID");
                tb.Property(col => col.IdVehiculo).HasColumnName("VEHICULO_ID");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.DETALLE_SALIDA_ALIMENTOS
            modelBuilder.Entity<DetalleSalidaAlimentos>(tb =>
            {
                tb.ToTable("DETALLE_SALIDA_ALIMENTOS");
                tb.HasKey(col => col.IdDetalleSalidaAlimentos);
                tb.HasIndex(col => col.IdDetalleSalidaAlimentos).IsUnique();
                tb.Property(col => col.IdDetalleSalidaAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdSolicitudSalidaAlimentos).HasColumnName("SOLICITUD_SALIDA_ALIMENTOS_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ID");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.CantidadSalida).HasColumnName("CANTIDAD_SALIDA");
                tb.Property(col => col.EstadoProducto).HasColumnName("ESTADO_PRODUCTO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.ESTADOS_EGRESO
            modelBuilder.Entity<EstadoEgresos>(tb =>
            {
                tb.ToTable("ESTADOS_EGRESO");
                tb.HasKey(col => col.IdEstadoEgresos);
                tb.HasIndex(col => col.IdEstadoEgresos).IsUnique();
                tb.Property(col => col.IdEstadoEgresos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.EstadoEgreso).HasColumnName("ESTADO_EGRESO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            #endregion

            #region Reingreso de Alimentos

            // SGI.REINGRESO_ALIMENTOS
            modelBuilder.Entity<ReingresoAlimentos>(tb =>
            {
                tb.ToTable("REINGRESO_ALIMENTOS");
                tb.HasKey(col => col.IdReingresoAlimentos);
                tb.HasIndex(col => col.IdReingresoAlimentos).IsUnique();
                tb.Property(col => col.IdReingresoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NoOficioReingreso).HasColumnName("NO_OFICIO_REINGRESO");
                tb.Property(col => col.IdPrograma).HasColumnName("PROGRAMA_ID");
                tb.Property(col => col.FechaSolicitud).HasColumnName("FECHA_SOLICITUD");
                tb.Property(col => col.ArchivoOficioReingreso).HasColumnName("ARCHIVO_OFICIO_REINGRESO");
                tb.Property(col => col.IdEgresoAlimentos).HasColumnName("EGRESO_ALIMENTOS_ID");
                tb.Property(col => col.ObservacionFisicaIndeca).HasColumnName("OBSERVACION_FISICA_INDECA");
                tb.Property(col => col.ArchivoDespachoAlmacenReingreso).HasColumnName("ARCHIVO_DESPACHO_ALMACEN_REINGRESO");
                tb.Property(col => col.IdUsuarioEncargado).HasColumnName("USUARIO_ENCARGADO_ID");
                tb.Property(col => col.IdEstadoReingreso).HasColumnName("ESTADO_REINGRESO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.ESTADOS_REINGRESO
            modelBuilder.Entity<EstadoReingresos>(tb =>
            {
                tb.ToTable("ESTADOS_REINGRESO");
                tb.HasKey(col => col.IdEstadoReingresos);
                tb.HasIndex(col => col.IdEstadoReingresos).IsUnique();
                tb.Property(col => col.IdEstadoReingresos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.EstadoReingreso).HasColumnName("ESTADO_REINGRESO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.DETALLE_REINGRESO_ALIMENTOS
            modelBuilder.Entity<DetalleReingresoAlimentos>(tb =>
            {
                tb.ToTable("DETALLE_REINGRESO_ALIMENTOS");
                tb.HasKey(col => col.IdDetalleReingresoAlimentos);
                tb.HasIndex(col => col.IdDetalleReingresoAlimentos).IsUnique();
                tb.Property(col => col.IdDetalleReingresoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdReingresoAlimentos).HasColumnName("REINGRESO_ALIMENTOS_ID");
                tb.Property(col => col.IdProductoAlimenticio).HasColumnName("PRODUCTO_ID");
                tb.Property(col => col.IdPresentacion).HasColumnName("PRESENTACION_ID");
                tb.Property(col => col.CantidadUnidad).HasColumnName("CANTIDAD_UNIDAD");
                tb.Property(col => col.IdUnidadMedida).HasColumnName("UNIDAD_MEDIDA_ID");
                tb.Property(col => col.Cantidad).HasColumnName("CANTIDAD");
                tb.Property(col => col.FechaVencimiento).HasColumnName("FECHA_VENCIMIENTO");
                tb.Property(col => col.Estiba).HasColumnName("ESTIBA");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            #endregion

            #region Traslado de Alimentos

            // SGI.TRASLADO_ALIMENTOS
            modelBuilder.Entity<TrasladoAlimentos>(tb =>
            {
                tb.ToTable("TRASLADO_ALIMENTOS");
                tb.HasKey(col => col.IdTrasladoAlimentos);
                tb.HasIndex(col => col.IdTrasladoAlimentos).IsUnique();
                tb.Property(col => col.IdTrasladoAlimentos).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdEgresoAlimentos).HasColumnName("EGRESO_ALIMENTOS_ID");
                tb.Property(col => col.AutorizaRecepcion).HasColumnName("AUTORIZA_RECEPCION");
                tb.Property(col => col.ArchivoDespachoAlmacenTraslado).HasColumnName("ARCHIVO_DESPACHO_ALMACEN_TRASLADO");
                tb.Property(col => col.IdEstadoTraslado).HasColumnName("ESTADO_TRASLADO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.ESTADOS_TRASLADO
            modelBuilder.Entity<EstadoTraslados>(tb =>
            {
                tb.ToTable("ESTADOS_TRASLADO");
                tb.HasKey(col => col.IdEstadoTraslados);
                tb.HasIndex(col => col.IdEstadoTraslados).IsUnique();
                tb.Property(col => col.IdEstadoTraslados).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.EstadoTraslado).HasColumnName("ESTADO_TRASLADO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            #endregion

            #region Registro de Junta Receptora

            // SGI.JUNTA_RECEPTORA
            modelBuilder.Entity<JuntaReceptora>(tb =>
            {
                tb.ToTable("JUNTA_RECEPTORA");
                tb.HasKey(col => col.IdJuntaReceptora);
                tb.HasIndex(col => col.IdJuntaReceptora).IsUnique();
                tb.Property(col => col.IdJuntaReceptora).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.NumeroNombramiento).HasColumnName("NUMERO_NOMBRAMIENTO");
                tb.Property(col => col.ArchivoNombramiento).HasColumnName("ARCHIVO_NOMBRAMIENTO");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            // SGI.DETALLE_JUNTA_RECEPTORA
            modelBuilder.Entity<DetalleJuntaReceptora>(tb =>
            {
                tb.ToTable("DETALLE_JUNTA_RECEPTORA");
                tb.HasKey(col => col.IdDetalleJuntaReceptora);
                tb.HasIndex(col => col.IdDetalleJuntaReceptora).IsUnique();
                tb.Property(col => col.IdDetalleJuntaReceptora).ValueGeneratedOnAdd().HasColumnName("ID");
                tb.Property(col => col.IdJuntaReceptora).HasColumnName("JUNTA_RECEPTORA_ID");
                tb.Property(col => col.NombreCompleto).HasColumnName("NOMBRE_COMPLETO");
                tb.Property(col => col.DPI).HasColumnName("NO_DPI");
                tb.Property(col => col.IdTipoNombramiento).HasColumnName("TIPO_NOMBRAMIENTO_ID");
                tb.Property(col => col.EstaPresente).HasColumnName("ESTA_PRESENTE");
                tb.Property(col => col.Estado).HasColumnName("ESTADO");
            });

            #endregion

            #endregion


        }
    }
}

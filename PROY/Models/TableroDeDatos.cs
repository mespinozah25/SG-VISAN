namespace PROY.Models.TableroDeDatos
{

    public class TableroConvenio
    {
        public string Convenio { get; set; }
        public string Descripcion { get; set; }
        public decimal Raciones { get; set; }
        public int Tipo { get; set; }
    }
    public class TableroConvenioAvance
    {
        public string Convenio { get; set; }
        public string Descripcion { get; set; }
        public decimal TotalConvenio { get; set; }
        public decimal RacionesRecibidas { get; set; }
        public decimal RacionesPendientesRecibir { get; set; }
        public decimal RacionesEntregadas { get; set; }
        public decimal RacionesPendientesEntregar { get; set; }
        public decimal PorcentajeEjecucion { get; set; }
    }
    public class TableroBodegaProducto
    {
        public string NombreBodega { get; set; }
        public string NombrePA { get; set; }
        public decimal Existencias { get; set; }
    }

    public class TableroRacion
    {
        public string NombrePrograma { get; set; }
        public decimal TotalRaciones { get; set; }
    }

    public class TableroAPATotalSolicitud
    {
        public decimal total_solicitudes { get; set; }
    }

    public class TableroAPATotalBeneficiarios
    {
        public decimal total_beneficiarios { get; set; }
    }

    public class TableroAPADepartamentoSolicitud
    {
        public string Departamento { get; set; }
        public decimal Total { get; set; }
    }

    public class TableroAPAMunicipioSolicitud
    {
        public string Municipio { get; set; }
        public decimal Total { get; set; }
    }


    public class TableroAPADepartamentoBeneficiario
    {
        public string Departamento { get; set; }
        public decimal Total { get; set; }
    }

    public class TableroAPAProyectoDepartamentoSolicitud
    {
        public string Proyecto { get; set; }
        public string Departamento { get; set; }
        public int Total { get; set; }
    }

    public class TableroAPAProyectoSolicitud
    {
        public string Proyecto { get; set; }
        public decimal Total { get; set; }
    }


    
}
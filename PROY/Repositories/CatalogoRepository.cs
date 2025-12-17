using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using System.Linq.Expressions;

namespace PROY.Repositories
{
    public class CatalogoRepository : ICatalogoRepository
    {
        private readonly AppDBContext _dbContext;
        //private readonly IEjecucionRepository _ejecucionRepository;

        public CatalogoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
            //_ejecucionRepository = ejecucionRepository;
        }
        public async Task<List<Catalogo>> GetAllCatalogos()
        {
            List<Catalogo> listado = await _dbContext.Catalogos
                                                     .Where(c => c.Restrictiva == 0)
                                                     .OrderBy(i => i.Tipo)
                                                     .ToListAsync();

            return listado;
        }
        public async Task<Catalogo> GetCatalogo(int id)
        {
            Catalogo catalogo_encontrado = await _dbContext.Catalogos.FirstOrDefaultAsync(c => c.IdCatalogo == id && c.Restrictiva == 0);

            return catalogo_encontrado;
        }

        public async Task<List<Catalogo>> GetCatalogo(string tipo)
        {
            List<Catalogo> lista_encontrada = await _dbContext.Catalogos.Where(c => c.Tipo == tipo && c.Restrictiva == 0).ToListAsync();

            return lista_encontrada;
        }

        public async Task<int> Cuenta()
        {
            int maxID = 0;
            try
            {               
                maxID = await _dbContext.Catalogos
                                        .Select(c => c.IdCatalogo)
                                        .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }


        public async Task<Catalogo> SaveCatalogo(Catalogo modelo)
        {
            await _dbContext.Catalogos.AddAsync(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Catalogo> UpdateCatalogo(Catalogo modelo)
        {
            _dbContext.Catalogos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }


        public JsonResult GetMunicipios(int orden)
        {
            var lista_encontrada = new List<dynamic>();

            switch (orden)
            {
                case 100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden>=101 && c.Orden<=117)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 201 && c.Orden <= 208)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 300:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 301 && c.Orden <= 316)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 400:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 401 && c.Orden <= 416)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 500:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 501 && c.Orden <= 514)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 600:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 601 && c.Orden <= 614)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 700:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 701 && c.Orden <= 719)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 800:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 801 && c.Orden <= 808)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 900:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >=901 && c.Orden <= 924)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1000:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >=1001 && c.Orden <= 1021)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1101 && c.Orden <= 1109)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1201 && c.Orden <= 1230)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1300:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1301 && c.Orden <= 1333)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1400:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1401 && c.Orden <= 1421)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1500:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1501 && c.Orden <= 1508)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1600:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1601 && c.Orden <= 1617)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1700:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1701 && c.Orden <= 1714)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1800:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1801 && c.Orden <= 1805)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1900:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1901 && c.Orden <= 1911)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2000:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2001 && c.Orden <= 2011)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2101 && c.Orden <= 2107)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2201 && c.Orden <= 2217)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                default:
                    // code block
                    break;
            }
            
            return new JsonResult(lista_encontrada);
        }

        public JsonResult GetMunicipiosFilter(int orden)
        {
            var lista_encontrada = new List<dynamic>();

            switch (orden)
            {
                case 100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 101 && c.Orden <= 117)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 201 && c.Orden <= 208)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 300:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 301 && c.Orden <= 316)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 400:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 401 && c.Orden <= 416)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 500:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 501 && c.Orden <= 514)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 600:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 601 && c.Orden <= 614)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 700:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 701 && c.Orden <= 719)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 800:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 801 && c.Orden <= 808)
                                                           .OrderBy(i => i.Descripcion)
                                                           .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                                           .ToList<Object>();
                    break;
                case 900:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 901 && c.Orden <= 924)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1000:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1001 && c.Orden <= 1021)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1101 && c.Orden <= 1109)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1201 && c.Orden <= 1230)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1300:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1301 && c.Orden <= 1333)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1400:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1401 && c.Orden <= 1421)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1500:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1501 && c.Orden <= 1508)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1600:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1601 && c.Orden <= 1617)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1700:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1701 && c.Orden <= 1714)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1800:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1801 && c.Orden <= 1805)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 1900:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 1901 && c.Orden <= 1911)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2000:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2001 && c.Orden <= 2011)
                                     .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2100:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2101 && c.Orden <= 2107)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                case 2200:
                    lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Orden >= 2201 && c.Orden <= 2217)
                                    .OrderBy(i => i.Descripcion)
                                    .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })
                                   .ToList<Object>();
                    break;
                default:
                    // code block
                    break;
            }

            return new JsonResult(lista_encontrada);
        }
        public JsonResult GetCat(string tipo)
        {
            var lista_encontrada = _dbContext.Catalogos.Where(c => c.Tipo == tipo && c.Restrictiva == 0)
                                   .OrderBy(i => i.Orden)
                                   .Select(i => new { Id = i.IdCatalogo, Descripcion = i.Descripcion })                                   
                                   .ToList();

            return new JsonResult(lista_encontrada);
        }

        public int GetOrdenDepto(int paramId)
        {
            Catalogo res = null;
            int deptoOrden = 0;

            try
            {
                res = _dbContext.Catalogos.Where(c => c.Tipo == "DEPARTAMENTO" && c.Restrictiva == 0 && c.IdCatalogo == paramId).FirstOrDefault();
            }
            catch
            {
                res = null;
            }

            try
            {
                deptoOrden = Convert.ToInt32(res.Orden);
            }
            catch
            {
                deptoOrden = 0;
            }

            return deptoOrden;
        }

        public async Task<int> GetDepartamento(string paramDepto)
        {
            Catalogo res= null;
            int deptoID = 0;

            try
            {
                res = await _dbContext.Catalogos.Where(c => c.Tipo == "DEPARTAMENTO" && c.Restrictiva == 0 && c.Descripcion == paramDepto.ToUpper()).FirstOrDefaultAsync();
            }
            catch
            {
                res = null;
            }

            try
            {
                deptoID = Convert.ToInt32(res.IdCatalogo);
            }
            catch
            {
                deptoID = 0;
            }            

            return deptoID;
        }

        public async Task<int> GetMunicipio(string paramDepto, string paramMuni){
            Catalogo resDepto=null;
            Catalogo resMuni=null;
            int muniID=0;

            try
            {
                resDepto = await _dbContext.Catalogos.Where(c => c.Tipo == "DEPARTAMENTO" && c.Restrictiva == 0 && c.Descripcion == paramDepto.ToUpper()).FirstOrDefaultAsync();
            }
            catch 
            { 
                resDepto=null;
            }

            try
            {
                //resMuni = await _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Descripcion == paramMuni.ToUpper() && c.Orden == resDepto.Orden).FirstOrDefaultAsync();
                resMuni = await _dbContext.Catalogos.Where(c => c.Tipo == "MUNICIPIO" && c.Restrictiva == 0 && c.Descripcion == paramMuni.ToUpper()).FirstOrDefaultAsync();
            }
            catch 
            { 
                resMuni=null;
            }

            try
            {
                muniID = Convert.ToInt32(resMuni.IdCatalogo);
            }
            catch 
            { 
                muniID=0;
            }            

            return muniID;
        }

        public async Task<int> GetItemCatalogo(string paramItem, string paramTipo)
        {
            Catalogo res = null;
            int itemID = 0;

            try
            {
                res = await _dbContext.Catalogos.Where(c => c.Tipo == paramTipo && c.Restrictiva == 0 && c.Descripcion == paramItem.ToUpper()).FirstOrDefaultAsync();
            }
            catch
            {
                res = null;
            }

            try
            {
                itemID = Convert.ToInt32(res.IdCatalogo);
            }
            catch
            {
                itemID = 0;
            }

            return itemID;
        }
    }
}

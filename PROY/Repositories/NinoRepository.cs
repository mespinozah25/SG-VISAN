using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using PROY.Repositories;

namespace PROY.Repositories
{
    public class NinoRepository : INinoRepository
    {
        private readonly AppDBContext _context;

        public NinoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task InsertarNinoAsync(Nino nino)
        {
            // Obtener último ID actual
            var ultimoId = await _context.Ninos
                .OrderByDescending(n => n.IdNino)
                .Select(n => n.IdNino)
                .FirstOrDefaultAsync();

            nino.IdNino = ultimoId + 1;

            await _context.Ninos.AddAsync(nino);
            await _context.SaveChangesAsync();
        }

    }
}



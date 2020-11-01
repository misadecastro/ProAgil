using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public ProAgilContext _context { get; }

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }        
        public void UpDate<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await  _context.SaveChangesAsync()) > 0;
        }

        #region Eventos
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();

        }
        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(c => c.DataEvento)
            .Where(p => p.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c => c.RedesSociais);

            if(includePalestrantes)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(c => c.DataEvento)
            .Where(p => p.Id.Equals(eventoId));

            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Palestrante
        public async Task<Palestrante[]> GetAllPalestranteAsyncByNome(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if(includeEventos)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            
            query = query.OrderBy(p => p.Nome)
            .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos= false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(c => c.RedesSociais);

            if(includeEventos)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            
            query = query.OrderBy(c => c.Nome)
            .Where(p => p.Id.Equals(palestranteId));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetAllPalestranteAsync(bool includeEventos= false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(c => c.RedesSociais);

            if(includeEventos)
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);

            return await query.FirstOrDefaultAsync();
        }
        #endregion
    }
}
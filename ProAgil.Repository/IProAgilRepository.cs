using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T : class;
         void UpDate<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChangesAsync();

         #region Eventos
         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
         Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantes);

         #endregion

         #region Palestrante
         Task<Palestrante[]> GetAllPalestranteAsyncByNome(string nome, bool includeEventos);
         Task<Palestrante> GetAPalestranteAsyncById(int palestranteId, bool includeEventos);

         #endregion
    }
}
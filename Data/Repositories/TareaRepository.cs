using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GestorTareas.Data.Repositories
{
    public sealed class TareaRepository : IGenericRepository<Tarea>
    {
        private const string _LocalStorageEntryKey = "ListaTareas";

        private LocalStorageRepository LocalStorageRepository { get; init; }

        private List<Tarea> Tareas { get; set; } = new();

        public TareaRepository(LocalStorageRepository localStorageRepository) => LocalStorageRepository = localStorageRepository;

        public async Task Add(Tarea o)
        {
            await GetAll();
            Tareas.Add(o);
            await SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<Tarea> o)
        {
            await GetAll();
            Tareas.AddRange(o);
            await SaveChangesAsync();
        }

        public async Task<Tarea> Update(Tarea o)
        {
            await GetAll();
            Tareas[Tareas.FindIndex(t => t.ID == o.ID)] = o;
            await SaveChangesAsync();
            return o;
        }

        public async Task<bool> Remove(Tarea o)
        {
            await GetAll();
            bool resultado = Tareas.Remove(Tareas.Single(t => t.ID == o.ID));
            await SaveChangesAsync();
            return resultado;
        }

        public async Task<Tarea?> Get(Func<Tarea, bool> predicate)
        {
            await GetAll();
            return Tareas.SingleOrDefault(predicate);
        }

        public async Task<List<Tarea>> GetAll()
        {
            List<Tarea>? tareas = await LocalStorageRepository.Get<List<Tarea>>(_LocalStorageEntryKey);
            if (tareas is null)
            {
                Tareas = new();
                await SaveChangesAsync();
            }
            else Tareas = tareas;
            return Tareas;
        }

        public async Task SaveChangesAsync() => await LocalStorageRepository.Save(_LocalStorageEntryKey, Tareas);
    }
}

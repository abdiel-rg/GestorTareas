using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorTareas.Data;

namespace GestorTareas.Pages
{
    public partial class ListaTareas : IDisposable
    {
        private List<Tarea> Tareas { get; set; } = new();

        private SortOrder CurrentSortOrder { get; set; } = SortOrder.Ascending;

        private async Task GetData()
        {
            Tareas = await tareaRepository.GetAll();
            SortTareas(CurrentSortOrder);
        }

        private void SortTareas(SortOrder sortOrder)
        {
            CurrentSortOrder = sortOrder;
            Tareas.Sort((a, b) =>
            {
                if (a.TiempoRestante.Finalizo) return -1;
                return (sortOrder switch
                {
                    SortOrder.Ascending => 1,
                    SortOrder.Descending => -1,
                    _ => 0,
                }) * a.TiempoRestante.CompareTo(b.TiempoRestante);
            });
        }

        private async Task RemoveTarea(Tarea tarea)
        {
            await tareaRepository.Remove(tarea);
            await GetData();
        }

        private bool keepLoopRunning = true;

        protected override async Task OnInitializedAsync()
        {
            await GetData();

            _ = InvokeAsync(async () =>
            {
                do
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    SortTareas(CurrentSortOrder);
                    StateHasChanged();
                } while (keepLoopRunning);
            });
        }

        public virtual void Dispose()
        {
            keepLoopRunning = false;
            GC.SuppressFinalize(this);
        }
    }
}
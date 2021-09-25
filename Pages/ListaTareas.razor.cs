using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorTareas.Data;
using GestorTareas.Data.Enums;

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
            Tareas.Sort((a, b) => true switch
            {
                true when a.TiempoRestante.Finalizo => -1,
                true when b.TiempoRestante.Finalizo => 1,
                _ => (int)sortOrder * a.TiempoRestante.CompareTo(b.TiempoRestante),
            });
        }

        private string FormatoHora { get; set; } = "HH";

        private async Task RemoveTarea(Tarea tarea)
        {
            await tareaRepository.Remove(tarea);
            await GetData();
        }

        private bool keepLoopRunning = true;

        protected override async Task OnInitializedAsync()
        {
            await GetData();
            FormatoHora = (await localStorageRepository.GetOrDefault("FormatoHora", "HH"))!;

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
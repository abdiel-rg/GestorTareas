using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorTareas.Data;

namespace GestorTareas.Pages
{
    public partial class ListaTareas
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
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    Tareas.Sort((a, b) => a.TiempoRestante.CompareTo(b.TiempoRestante));
                    break;
                case SortOrder.Descending:
                    Tareas.Sort((a, b) => b.TiempoRestante.CompareTo(a.TiempoRestante));
                    break;
                default:
                    break;
            }
        }

        private async Task RemoveTarea(Tarea tarea)
        {
            await tareaRepository.Remove(tarea);
            await GetData();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetData();

            _ = InvokeAsync(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    StateHasChanged();
                }
            });
        }
    }
}
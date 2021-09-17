using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorTareas.Data;

namespace GestorTareas.Pages
{
    public partial class ListaTareas
    {
        private List<Tarea> Tareas = new();
        private bool SortAscending = true;

        protected override bool ShouldRender() => true;

        private async Task GetData()
        {
            Tareas = await tareaRepository.GetAll();
        }

        private void SortTareas(bool changeCondition = false)
        {
            Tareas.Sort((a, b) => SortAscending ? a.TiempoRestante.CompareTo(b.TiempoRestante) : b.TiempoRestante.CompareTo(a.TiempoRestante));
            if (changeCondition) SortAscending = !SortAscending;
            StateHasChanged();
        }

        private async Task RemoveTarea(Tarea tarea)
        {
            await tareaRepository.Remove(tarea);
            await GetData();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetData();
            SortTareas(true);

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
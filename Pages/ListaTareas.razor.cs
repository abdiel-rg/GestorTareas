using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorTareas.Data;

namespace GestorTareas.Pages
{
    public partial class ListaTareas
    {
        private List<Tarea> Tareas { get; set; } = new();
        private bool SortAscending { get; set; } = false;

        private async Task GetData()
        {
            Tareas = await tareaRepository.GetAll();
            SortTareas();
        }

        private void SortTareas()
        {
            Tareas.Sort((a, b) => (SortAscending ? -1 : 1) * a.TiempoRestante.CompareTo(b.TiempoRestante));
            SortAscending = !SortAscending;
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
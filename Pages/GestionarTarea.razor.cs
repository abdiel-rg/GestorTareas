using GestorTareas.Data;
using GestorTareas.Data.Repositories;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorTareas.Pages
{
    public partial class GestionarTarea
    {
        private Tarea tarea = new();

        private string ButtonText => ID is null ? "Agregar Tarea" : "Editar Tarea";
        private string ButtonClass => ID is null ? "btn btn-primary" : "btn btn-success";
        private string ButtonIconClass => ID is null ? "oi oi-plus" : "oi oi-pencil";

        private Task<Tarea?> TareaABuscar => tareaRepository.Get(t => t.ID.ToString() == ID);

        [Parameter]
        public string? ID { get; set; }

        private async Task SaveTarea(Tarea tarea)
        {
            if ((await TareaABuscar) is not null) await tareaRepository.Update(tarea);
            else await tareaRepository.Add(tarea);
            Nav.NavigateTo(Nav.BaseUri);
        }

        protected override async Task OnInitializedAsync()
        {
            if (ID is not null)
                if ((await TareaABuscar) is null) Nav.NavigateTo("/");
                else tarea = (await TareaABuscar)!;
        }
    }
}

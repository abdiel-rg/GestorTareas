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

        private Task<Tarea?> TareaABuscar => tareaRepository.Get(t => t.ID == ParsedID);


        [Parameter]
        public string? ID { get; set; }

        private Guid ParsedID => ID is not null ? Guid.Parse(ID) : tarea.ID;

        private async Task SaveTarea(Tarea tarea)
        {
            if ((await TareaABuscar) is not null) await tareaRepository.Update(tarea);
            else await tareaRepository.Add(tarea);
            Nav.NavigateTo("/");
        }

        protected override async Task OnInitializedAsync()
        {
            if (ID is not null)
                if ((await TareaABuscar) is null) Nav.NavigateTo("/");
                else tarea = (await TareaABuscar)!;

            StateHasChanged();
        }
    }
}

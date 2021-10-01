using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using GestorTareas.Data;
using GestorTareas.Data.Repositories;
using System.Text.Json;
using System.IO;
using GestorTareas.Components;
using GestorTareas.Data.Enums;
using Microsoft.JSInterop;

namespace GestorTareas.Pages
{
    public partial class Opciones
    {
        private Toast? ToastComponent { get; set; }

        private IBrowserFile? BrowserFile { get; set; }

        private string? GeneratedFile { get; set; } = null;

        private string FormatoHoraInput { get; set; } = "HH";

        [JsonValidator(typeof(IEnumerable<Tarea>), ErrorMessage = "Invalid JSON")]
        public string ImportedJsonText { get; set; } = "";

        private OpcionImportar OpcionElegida { get; set; } = OpcionImportar.Archivo;

        private async Task EstablecerFormatoHora(FormatoHora formatoHora) => await localStorageRepository.Save("FormatoHora", formatoHora.ToString());

        private static List<Tarea> ProcesarDatos(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<Tarea>>(json)!;
            }
            catch (Exception)
            {
                return new();
            }
        }

        private static async Task<List<Tarea>> ProcesarDatos(Stream stream)
        {
            try
            {
                return (await JsonSerializer.DeserializeAsync<List<Tarea>>(stream))!;
            }
            catch (Exception)
            {
                return new();
            }
        }

        private async Task ImportarDatos()
        {
            List<Tarea> tareas;

            switch (true)
            {
                case true when !string.IsNullOrWhiteSpace(ImportedJsonText) && OpcionElegida == OpcionImportar.Texto:
                    tareas = ProcesarDatos(ImportedJsonText);
                    break;

                case true when BrowserFile is not null && OpcionElegida == OpcionImportar.Archivo:
                    tareas = await ProcesarDatos(BrowserFile!.OpenReadStream());
                    break;

                default:
                    await ToastComponent!.ShowToast();
                    return;
            }

            await localStorageRepository.Save(TareaRepository.LocalStorageEntryKey, tareas);
            Nav.NavigateTo(Nav.BaseUri);
        }

        private async Task ExportarDatos()
        {
            await ToastComponent!.HideToast();
            string? json = await localStorageRepository.GetRawJsonOrDefault(TareaRepository.LocalStorageEntryKey, "[]");
            byte[] buffer = json!.Select(c => (byte)c).ToArray();
            GeneratedFile = $"data:{System.Net.Mime.MediaTypeNames.Application.Json};base64,{Convert.ToBase64String(buffer)}";
        }

        protected override async Task OnInitializedAsync()
        {
            FormatoHoraInput = (await localStorageRepository.GetOrDefault<string>("FormatoHora"))!;
        }
    }
}

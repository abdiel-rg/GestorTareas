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

        private bool? DatosValidos { get; set; } = null;

        private IBrowserFile? BrowserFile { get; set; }

        private string? GeneratedFile { get; set; } = null;

        private string FormatoHoraInput { get; set; } = "HH";

        private async Task EstablecerFormatoHora(FormatoHora formatoHora) => await localStorageRepository.Save("FormatoHora", formatoHora.ToString());

        private async Task<List<Tarea>> ProcesarDatos(Stream stream)
        {
            try
            {
                DatosValidos = true;
                return (await JsonSerializer.DeserializeAsync<List<Tarea>>(stream))!;
            }
            catch (Exception)
            {
                DatosValidos = false;
                return new();
            }
        }

        private async Task ImportarDatos()
        {
            if (BrowserFile is null)
            {
                await ToastComponent!.ShowToast();
                return;
            }

            List<Tarea> tareas = await ProcesarDatos(BrowserFile!.OpenReadStream());

            if ((bool)DatosValidos!)
            {
                await localStorageRepository.Save(TareaRepository.LocalStorageEntryKey, tareas);
                Nav.NavigateTo(Nav.BaseUri);
            }
            else
            {
                await ToastComponent!.ShowToast();
            }
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
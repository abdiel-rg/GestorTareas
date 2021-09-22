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

namespace GestorTareas.Pages
{
    public partial class Opciones
    {
        private Toast? ToastComponent { get; set; }

        private bool? DatosValidos { get; set; } = null;

        private IBrowserFile? BrowserFile { get; set; }

        private string? GeneratedFile { get; set; } = null;


        private async Task<List<Tarea>> ProcesarDatos(Stream stream)
        {
            List<Tarea> tareas = new();
            try
            {
                DatosValidos = true;
                tareas =  (await JsonSerializer.DeserializeAsync<List<Tarea>>(stream))!;
                Console.WriteLine("Válido");
                return tareas;
            }
            catch (Exception)
            {
                DatosValidos = false;
                Console.WriteLine("Inválido");
                return new();
            }
        }

        private async Task ImportarDatos()
        {
            if(BrowserFile is null)
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
            string? json = await localStorageRepository.Get(TareaRepository.LocalStorageEntryKey);
            byte[]? buffer = json!.Select(c => (byte)c).ToArray();
            GeneratedFile = $"data:{System.Net.Mime.MediaTypeNames.Application.Json};base64,{Convert.ToBase64String(buffer)}";
        }
    }
}
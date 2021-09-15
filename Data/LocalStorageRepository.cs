using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestorTareas.Data
{
    public class LocalStorageRepository
    {
        private IJSRuntime JSRuntime { get; init; }

        public LocalStorageRepository(IJSRuntime jsRuntime) => JSRuntime = jsRuntime;

        public async Task Add<T>(string key, T value) => await JSRuntime.InvokeVoidAsync("SaveToLocalStorage", key, JsonSerializer.Serialize(value));

        public async Task<T> Get<T>(string key) => JsonSerializer.Deserialize<T>(await JSRuntime.InvokeAsync<string>("GetItemFromLocalStorage", key));
    }
}

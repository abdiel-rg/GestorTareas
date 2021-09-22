using Microsoft.AspNetCore.Components;
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

        public async Task Save<T>(string key, T value) => await JSRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));

        public async Task<T?> Get<T>(string key)
        {
            string json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json switch
            {
                "undefined" or null or "" => default,
                _ => JsonSerializer.Deserialize<T>(json!),
            };
        }

        public async Task<string?> Get(string key) => await JSRuntime.InvokeAsync<string>("localStorage.getItem", key);

        public async Task Remove(string key) => await JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}

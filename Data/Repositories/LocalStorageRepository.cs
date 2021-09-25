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

        public virtual async Task<string?> GetRawJsonOrDefault(string key, string? returnedValueIfError = default) => await JSRuntime.InvokeAsync<string?>("localStorage.getItem", key) ?? returnedValueIfError;

        public virtual async Task<T?> GetOrDefault<T>(string key, T? returnedValueIfError = default)
        {
            string? json = await GetRawJsonOrDefault(key);
            return json is null ? returnedValueIfError : JsonSerializer.Deserialize<T>(json!);
        }

        public virtual async Task<T?> GetOrDefault<T>(string key, T? returnedValueIfError = default, params string[] errorValues)
        {
            string? json = await GetRawJsonOrDefault(key);
            return (json is null || errorValues.Contains(json)) ? returnedValueIfError : JsonSerializer.Deserialize<T>(json!);
        }

        public virtual async Task Save<T>(string key, T value) => await JSRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));

        public virtual async Task Remove(string key) => await JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}

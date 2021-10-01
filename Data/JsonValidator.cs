using System;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace GestorTareas.Data
{
    public class JsonValidator : ValidationAttribute
    {

        public JsonValidator(Type typeToDeserialize) => TypeToDeserialize = typeToDeserialize;

        private Type TypeToDeserialize { get; set; } = typeof(object);

        public override bool IsValid(object? value)
        {
            try
            {
                if (value is null) return false;
                object? json = JsonSerializer.Deserialize(value.ToString()!, TypeToDeserialize);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => IsValid(value) ? ValidationResult.Success : new(ErrorMessage);
    }
}

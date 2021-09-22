using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using GestorTareas;
using GestorTareas.Shared;
using GestorTareas.Data;

namespace GestorTareas.Components
{
    public partial class Modal
    {
        [Parameter]
        public string ModalID { get; set; } = $"modal_{Guid.NewGuid().ToString().Replace("-", "")}";

        [Parameter]
        public bool ShowButton { get; set; } = true;

        [Parameter]
        public string ButtonText { get; set; } = "Button Text";

        [Parameter]
        public string? ModalTitle { get; set; } = "Modal Title";


        [Parameter]
        public EventCallback<MouseEventArgs> OnButtonClick { get; set; }


        [Parameter]
        public RenderFragment? ModalHeader { get; set; }

        [Parameter]
        public RenderFragment? ModalBody { get; set; }

        [Parameter]
        public RenderFragment? ModalFooter { get; set; }
    }
}
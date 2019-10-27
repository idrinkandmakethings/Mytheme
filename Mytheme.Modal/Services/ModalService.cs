using System;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Modal.Services
{
    public class ModalService : IModalService
    {
        public event Action<ModalResult> OnClose;

        internal event Action<string, RenderFragment, ModalParameters, ModalOptions> OnShow;

        public void Show(string title, Type contentType)
        {
            Show(title, contentType, new ModalParameters(), new ModalOptions());
        }

        public void Show(string title, Type contentType, ModalOptions options)
        {
            Show(title, contentType, new ModalParameters(), options);
        }

        public void Show(string title, Type contentType, ModalParameters parameters)
        {
            Show(title, contentType, parameters, new ModalOptions());
        }

        public void Show(string title, Type componentType, ModalParameters parameters, ModalOptions options)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType))
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var content = new RenderFragment(x => { x.OpenComponent(1, componentType); x.CloseComponent(); });

            OnShow?.Invoke(title, content, parameters, options);
        }

        public void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null) where T : ComponentBase
        {
            Show(title,
                typeof(T),
                parameters ?? new ModalParameters(),
                options ?? new ModalOptions());
        }

        public void Cancel()
        {
            OnClose?.Invoke(ModalResult.Cancel());
        }

        public void Close(ModalResult result)
        {
            OnClose?.Invoke(result);
        }
    }
}

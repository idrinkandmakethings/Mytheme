using System;
using Microsoft.AspNetCore.Components;
using Mytheme.Modal.Defaults;

namespace Mytheme.Modal.Services
{
    public class ModalService : IModalService
    {
        internal event Action<ModalResult> OnClose;
        public event Action CloseWindow;

        internal event Action<string, RenderFragment, ModalParameters, ModalOptions> OnShow;

        public void ShowInfoModal(string title, string message, ModalOptions options = null)
        {
            var parameters = new ModalParameters();
            parameters.Add("message", message);
            Show<InfoModal>(title, parameters,  options, null);
        }

        public void Show(string title, Type contentType, Action<ModalResult> callback = null)
        {
            Show(title, contentType, new ModalParameters(), new ModalOptions(), callback);
        }

        public void Show(string title, Type contentType, ModalOptions options, Action<ModalResult> callback = null)
        {
            Show(title, contentType, new ModalParameters(), options, callback);
        }

        public void Show(string title, Type contentType, ModalParameters parameters, Action<ModalResult> callback = null)
        {
            Show(title, contentType, parameters, new ModalOptions(), callback);
        }

        public void Show(string title, Type componentType, ModalParameters parameters, ModalOptions options, Action<ModalResult> callback)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType))
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            OnClose = callback;

            var content = new RenderFragment(x => { x.OpenComponent(1, componentType); x.CloseComponent(); });

            OnShow?.Invoke(title, content, parameters, options);
        }

        public void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null, Action<ModalResult> callback = null) where T : ComponentBase
        {
            Show(title,
                typeof(T),
                parameters ?? new ModalParameters(),
                options ?? new ModalOptions(),
                callback);
        }

        public void Cancel()
        {
            OnClose?.Invoke(ModalResult.Cancel());
            CloseWindow?.Invoke();
            OnClose = null;
        }

        public void Close(ModalResult result)
        {
            OnClose?.Invoke(result);
            CloseWindow?.Invoke();
            OnClose = null;
        }
    }
}

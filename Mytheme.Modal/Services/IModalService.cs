using System;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Modal.Services
{
    public interface IModalService
    {
        event Action<ModalResult> OnClose;

        void Show(string title, Type contentType);

        void Show(string title, Type contentType, ModalOptions options);

        void Show(string title, Type contentType, ModalParameters parameters);

        void Show(string title, Type componentType, ModalParameters parameters, ModalOptions options);

        void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null) where T : ComponentBase;

        void Cancel();

        void Close(ModalResult result);
    }
}

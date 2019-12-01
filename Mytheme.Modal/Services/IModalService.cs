using System;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Modal.Services
{
    public interface IModalService
    {
        event Action CloseWindow;

        void ShowInfoModal(string title, string message, ModalOptions options = null);

        void Show(string title, Type contentType, Action<ModalResult> callback = null);

        void Show(string title, Type contentType, ModalOptions options, Action<ModalResult> callback = null );

        void Show(string title, Type contentType, ModalParameters parameters, Action<ModalResult> callback = null);

        void Show(string title, Type componentType, ModalParameters parameters, ModalOptions options, Action<ModalResult> callback = null);

        void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null, Action<ModalResult> callback = null) where T : ComponentBase;

        void Cancel();

        void Close(ModalResult result);
    }
}

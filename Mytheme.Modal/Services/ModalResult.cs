using System;

namespace Mytheme.Modal.Services
{
    public class ModalResult
    {
        public object Data { get; }
        public Type DataType { get; set; }
        public bool Cancelled { get; set; }

        protected ModalResult(object data, Type dataType, bool cancelled)
        {
            Data = data;
            DataType = dataType;
            Cancelled = cancelled;
        }

        public static ModalResult Ok<T>(T result) => new ModalResult(result, typeof(T), false);

        public static ModalResult Cancel() => new ModalResult(default, typeof(object), true);
    }
}

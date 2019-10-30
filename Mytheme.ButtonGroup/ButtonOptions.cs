using System;

namespace Mytheme.ButtonGroup
{
    public class ButtonOptions
    {
        public string ButtonText { get; set; }
        public Action OnClick { get; set; }

        public ButtonOptions(string buttonText, Action onClick)
        {
            ButtonText = buttonText;
            OnClick = onClick;
        }
    }
}

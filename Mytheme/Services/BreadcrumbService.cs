using System;

namespace Mytheme.Services
{
    public class BreadcrumbService
    {
        public event Action<string> OnBreadCrumbChange;

        public void SetBreadCrumb(string text)
        {
            OnBreadCrumbChange?.Invoke(text);
        }
    }
}

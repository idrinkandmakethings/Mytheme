using System;

namespace Mytheme.Data
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

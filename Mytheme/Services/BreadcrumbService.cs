using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Services
{
    public class BreadcrumbService
    {
        public List<NavBarButton> NavBarButtons { get; set; }
        
        public event Action<string> OnBreadCrumbChange;
        public event Action OnNavbarButtonChange;

        private Queue<string> history;
        private string currentRoute;
        

        public BreadcrumbService()
        {
            NavBarButtons = new List<NavBarButton>();
            history = new Queue<string>();
        }

        public void SetBreadCrumb(string text, string route, bool addNavBack)
        {
            if (!string.IsNullOrEmpty(currentRoute) && addNavBack)
            {
                history.Enqueue(currentRoute);
            }

            currentRoute = route;
            
            OnBreadCrumbChange?.Invoke(text);
        }

        public void ClearHistory()
        {
            history.Clear();
        }

        public void SetNavBarButtons(List<NavBarButton> buttons)
        {
            NavBarButtons.Clear();
            NavBarButtons.AddRange(buttons);
            OnNavbarButtonChange?.Invoke();
        }
    }

    public class NavBarButton
    {
        public Action CallBack { get; set; }

        public SvgName Image { get; set; }
        public string Name { get; set; }

        public NavBarButton(string name, SvgName image, Action callBack)
        {
            Name = name;
            CallBack = callBack;
            Image = image;
        }

        public void InvokeCallBack()
        {
            CallBack?.Invoke();
        }

        public MarkupString ToMarkupString()
        {
            return new MarkupString("");
        }
    }
}

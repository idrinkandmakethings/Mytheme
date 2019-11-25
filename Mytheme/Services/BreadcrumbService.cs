using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Services
{
    public class BreadcrumbService
    {
        public List<NavBarButton> NavBarButtons { get; set; }
        
        public event Action<string> OnBreadCrumbChange;

        public BreadcrumbService()
        {
            NavBarButtons = new List<NavBarButton>();
        }

        public void SetBreadCrumb(string text)
        {
            OnBreadCrumbChange?.Invoke(text);
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

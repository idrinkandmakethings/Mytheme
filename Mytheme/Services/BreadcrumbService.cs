using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Mytheme.Models;

namespace Mytheme.Services
{
    public class BreadcrumbService
    {
        public List<NavBarButton> NavBarButtons { get; set; }
        
        public event Action<string> OnBreadCrumbChange;
        public event Action<string> OnCampaignSelected;
        public event Action<NavigationLink> OnNavigateToLink;
        
        public event Action OnNavbarButtonChange;

        private Stack<NavigationLink> history;
        private NavigationLink currentRoute;
        private bool addCurrentNavBack;

        private string currentCampaignId;

        public BreadcrumbService()
        {
            NavBarButtons = new List<NavBarButton>();
            history = new Stack<NavigationLink>();
        }

        public void SetBreadCrumb(string text, NavigationLink route, bool addNavBack)
        {
            if (addCurrentNavBack)
            {
                history.Push(currentRoute);
            }

            addCurrentNavBack = addNavBack;
            currentRoute = route;
            
            OnBreadCrumbChange?.Invoke(text);
        }

        public void CampaignSelected(string campaignId)
        {
            history.Clear();
            currentCampaignId = campaignId;
            OnCampaignSelected?.Invoke(campaignId);
        }

        public void RefreshIndex()
        {
            OnCampaignSelected?.Invoke(currentCampaignId);
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

        public void Navigate(NavigationLink link)
        {
            OnNavigateToLink?.Invoke(link);
        }

        public void NavBack()
        {
            if (history.TryPop(out var route))
            {
                addCurrentNavBack = false;
                Navigate(route);
            }
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

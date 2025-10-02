using Asce.Managers.UIs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UITabs : UIObject
    {
        [SerializeField] private List<TabView> _tabViews = new();

        [Space]
        [SerializeField] private bool _isShowFirstTabOnStart = true;

        public bool IsShowFirstTabOnStart
        {
            get => _isShowFirstTabOnStart;
            set => _isShowFirstTabOnStart = value;
        }

        private void Start()
        {
            // Register click events for each tab
            foreach (var tabView in _tabViews)
            {
                if (tabView.Tab == null || tabView.View == null) continue;
                tabView.Tab.onClick.AddListener(() => ShowTab(tabView));
            }

            if (IsShowFirstTabOnStart && _tabViews.Count > 0)
                ShowTab(_tabViews[0]);
        }

        /// <summary> Show the view of the selected tab and hide others. </summary>
        private void ShowTab(TabView selected)
        {
            foreach (TabView tabView in _tabViews)
            {
                if (tabView.View == null) continue;

                bool isEquals = tabView.Equals(selected);
                SetTabVisibility(tabView, isEquals);
            }
        }

        /// <summary> Show tab by index. </summary>
        public void ShowTabByIndex(int index)
        {
            if (index < 0 || index >= _tabViews.Count) return;
            ShowTab(_tabViews[index]);
        }

        /// <summary> Show tab by button reference. </summary>
        public void ShowTabByButton(Button button)
        {
            foreach (var tabView in _tabViews)
            {
                if (tabView.Tab == button)
                {
                    ShowTab(tabView);
                    return;
                }
            }
        }

        /// <summary> Close all tabs (hide all views). </summary>
        public void CloseAllTabs()
        {
            foreach (var tabView in _tabViews)
            {
                if (tabView.View == null) continue;
                SetTabVisibility(tabView, false);
            }
        }

        /// <summary> Get the currently visible tab view. </summary>
        /// <returns> Returns null if none is visible. </returns>
        public TabView? GetCurrentTab()
        {
            foreach (var tabView in _tabViews)
            {
                if (tabView.View == null) continue;

                UITabView tabComp = tabView.View.GetComponent<UITabView>();
                if (tabComp != null && tabComp.IsShow) return tabView;
                if (tabView.View.gameObject.activeSelf) return tabView;
            }
            return null;
        }

        /// <summary> Check if any tab view is visible. </summary>
        public bool IsAnyViewVisible()
        {
            foreach (var tabView in _tabViews)
            {
                if (tabView.View == null) continue;

                var tabComp = tabView.View.GetComponent<UITabView>();
                if (tabComp != null && tabComp.IsShow) return true;
                else if (tabView.View.gameObject.activeSelf) return true;
            }
            return false;
        }

        /// <summary>
        /// Handle showing/hiding a tab. Uses UITabView if available, otherwise fallbacks to SetActive.
        /// </summary>
        private void SetTabVisibility(TabView tabView, bool visible)
        {
            if (tabView.View.TryGetComponent(out UITabView tabComp))
            {
                tabComp.SetVisible(visible);
            }
            else tabView.View.gameObject.SetActive(visible);
        }
    }
}

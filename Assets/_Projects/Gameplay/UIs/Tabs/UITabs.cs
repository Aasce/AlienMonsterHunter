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
        [SerializeField] private int _currentTabIndex = -1;

        public List<TabView> TabViews => _tabViews;
        public int CurrentTab => _currentTabIndex;

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
            int index = _tabViews.IndexOf(selected);
            if (index < 0) return;
            if (index == _currentTabIndex) return;
            _currentTabIndex = index;

            for (int i = 0; i < _tabViews.Count; i++)
            {
                TabView tabView = _tabViews[i];
                if (tabView.View == null) continue;

                bool isEquals = i == index;
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
            if (button == null) return;
            foreach (var tabView in _tabViews)
            {
                if (tabView.Tab == button)
                {
                    ShowTab(tabView);
                    return;
                }
            }
        }

        public void ShowTabByTabView(RectTransform view)
        {
            if (view == null) return;
            foreach (var tabView in _tabViews)
            {
                if (tabView.View == view)
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
            _currentTabIndex = -1;
        }

        /// <summary> Get the currently visible tab view. </summary>
        /// <returns> Returns null if none is visible. </returns>
        public TabView? GetCurrentTab()
        {
            if (_currentTabIndex < 0) return null;
            return _tabViews[_currentTabIndex];
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

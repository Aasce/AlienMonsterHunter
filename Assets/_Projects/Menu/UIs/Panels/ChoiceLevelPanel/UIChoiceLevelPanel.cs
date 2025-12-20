using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.UIs;
using Asce.Game.UIs.Panels;
using Asce.MainMenu.Picks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIChoiceLevelPanel : UIFullScreenPanel
    {
        [SerializeField] protected UIChoiceMapLevelDetails _details;

        [Space]
        [SerializeField] protected Pool<UIChoiceMapItem> _mapPool = new();
        [SerializeField] protected Pool<UIMapLevelItem> _levelPool = new();

        [Space]
        [SerializeField] protected bool _showFirstMapLevels = true;

        [Header("Runtime")]
        [SerializeField, Readonly] protected Map _currentMap = null;

        protected IEnumerable<Map> Maps => GameManager.Instance.AllMaps.Maps;

        protected override void Reset()
        {
            base.Reset();
            _name = "Choice Level";
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _details);
        }

        public override void Initialize()
        {
            base.Initialize();
            _details.Initialize();
        }

        public override void Show()
        {
            base.Show();
            this.Refresh();

            if (_showFirstMapLevels)
            {
                if (GameManager.Instance.AllMaps.Maps.Count <= 0) return;
                this.ChoiceMap(GameManager.Instance.AllMaps.Maps[0]);
            }
        }

        protected virtual void Refresh()
        {
            _mapPool.Clear(onClear: (item) =>
            {
                item.ResetStatus();
                item.Hide();
            });
            foreach (Map item in Maps)
            {
                UIChoiceMapItem uiMap = _mapPool.Activate(out bool isCreated);
                if (isCreated) uiMap.Panel = this;

                uiMap.Set(item);

                uiMap.RectTransform.SetAsLastSibling();
                uiMap.Show();
            }
        }

        public void ChoiceMap(Map map, int level = 1)
        {
            if (map == null) return;
            _currentMap = map;

            _levelPool.Clear(onClear: (item) =>
            {
                item.ResetStatus();
                item.Hide();
            });
            foreach (SO_MapLevelInformation item in _currentMap.Information.Levels)
            {
                UIMapLevelItem uiLevel = _levelPool.Activate(out bool isCreated);
                if (isCreated) uiLevel.Panel = this;

                uiLevel.Set(item);

                uiLevel.RectTransform.SetAsLastSibling();
                uiLevel.Show();
            }

            UIChoiceMapItem uiMap = _mapPool.Activities.FirstOrDefault(item => item.Item == _currentMap);
            if (uiMap is IUIHighlightable highlightable) highlightable.Highlight();
            
            this.ChoiceLevel(level);
            PickController.Instance.PickMap(_currentMap, level);
        }

        public void ChoiceLevel(int level)
        {
            if (_currentMap == null) return;
            SO_MapLevelInformation levelInformation = _currentMap.Information.Levels.FirstOrDefault(i => i.Level == level);
            if (levelInformation == null) return;

            UIMapLevelItem uiLevel = _levelPool.Activities.FirstOrDefault(item => item.Item == levelInformation);
            if (uiLevel is IUIHighlightable highlightable) highlightable.Highlight();

            _details.Set(levelInformation);
            _details.Show();
            PickController.Instance.PickMap(_currentMap, level);
        }


    }
}

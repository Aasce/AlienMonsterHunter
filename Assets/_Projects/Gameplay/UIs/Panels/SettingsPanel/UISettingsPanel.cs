using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public class UISettingsPanel : UIWindowPanel
    {
        [SerializeField] private List<UISettingContent> _contents = new();

        protected override void Reset()
        {
            base.Reset();
            _name = "Settings";
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.GetComponentsInChildren(_contents);
        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (UISettingContent content in _contents)
            {
                if (content == null) continue;
                content.Panel = this;
                content.Initialize();
            }
        }

    }
}

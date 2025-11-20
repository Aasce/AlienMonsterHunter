using Asce.Game.Supports;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.UIs
{
    public class UISupportsInformation : UIObject
    {
        [SerializeField] private Pool<UISupportItem> _pool = new();
        [SerializeField, Readonly] private SupportCaller _caller;
        [SerializeField, Readonly] private List<KeyCode> _callKeys = new();

        public SupportCaller Caller
        {
            get => _caller;
            set
            {
                if (_caller == value) return;
                this.Unregister();
                _caller = value;
                this.Register();
            }
        }

        private void Register()
        {
            if (_caller == null) return;

            this.SetSupport();
            _caller.OnSupportChanged += Caller_OnSupportChanged;
        }

        private void Unregister()
        {
            if (_caller == null) return;
            _caller.OnSupportChanged -= Caller_OnSupportChanged;
        }

        public void SetCallKeys(List<KeyCode> keys)
        {
            if (keys == null) return;
            _callKeys.Clear();
            _callKeys.AddRange(keys);
        }

        private void SetSupport()
        {
            _pool.Clear(onClear: (item) => item.Hide());
            if (Caller == null) return;
            for (int i = 0; i < Caller.Supports.Count; i++)
            {
                SupportContainer container = Caller.Supports[i];
                KeyCode callKey = this.GetKey(i);

                UISupportItem uiItem = _pool.Activate(out bool isCreated);
                if (uiItem == null) continue;

                if (isCreated) uiItem.SupportsInformation = this;
                uiItem.SetKey(callKey);
                uiItem.Container = container;
                uiItem.RectTransform.SetAsLastSibling();
                uiItem.Show();
            }
        }

        private KeyCode GetKey(int index)
        {
            if (index < 0 || index >= _callKeys.Count) return KeyCode.None;
            return _callKeys[index];
        }

        private void Caller_OnSupportChanged()
        {
            this.SetSupport();
        }

    }
}

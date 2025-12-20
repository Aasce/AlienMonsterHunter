using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UISFX_ButtonClick : UIComponent, IPointerClickHandler
    {
        [SerializeField, Readonly] private Button _button;
        [SerializeField] private SFXPlayer _sfxSuccessPlayer;
        [SerializeField] private SFXPlayer _sfxFailPlayer;

        override protected void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _button);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_button == null) return;
            if (_button.interactable)
            {
                if (_sfxSuccessPlayer != null)
                    _sfxSuccessPlayer.Play();
            }
            else
            {
                if (_sfxFailPlayer != null) 
                    _sfxFailPlayer.Play();
            }
        }
    }
}

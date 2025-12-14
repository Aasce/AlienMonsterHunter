using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.ResultGame.UIs.HUDs
{
    public class UIAwards : UIComponent
    {
        [SerializeField] private Pool<UIAwardItem> _pool = new();

        public void Initialize()
        {

        }

        public void Ready()
        {
            foreach(Award award in ResultGameManager.Instance.AwardController.Awards)
            {
                UIAwardItem uiAward = _pool.Activate();
                if (uiAward == null) continue;

                uiAward.Set(award);
                uiAward.RectTransform.SetAsLastSibling();
                uiAward.Show();
            }
        }
    }
}

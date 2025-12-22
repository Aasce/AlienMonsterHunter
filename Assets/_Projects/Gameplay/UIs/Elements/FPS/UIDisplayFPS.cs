using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Managers.Performance;
using TMPro;
using UnityEngine;

namespace Asce.Game.UIs.Elements
{
    public class UIDisplayFPS : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _fpsText;

        [Header("Configs")]
        [SerializeField] private Cooldown _updateCooldown = new(0.2f);

        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (!_updateCooldown.IsComplete) return;
            _updateCooldown.Reset();

            _fpsText.text = $"FPS: {Mathf.RoundToInt(FPSCounter.CurrentFPS)}";
        }
    }
}

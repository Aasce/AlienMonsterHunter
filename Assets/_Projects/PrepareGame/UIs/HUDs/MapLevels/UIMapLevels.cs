using Asce.Core.UIs;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Progress;
using TMPro;
using UnityEngine;

namespace Asce.PrepareGame.UIs.HUDs
{
    public class UIMapLevels : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        private void Start()
        {
            if (!GameManager.Instance.Shared.TryGet("MapLevel", out PickMapLevelShareData mapLevelData))
            {
                Debug.LogError("[NewGameController] Map Level Share Data is null", this);
            }

            Map mapPrefab = GameManager.Instance.AllMaps.Get(mapLevelData.MapName);
            SO_MapLevelInformation levelInformation = mapPrefab.Information.GetLevel(mapLevelData.Level);

            _nameText.text = mapPrefab.Information.Name;
            _levelText.text = $"Level {levelInformation.Level}";
        }
    }
}

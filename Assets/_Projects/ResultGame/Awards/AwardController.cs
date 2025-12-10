using Asce.Core;
using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Players;
using Asce.Game.Progress;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.ResultGame
{
    public class AwardController : ControllerComponent
    {
        [SerializeField] private List<MapLevelAward> _awards = new();
        public override string ControllerName => "Award";

        public List<MapLevelAward> Awards => _awards;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Ready()
        {
            base.Ready();
            _awards.AddRange(GetMapLevelAward());
            foreach (MapLevelAward award in _awards)
            {
                if (award == null) continue;

                SO_ItemInformation itemInformation = GameManager.Instance.AllItems.Get(award.ItemName);
                if (itemInformation == null) continue;

                if (itemInformation.Type == ItemType.Currency)
                {
                    PlayerManager.Instance.Currencies.Add(itemInformation.Name, award.Quantity);
                }
            }
        }

        private List<MapLevelAward> GetMapLevelAward()
        {
            List<MapLevelAward> awards = new();
            GameResultType type = ResultGameManager.Instance.ResultData.FinalResult;
            PickMapLevelShareData pickMapLevel = GameManager.Instance.Shared.Get<PickMapLevelShareData>("MapLevel");
            if (pickMapLevel == null) return awards;

            Map mapPrefab = GameManager.Instance.AllMaps.Get(pickMapLevel.MapName);
            if (mapPrefab == null) return awards;

            SO_MapLevelInformation levelInformation = mapPrefab.Information.GetLevel(pickMapLevel.Level);
            if (levelInformation == null) return awards;

            bool isFirstWin = false;

            switch (type)
            {
                case GameResultType.Victory:
                    if (isFirstWin) awards.AddRange(levelInformation.FirstWinAwards);
                    else awards.AddRange(levelInformation.WinAwards);
                    break;

                case GameResultType.Defeat:
                    awards.AddRange(levelInformation.PlayAwards);
                    break;

                default:
                    break;
            }

            return awards;
        }
    }
}

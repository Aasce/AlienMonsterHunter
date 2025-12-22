using Asce.Core;
using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Players;
using Asce.Game.Progress;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.ResultGame
{
    public class AwardController : ControllerComponent
    {
        [SerializeField] private List<Award> _awards = new();
        public override string ControllerName => "Award";

        public List<Award> Awards => _awards;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Ready()
        {
            base.Ready();

            foreach (Award award in GetMapLevelAward())
                AddOrMergeAward(_awards, award);

            foreach (Award award in GetSpoilsAward())
                AddOrMergeAward(_awards, award);

            foreach (Award award in _awards)
            {
                if (award == null) continue;

                SO_ItemInformation itemInformation = GameManager.Instance.AllItems.Get(award.ItemName);
                if (itemInformation == null) continue;

                PlayerManager.Instance.Items.Add(itemInformation.Name, award.Quantity);
            }
        }

        private List<Award> GetMapLevelAward()
        {
            List<Award> awards = new();
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
                    if (isFirstWin) this.AddMapLevelAward(awards, levelInformation.FirstWinAwards);
                    else this.AddMapLevelAward(awards, levelInformation.WinAwards);
                    break;

                case GameResultType.Defeat:
                    this.AddMapLevelAward(awards, levelInformation.PlayAwards);
                    break;

                default:
                    break;
            }

            return awards;
        }

        private List<Award> GetSpoilsAward()
        {
            List<Award> awards = new();
            foreach (var spoil in ResultGameManager.Instance.ResultData.Spoils)
            {
                Award award = awards.FirstOrDefault(a => a != null && a.ItemName == spoil.Key);
                if (award == null)
                {
                    awards.Add(new Award(spoil.Key, spoil.Value));
                }
                else
                {
                    award.AddQuantity(spoil.Value);
                }
            }
            return awards;
        }

        private void AddMapLevelAward(List<Award> awards, IEnumerable<MapLevelAward> levelAwards)
        {
            if (awards == null || levelAwards == null) return;
            foreach (MapLevelAward levelAward in levelAwards)
            {
                if (levelAward == null) continue;

                Award award = awards.FirstOrDefault(a => a != null && a.ItemName == levelAward.ItemName);
                if (award == null)
                {
                    awards.Add(new Award(levelAward.ItemName, levelAward.Quantity));
                }
                else
                {
                    award.AddQuantity(levelAward.Quantity);
                }   
            }

        }

        private void AddOrMergeAward(List<Award> target, Award source)
        {
            if (target == null || source == null) return;

            Award existing = target.FirstOrDefault(a => a.ItemName == source.ItemName);
            if (existing == null)
            {
                target.Add(source);
            }
            else
            {
                existing.AddQuantity(source.Quantity);
            }
        }

    }
}

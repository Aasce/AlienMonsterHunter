using Asce.Game.Players;
using UnityEngine;

namespace Asce.Game.Progress
{
    [CreateAssetMenu(menuName = "Asce/Progress/Unlocks/Currency Payment", fileName = "Currency Payment")]
    public class SO_UnlockWithCurrency : SO_UnlockCondition
    {
        [Header("Currency Payment")]
        [SerializeField] private string _currencyName = string.Empty;
        [SerializeField] private int _cost = 0;

        public string CurrencyName => _currencyName;
        public int Cost => _cost;

        private void Reset()
        {
            _name = "Currency Payment";
        }

        public override bool IsMet()
        {
            if (!Application.isPlaying) return false;
            if (PlayerManager.Instance == null) return false;
            return PlayerManager.Instance.Currencies.CanSpend(CurrencyName, Cost);
        }

        public override void Met()
        {
            if (PlayerManager.Instance == null) return;
            PlayerManager.Instance.Currencies.TrySpend(CurrencyName, Cost);
        }
    }
}
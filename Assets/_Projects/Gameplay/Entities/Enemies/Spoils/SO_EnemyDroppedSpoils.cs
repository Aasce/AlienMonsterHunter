using Asce.Core;
using Asce.Game.Items;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [CreateAssetMenu(menuName = "Asce/Entities/Enemy Dropped Spoils", fileName = "Enemy Dropped Spoils")]
    public class SO_EnemyDroppedSpoils : ScriptableObject
    {
        [Header("Exp")]
        [SerializeField] private int _exp = 0;
        [SerializeField] private int _additionalExpPerLevel = 0;

        [Space]
        [SerializeField] private ListObjects<string, Spoil> _spoils = new((spoil) =>
        {
            if (spoil == null) return null;
            return spoil.Name;
        });

        public int Exp => _exp;
        public int AdditionalExpPerLevel => _additionalExpPerLevel;

        public ReadOnlyCollection<Spoil> Spoils => _spoils.List;
        public Spoil GetSpoil(string name) => _spoils.Get(name);

    }
}

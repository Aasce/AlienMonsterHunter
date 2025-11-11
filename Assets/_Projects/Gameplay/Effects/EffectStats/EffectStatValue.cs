using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Effects
{
    [System.Serializable]
    public struct EffectStatValue : IIdentifiable
    {
        public const string PREFIX_ID = "effect_stat";

        [SerializeField] private string _id;

        public readonly string Id => _id; 
        string IIdentifiable.Id
        {
            readonly get => Id;
            set => _id = value;
        }

        public EffectStatValue(string id = null)
        {
            _id = string.IsNullOrEmpty(id) ? IdGenerator.NewId(PREFIX_ID) : id;
        }

        public override readonly string ToString()
        {
            return $"EffectStatValue(Id={_id})";
        }
    }
}
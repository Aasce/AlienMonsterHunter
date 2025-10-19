using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Character Stats", fileName = "Character Stats")]
    public class SO_CharacterStats : SO_EntityStats
    {
        [Header("Character Stats")]
        [Space]
        [SerializeField, Min(0f)] private float _selfViewRadius = 2f;
        [SerializeField, Min(0f)] private float _viewRadius = 12f;
        [SerializeField, Min(0f)] private float _viewAngle = 70f;

        public float SelfViewRadius => _selfViewRadius;
        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;
    }
}
using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Character Stats", fileName = "Character Stats")]
    public class SO_CharacterStats : SO_EntityStats
    {
        public float SelfViewRadius => this.GetCustomStat("SelfViewRadius");
        public float ViewRadius => this.GetCustomStat("ViewRadius");
        public float ViewAngle => this.GetCustomStat("ViewAngle");
    }
}
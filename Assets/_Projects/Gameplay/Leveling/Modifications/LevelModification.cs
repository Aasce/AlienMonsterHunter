using UnityEngine;

namespace Asce.Game.Levelings
{
    [System.Serializable]
    public class LevelModification
    {
        [SerializeField] private string _targetKey;
        [SerializeField] private float _deltaValue;
        [SerializeField] private ModificationType _type = ModificationType.Additive;

        /// <summary> Identifier for what to modify (name/key). </summary>
        public string TargetKey => _targetKey;

        /// <summary> Numeric value for the modification. Interpretation depends on Type. </summary>
        public float Value => _deltaValue;

        /// <summary> How to interpret the value (additive, multiplicative, set, custom). </summary>
        public ModificationType Type => _type;
    }
}
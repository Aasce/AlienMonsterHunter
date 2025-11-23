using UnityEngine;

namespace Asce.Game
{
    [System.Serializable]
    public struct CustomValue
    {
        [SerializeField] private string _name;
        [SerializeField] private float _value;


        public readonly string Name => _name;
        public readonly float Value => _value;

        public CustomValue(string name, float value)
        {
            _name = name;
            _value = value;
        }

    }
}

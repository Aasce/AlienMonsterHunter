using System;
using UnityEngine;

namespace Asce.Game.Items
{ 
    [Serializable]
    public class Spoil
    {
        [SerializeField] private string _name = string.Empty;

        [SerializeField, Range(0f, 1f)] private float _dropRate = 0f;
        [SerializeField] private int _minQuantity = 0;
        [SerializeField] private int _maxQuantity = 0;

        public string Name => _name;

        public float DropRate => _dropRate;
        public int MinQuantity => _minQuantity;
        public int MaxQuantity => _maxQuantity;
    }
}
using UnityEngine;

namespace Asce.Game.Enviroments
{
    [System.Serializable]
    public class PlaceHolder
    {
        [SerializeField] private Transform _point;
        [SerializeField] private bool _isOccupied = false;

        public Transform Point => _point;
        public bool IsOccupied
        {
            get => _isOccupied;
            set => _isOccupied = value;
        }
    }
}
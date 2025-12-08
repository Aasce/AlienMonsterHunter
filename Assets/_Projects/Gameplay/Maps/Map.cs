using Asce.Core;
using UnityEngine;

namespace Asce.Game.Maps
{
    public class Map : GameComponent
    {
        [SerializeField] private SO_MapInformation _information;

        public SO_MapInformation Information => _information;



    }
}
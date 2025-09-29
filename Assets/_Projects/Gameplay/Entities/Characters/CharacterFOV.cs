using Asce.Managers;
using UnityEngine;
using System.Collections.Generic;
using Asce.Game.FOVs;

namespace Asce.Game.Entities
{
    public class CharacterFOV : GameComponent
    {
        [SerializeField] private List<FieldOfView> _fovs = new();

        public List<FieldOfView> FOVs => _fovs;


        private void LateUpdate()
        {
            foreach (var fov in _fovs)
            {
                if (fov == null) continue;
                fov.DrawFieldOfView();
            }
        }
    }
}

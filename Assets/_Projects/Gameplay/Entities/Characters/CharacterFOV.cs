using Asce.Core;
using UnityEngine;
using Asce.Game.FOVs;

namespace Asce.Game.Entities
{
    public class CharacterFOV : GameComponent
    {
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;

        public FieldOfView Fov => _fov;
        public FieldOfView FovSelf => _fovSelf;

        private void LateUpdate()
        {
            Fov.DrawFieldOfView();
            FovSelf.DrawFieldOfView();
        }
    }
}

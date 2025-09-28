using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class ParticleVFXObject : VFXObject
    {
        [SerializeField] protected List<ParticleSystem> _particleSystems = new();


        public List<ParticleSystem> ParticleSystems => _particleSystems;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class ParticleVFXObject : VFXObject
    {
        [SerializeField] protected List<ParticleSystem> _particleSystems = new();


        public List<ParticleSystem> ParticleSystems => _particleSystems;

        public void StopEmitting()
        {
            foreach (ParticleSystem particle in _particleSystems)
            {
                if (particle == null) Console.WriteLine();
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public void ClearEmitting()
        {
            foreach (ParticleSystem particle in _particleSystems)
            {
                if (particle == null) Console.WriteLine();
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

        }
    }
}

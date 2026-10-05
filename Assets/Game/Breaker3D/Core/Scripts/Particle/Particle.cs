using System.Collections.Generic;
using UnityEngine;
using VirtueSky.ObjectPooling;

namespace Breaker3D.Core.Particles
{
    public class Particle : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        private bool isReturningToPool;

        private void Awake()
        {
            RefreshParticleSystemsIfEmpty();
        }

        private void OnEnable()
        {
            isReturningToPool = false;
            RefreshParticleSystemsIfEmpty();

            foreach (ParticleSystem system in particleSystems)
            {
                if (system == null)
                    continue;

                system.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                system.Play(false);
            }
        }

        private void Update()
        {
            if (isReturningToPool || particleSystems.Count == 0 || HasAliveParticles())
                return;

            isReturningToPool = true;
            gameObject.DeSpawn();
        }

        private void OnDisable()
        {
            foreach (ParticleSystem system in particleSystems)
            {
                if (system != null)
                    system.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        [ContextMenu("Refresh Particle Systems")]
        private void RefreshParticleSystems()
        {
            particleSystems.Clear();
            particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>(true));
        }

        private void RefreshParticleSystemsIfEmpty()
        {
            if (particleSystems.Count == 0)
                RefreshParticleSystems();
        }

        private bool HasAliveParticles()
        {
            foreach (ParticleSystem system in particleSystems)
            {
                if (system != null && system.IsAlive(false))
                    return true;
            }

            return false;
        }
    }
}

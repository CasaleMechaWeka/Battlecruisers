using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class AdvancedExplosion : MonoBehaviour
    {
        public ParticleSystem leadingParticleSystem;
        public ParticleSystem followingParticleSystem;

        void Start()
        {
            Helper.AssertIsNotNull(leadingParticleSystem, followingParticleSystem);

            followingParticleSystem.randomSeed = leadingParticleSystem.randomSeed;
        }
    }
}
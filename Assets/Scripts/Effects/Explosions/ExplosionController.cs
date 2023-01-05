using BattleCruisers.Effects.ParticleSystems;
using UnityEngine;
namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionController : ParticleSystemGroupInitialiser
    {
        public virtual IExplosion Initialise()
        {
            return 
                new Explosion(
                    this, 
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }
    }
  //   For directionalShine shader 
  /*
    public class ExplosionColor : MonoBehaviour
    {
        public Color explosionColor = Color.red;

        private void Start()
        {
            // Get the particle system component
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();

            // Get the main module of the particle system
            ParticleSystem.MainModule mainModule = particleSystem.main;

            // Set the start color of the particles to the explosion color
            mainModule.startColor = explosionColor;
        }
    } */
}
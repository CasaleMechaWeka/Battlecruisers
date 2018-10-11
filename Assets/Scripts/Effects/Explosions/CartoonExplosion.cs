using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class CartoonExplosion : MonoBehaviour, IExplosion
    {
        private ParticleSystem _explosionCore, _explosionTrails;

        public void Initialise(bool showTrails)
        {
            _explosionCore = transform.FindNamedComponent<ParticleSystem>("CoreExplosion");
            SetupParticleSystem(_explosionCore.main);

            _explosionTrails = transform.FindNamedComponent<ParticleSystem>("FireworkExplosion");
            _explosionTrails.gameObject.SetActive(showTrails);
            SetupParticleSystem(_explosionTrails.main);
        }

        /// <summary>
        /// Undo settings that are there to make testing particle systems easy.
        /// Could really do this at the prefab level, but then every time I want
        /// to tweak a particle system I need to readd them :P
        /// </summary>
        private void SetupParticleSystem(ParticleSystem.MainModule particleSystemMainModule)
        {
            particleSystemMainModule.playOnAwake = false;
            particleSystemMainModule.loop = false;
        }

        public void Show(Vector3 position)
        {
            gameObject.transform.position = position;

            _explosionCore.Play();
        }
    }
}
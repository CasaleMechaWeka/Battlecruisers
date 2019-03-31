using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Laser
{
    public class LaserImpact : MonoBehaviour, ILaserImpact
    {
        private ParticleSystem _glow;

        public void Initialise()
        {
            _glow = GetComponent<ParticleSystem>();
            Assert.IsNotNull(_glow);
        }

        public void Show(Vector3 postion)
        {
            transform.position = postion;
            _glow.Play();
        }

        public void Hide()
        {
            _glow.Stop();
        }
    }
}
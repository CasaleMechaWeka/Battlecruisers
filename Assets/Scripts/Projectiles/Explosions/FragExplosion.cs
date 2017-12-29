using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    public class FragExplosion : Explosion
    {
        private Animator _animator;

        public override void Initialise(float radiusInM, float durationInS)
        {
            base.Initialise(radiusInM, durationInS);

            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);
        }

        protected override void OnShow()
        {
            _animator.Play(stateNameHash: 0);
        }

        public void OnAnimationCompleted()
        {
            Destroy(gameObject);
        }
    }
}

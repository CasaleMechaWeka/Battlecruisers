using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.BarrelRecoil
{
    public class BarrelAnimation : IAnimation
    {
        private readonly Animator _animator;

        private const string BARREL_ANIMATION_STATE = "BarrelAnimation";
        private const int DEFAULT_ANIMATION_LAYER_INDEX = 0;

        public BarrelAnimation(Animator animator)
        {
            _animator = animator;

            int stateId = Animator.StringToHash(BARREL_ANIMATION_STATE);
            Assert.IsTrue(_animator.HasState(DEFAULT_ANIMATION_LAYER_INDEX, stateId));
        }

        public void Play()
        {
            _animator.enabled = true;
            _animator.Play(BARREL_ANIMATION_STATE, layer: -1, normalizedTime: 0);
        }
    }
}
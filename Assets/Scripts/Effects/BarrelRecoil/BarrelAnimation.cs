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
            Assert.IsNotNull(animator, "Animator cannot be null");
            _animator = animator;
        }

        public void Play()
        {
            if (CheckAnimationState())
            {
                _animator.enabled = true;
                _animator.Play(BARREL_ANIMATION_STATE, layer: -1, normalizedTime: 0);
            }
            else
            {
                Debug.LogWarning("Barrel animation state not found.");
            }
        }

        private bool CheckAnimationState()
        {
            int stateId = Animator.StringToHash(BARREL_ANIMATION_STATE);
            return _animator.HasState(DEFAULT_ANIMATION_LAYER_INDEX, stateId);
        }
    }
}
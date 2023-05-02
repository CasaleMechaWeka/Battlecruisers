using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.BarrelRecoil
{
    public class PvPBarrelAnimation : IPvPAnimation
    {
        private readonly Animator _animator;

        private const string BARREL_ANIMATION_STATE = "PvPBarrelAnimation";
        private const int DEFAULT_ANIMATION_LAYER_INDEX = 0;

        public PvPBarrelAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void Play()
        {
            CheckAnimationState();
            _animator.enabled = true;
            _animator.Play(BARREL_ANIMATION_STATE, layer: -1, normalizedTime: 0);
        }

        /// <summary>
        /// If the game object is inactive, Animator.HasState() always returns false.
        /// Hence cannot perform this check at construction in case the game object is inactive.
        /// </summary>
        private void CheckAnimationState()
        {
            int stateId = Animator.StringToHash(BARREL_ANIMATION_STATE);
            Assert.IsTrue(_animator.HasState(DEFAULT_ANIMATION_LAYER_INDEX, stateId));
        }
    }
}
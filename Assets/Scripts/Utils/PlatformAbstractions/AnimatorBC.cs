using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class AnimatorBC : IAnimator
    {
        private readonly Animator _platformObject;

        public float Speed
        {
            get => _platformObject.speed;
            set => _platformObject.speed = value;
        }

        public AnimatorBC(Animator platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }

        public void Play(string stateName, int layer, float normalizedTime)
        {
            _platformObject.Play(stateName, layer, normalizedTime);
        }
    }
}
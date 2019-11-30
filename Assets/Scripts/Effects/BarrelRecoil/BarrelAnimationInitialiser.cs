using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.BarrelRecoil
{
    public class BarrelAnimationInitialiser : MonoBehaviour, IAnimationInitialiser
    {
        public IAnimation CreateAnimation()
        {
            Animator animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);
            animator.enabled = false;
            return new BarrelAnimation(animator);
        }
    }
}
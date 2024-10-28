using UnityEngine;

namespace BattleCruisers.Effects.BarrelRecoil
{
    public class DummyAnimationInitialiser : MonoBehaviour, IAnimationInitialiser
    {
        public Animator overrideAnimator;

        public IAnimation CreateAnimation()
        {
            Animator animator = overrideAnimator;

            if (animator == null)
            {
                // Try to get the Animator component from the current object or its parents
                animator = GetComponentInParent<Animator>();
                if (animator != null)
                {
                    Debug.Log("Animator from current or parent object: Found");
                }
            }
            else
            {
                Debug.Log("Using override animator.");
            }

            if (animator == null)
            {
                Debug.LogWarning("Animator component not found in the current or parent object.");
                return null;
            }

            animator.enabled = false;
            return new BarrelAnimation(animator);
        }
    }
}

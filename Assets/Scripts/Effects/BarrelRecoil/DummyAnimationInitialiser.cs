using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.BarrelRecoil
{
    public class DummyAnimationInitialiser : MonoBehaviour, IAnimationInitialiser
    {
        // Public field to allow manual assignment of an Animator in the Inspector
        public Animator overrideAnimator;

        public IAnimation CreateAnimation()
        {
            // Use the manually assigned animator if available
            Animator animator = overrideAnimator;

            // If no animator is assigned, use the default behavior
            if (animator == null)
            {
                // Try to get the Animator component from the current object
                animator = GetComponent<Animator>();
                Debug.Log("Animator from current object: " + (animator != null ? "Found" : "Not Found"));

            }
            else
            {
                Debug.Log("Using override animator.");
            }

            // Ensure the animator is not null
            if (animator == null)
            {
                Debug.LogError("Animator component not found in the current or parent object.");
                return null; // Return null or handle the error as needed
            }

            animator.enabled = false;
            return new BarrelAnimation(animator);
        }
    }
}

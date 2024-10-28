using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.BarrelRecoil
{
    public class PvPDummyAnimationInitialiser : MonoBehaviour, IPvPAnimationInitialiser
    {
        public Animator overrideAnimator;

        public IPvPAnimation CreateAnimation()
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
                return new PvPDummyAnimation(); // Return a dummy animation if no animator is found
            }

            animator.enabled = false;
            return new PvPBarrelAnimation(animator);
        }
    }
}
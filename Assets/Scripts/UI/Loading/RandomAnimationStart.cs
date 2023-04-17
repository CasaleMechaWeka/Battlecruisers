using System.Collections;
using UnityEngine;

namespace BattleCruisers.UI.Loading
{
    public class RandomAnimationStart : MonoBehaviour
    {
        public string animationName = "LoadingScreenBackground";
        public float earliestAnimationStartTime = 0.0f;
        public float latestAnimationStartTime = 1.0f;

        void Start()
        {
            Animator animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found.");
                return;
            }

            if (earliestAnimationStartTime < 0.0f) earliestAnimationStartTime = 0.0f;
            if (latestAnimationStartTime > 1.0f) latestAnimationStartTime = 1.0f;
            if (earliestAnimationStartTime > latestAnimationStartTime) earliestAnimationStartTime = latestAnimationStartTime;

            float randomStartPoint = Random.Range(earliestAnimationStartTime, latestAnimationStartTime);
            animator.Play(animationName, 0, randomStartPoint);
        }
    }
}
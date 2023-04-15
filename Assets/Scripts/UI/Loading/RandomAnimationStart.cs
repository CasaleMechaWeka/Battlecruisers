using System.Collections;
using UnityEngine;

namespace BattleCruisers.UI.Loading
{
    public class RandomAnimationStart : MonoBehaviour
    {
        public GameObject background;
        public string animationName = "LoadingScreenBackground";
        public float minStartDelay = 0.0f;
        public float maxStartDelay = 1.0f;

        private IEnumerator Start()
        {
            Animator animator = background.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
                float startDelay = Random.Range(minStartDelay, maxStartDelay);
                yield return new WaitForSeconds(startDelay);
                animator.enabled = true;
                animator.Play(animationName, -1, Random.Range(0.0f, 1.0f));
            }
        }
    }
}
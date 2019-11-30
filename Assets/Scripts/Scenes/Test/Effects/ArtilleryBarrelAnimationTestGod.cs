using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class ArtilleryBarrelAnimationTestGod : MonoBehaviour
    {
        public Animator artilleryAnimator;

        void Start()
        {
            Debug.Log("Yo");

            // FELIX  NEXT  Look at animator properties while debugging?  Find state names?
            //artilleryAnimator.enabled = false;
            Debug.Log($"artilleryAnimator.enabled: {artilleryAnimator.enabled}");
        }

        public void PlayAnimation()
        {
            Debug.Log("PlayAnimation");

            artilleryAnimator.enabled = true;
            //artilleryAnimator.Play("GIBBERISH");
            //artilleryAnimator.Play("ArtilleryBarrel");
            artilleryAnimator.Play("ArtilleryBarrel", layer: -1, normalizedTime: 0);
        }
    }
}
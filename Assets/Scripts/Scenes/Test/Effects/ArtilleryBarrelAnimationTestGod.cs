using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class ArtilleryBarrelAnimationTestGod : MonoBehaviour
    {
        public Animator artilleryAnimator;

        void Start()
        {
            Debug.Log("Yo");

            //artilleryAnimator.enabled = false;
            Debug.Log($"artilleryAnimator.enabled: {artilleryAnimator.enabled}");
        }
    }
}
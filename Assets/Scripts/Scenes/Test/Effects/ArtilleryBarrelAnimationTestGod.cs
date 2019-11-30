using BattleCruisers.Effects;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class ArtilleryBarrelAnimationTestGod : MonoBehaviour
    {
        private BarrelAnimation _barrelAnimation;

        void Start()
        {
            Debug.Log("Start()");

            _barrelAnimation = FindObjectOfType<BarrelAnimation>();
            Assert.IsNotNull(_barrelAnimation);
            _barrelAnimation.Initialise();
        }

        public void PlayAnimation()
        {
            Debug.Log("PlayAnimation()");
            _barrelAnimation.Play();
        }
    }
}
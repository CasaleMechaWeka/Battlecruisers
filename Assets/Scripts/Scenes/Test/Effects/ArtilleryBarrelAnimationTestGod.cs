using BattleCruisers.Effects;
using BattleCruisers.Effects.BarrelRecoil;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class ArtilleryBarrelAnimationTestGod : MonoBehaviour
    {
        private IAnimation _barrelAnimation;

        void Start()
        {
            Debug.Log("Start()");

            BarrelAnimationInitialiser barrelAnimationInitialiser = FindObjectOfType<BarrelAnimationInitialiser>();
            Assert.IsNotNull(barrelAnimationInitialiser);
            _barrelAnimation = barrelAnimationInitialiser.CreateAnimation();
        }

        public void PlayAnimation()
        {
            Debug.Log("PlayAnimation()");
            _barrelAnimation.Play();
        }
    }
}
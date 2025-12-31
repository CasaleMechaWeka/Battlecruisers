using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.TurretBarrels
{
    public class LascannonFlapAnimationTestGod : MonoBehaviour
    {
        public Animator flapAnimator;

        private const string CLOSED_STATE = "ClosedFlap";
        private const string OPENED_STATE = "OpenedFlap";

        void Start()
        {
            Assert.IsNotNull(flapAnimator);
        }

        public void OpenFlap()
        {
            flapAnimator.Play(OPENED_STATE, layer: -1, normalizedTime: 0);
        }

        public void CloseFlap()
        {
            flapAnimator.Play(CLOSED_STATE, layer: -1, normalizedTime: 0);
        }
    }
}
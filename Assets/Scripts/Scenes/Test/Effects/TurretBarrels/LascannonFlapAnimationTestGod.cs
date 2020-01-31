using System.Collections;
using System.Collections.Generic;
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
    }
}
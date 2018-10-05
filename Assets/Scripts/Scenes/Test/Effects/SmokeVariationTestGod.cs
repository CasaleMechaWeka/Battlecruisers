using BattleCruisers.Effects;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    public class SmokeVariationTestGod : MonoBehaviour
    {
        private ICircularList<SmokeStrength> _smokeStrengths;
        private Smoke _smoke;

        void Start()
        {
            _smokeStrengths 
                = new CircularList<SmokeStrength>(
                    new List<SmokeStrength> { SmokeStrength.None, SmokeStrength.Weak, SmokeStrength.None, SmokeStrength.Strong });

            _smoke = FindObjectOfType<Smoke>();
            _smoke.Initialise();

            ChangeSmokeStrength();
        }

        private void ChangeSmokeStrength()
        {
            _smoke.SmokeStrength = _smokeStrengths.Next();
            Debug.Log("Set smoke strength to: " + _smoke.SmokeStrength);
            Invoke("ChangeSmokeStrength", time: 3);
        }
    }
}
using BattleCruisers.Effects.Smoke;
using BattleCruisers.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Smokes
{
    public class SmokeVariationTestGod : MonoBehaviour
    {
        private ICircularList<SmokeStrength> _smokeStrengths;
        private Smoke _smoke;

        void Start()
        {
            _smokeStrengths 
                = new CircularList<SmokeStrength>(
                    new List<SmokeStrength> { SmokeStrength.None, SmokeStrength.Weak, SmokeStrength.Normal, SmokeStrength.Strong });

            _smoke = FindObjectOfType<Smoke>();
            _smoke.Initialise(new SmokeChanger());

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
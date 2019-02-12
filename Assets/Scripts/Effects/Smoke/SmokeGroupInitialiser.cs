using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Effects.Smoke
{
    public class SmokeGroupInitialiser : MonoBehaviour
    {
        public void Initialise(IDamagable parentDamagable, bool showSmokeWhenDestroyed)
        {
            SmokeInitialiser[] smokeInitialisers = GetComponentsInChildren<SmokeInitialiser>();

            foreach (SmokeInitialiser smokeInitialiser in smokeInitialisers)
            {
                smokeInitialiser.Initialise(parentDamagable, showSmokeWhenDestroyed);
            }
        }
    }
}
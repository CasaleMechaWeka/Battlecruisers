using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers.Fog
{
    public enum FogStrength
    {
        Weak, Strong
    }

    public class FogOfWar : MonoBehaviourWrapper
    {
        public GameObject weakFog, strongFog;

        public void Initialise(FogStrength fogStrength)
        {
            Helper.AssertIsNotNull(weakFog, strongFog);

            IsVisible = false;

            weakFog.SetActive(fogStrength == FogStrength.Weak);
            strongFog.SetActive(fogStrength == FogStrength.Strong);
        }
    }
}

using BattleCruisers.Cruisers.Fog;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class FogOfWarTestGod : MonoBehaviour
    {
        public FogStrength fogStrength;

        void Start()
        {
            FogOfWar fog = FindObjectOfType<FogOfWar>();
            fog.Initialise(fogStrength);
            fog.UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);
        }
    }
}
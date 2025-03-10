using BattleCruisers.Cruisers.Fog;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class FogOfWarTestGod : MonoBehaviour
    {
        private FogOfWar _fog;

        public FogStrength initialFogStrength;

        void Start()
        {
            _fog = FindObjectOfType<FogOfWar>();
            InitialiseFog(initialFogStrength);
        }

        public void Strong()
        {
            InitialiseFog(FogStrength.Strong);
        }

        public void Weak()
        {
            InitialiseFog(FogStrength.Weak);
        }

        private void InitialiseFog(FogStrength fogStrength)
        {
            _fog.Initialise(fogStrength);
            _fog.IsVisible = true;
        }
    }
}
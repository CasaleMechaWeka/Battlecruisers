using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Aircraft.Kamikaze
{
    public class SmokingKamikazeTestGod : KamikazeTestGod
    {
        public AircraftController smokingAircraft;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(smokingAircraft);
            return base.GetGameObjects();
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            smokingAircraft.CompletedBuildable += (sender, e) => smokingAircraft.TakeDamage(smokingAircraft.Health / 2, damageSource: null);
        }
    }
}

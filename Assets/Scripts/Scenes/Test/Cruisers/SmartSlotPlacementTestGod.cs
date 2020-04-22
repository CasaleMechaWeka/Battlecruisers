using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Cruisers
{
    public class SmartSlotPlacementTestGod : TestGodBase
    {
        public Cruiser cruiser;

        protected override List<GameObject> GetGameObjects()
        {
            Assert.IsNotNull(cruiser);
            return new List<GameObject>()
            {
                cruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // FELIX  Avoid duplicate code for muiltiple cruisers => CruiserRegion class :)
            helper.SetupCruiser(cruiser);
        }
    }
}
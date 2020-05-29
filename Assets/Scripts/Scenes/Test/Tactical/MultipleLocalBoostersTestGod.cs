using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tactical
{
    public class MultipleLocalBoostersTestGod : TestGodBase
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
            helper.SetupCruiser(cruiser);
        }
    }
}
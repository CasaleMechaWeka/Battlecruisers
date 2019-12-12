using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathManagerTestGod : TestGodBase
    {
        public Cruiser playerCruiser, aiCruiser;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            return new List<GameObject>()
            {
                playerCruiser.GameObject,
                aiCruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            CreateAndDestroyCruiser(playerCruiser, helper);
            CreateAndDestroyCruiser(aiCruiser, helper);
        }

        private void CreateAndDestroyCruiser(Cruiser cruiser, Helper helper)
        {
            helper.SetupCruiser(cruiser);
            cruiser.Destroy();
        }
    }
}
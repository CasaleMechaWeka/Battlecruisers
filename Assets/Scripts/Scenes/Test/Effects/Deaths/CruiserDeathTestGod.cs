using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTestGod : TestGodBase
    {
        private Cruiser _cruiser;

        protected override IList<GameObject> GetGameObjects()
        {
            _cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(_cruiser);

            return new List<GameObject>()
            {
                _cruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            CreateAndDestroyCruiser(helper);
        }

        private void CreateAndDestroyCruiser(Helper helper)
        {
            helper.SetupCruiser(_cruiser);
            DestroyCruiser(helper, _cruiser);
        }

        protected virtual void DestroyCruiser(Helper helper, Cruiser cruiser)
        {
            cruiser.Destroy();
        }
    }
}
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class ShipDeathsTestGod : TestGodBase
    {
        private ShipController[] _ships;
        private IDeferrer _deferrer;

        protected override List<GameObject> GetGameObjects()
        {
            _ships = FindObjectsOfType<ShipController>();
            return
                _ships
                    .Select(ship => ship.GameObject)
                    .ToList();
        }

        protected override void Setup(Helper helper)
        {
            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

            foreach (ShipController ship in _ships)
            {
                helper.InitialiseUnit(ship);
                ship.StartConstruction();
                ship.CompletedBuildable += Ship_CompletedBuildable;
            }
        }

        private void Ship_CompletedBuildable(object sender, EventArgs e)
        {
            // Ship turrets are initialised when ship completes building, but this
            // event handler is called first.  Hence wait for turrets to be initialised
            // before destroying ship.
            _deferrer.Defer(() => (sender as ShipController).Destroy(), delayInS: 0.1f);
        }
    }
}
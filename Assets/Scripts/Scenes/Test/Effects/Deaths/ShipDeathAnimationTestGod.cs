using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class ShipDeathAnimationTestGod : TestGodBase
    {
        private IList<IShipDeath> _shipDeaths;
        private IDeferrer _deferrer;
        // FELIX  Tidy :)
        private ShipController[] _ships;

        protected override List<GameObject> GetGameObjects()
        {
            ShipDeathInitialiser[] shipDeathInitialisers = FindObjectsOfType<ShipDeathInitialiser>();
            _shipDeaths
                = shipDeathInitialisers
                    .Select(initialiser => initialiser.CreateShipDeath())
                    .ToList();

            return
                shipDeathInitialisers
                    .Select(initialiser => initialiser.gameObject)
                    .ToList();

            // FELIX
            //_ships = FindObjectsOfType<ShipController>();
            //return
            //    _ships
            //        .Select(ship => ship.GameObject)
            //        .ToList();
        }

        protected override void Setup(Helper helper)
        {
            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

            foreach (IShipDeath shipDeath in _shipDeaths)
            {
                shipDeath.Activate(new Vector3(7, 0.25f));
                shipDeath.Deactivated += (sender, e) => _deferrer.Defer(() => shipDeath.Activate(new Vector3(7, 0.25f)), delayInS: 1);
            }

            // FELIX

            //foreach (ShipController ship in _ships)
            //{
            //    helper.InitialiseUnit(ship);
            //    ship.StartConstruction();
            //    ship.CompletedBuildable += Ship_CompletedBuildable;
            //}
        }

        // FELIX
        //private void Ship_CompletedBuildable(object sender, EventArgs e)
        //{
        //    // Ship turrets are initialised when ship completes building, but this
        //    // event handler is called first.  Hence wait for turrets to be initialised
        //    // before destroying ship.
        //    _deferrer.Defer(() => (sender as ShipController).Destroy(), delayInS: 0.1f);
        //}
    }
}
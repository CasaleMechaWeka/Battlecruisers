using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class ShipDeathsTestGod : MonoBehaviour
    {
        private IVariableDelayDeferrer _deferrer;

        void Start()
        {
            _deferrer = new VariableDelayDeferrer();
            Helper helper = new Helper();
            ShipController[] ships = FindObjectsOfType<ShipController>();

            foreach (ShipController ship in ships)
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
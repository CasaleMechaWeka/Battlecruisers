using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipAttackingCruiserTestGod : TestGodBase
    {
        private ShipController _attackBoat;

        protected override IList<GameObject> GetGameObjects()
        {
            _attackBoat = FindObjectOfType<ShipController>();

            return new List<GameObject>()
            {
                _attackBoat.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(Faction.Reds);

            // Setup ship
            helper.InitialiseUnit(_attackBoat, Faction.Blues);
            _attackBoat.StartConstruction();
        }
    }
}
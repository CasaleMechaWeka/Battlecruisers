using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipStopsForEnemyCruiserTestGod : TestGodBase
    {
        private ShipController _ship;

        protected override IList<GameObject> GetGameObjects()
        {
            _ship = FindObjectOfType<ShipController>();
            Assert.IsNotNull(_ship);

            return new List<GameObject>()
            {
                _ship.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            BuildableInitialisationArgs redArgs = new BuildableInitialisationArgs(helper, faction: Faction.Reds);

            EnemyShipBlockerInitialiser enemyShipBlockerInitialiser = FindObjectOfType<EnemyShipBlockerInitialiser>();
            Assert.IsNotNull(enemyShipBlockerInitialiser);
            ITargetTracker enemyShipBlockerTargetTracker
                = enemyShipBlockerInitialiser.Initialise(
                    redArgs.FactoryProvider.Targets,
                    redArgs.CruiserSpecificFactories.Targets.TrackerFactory,
                    Faction.Blues);

            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            redCruiser.BlockedShipsTracker.Returns(enemyShipBlockerTargetTracker);

            helper.InitialiseUnit(_ship, enemyCruiser: redCruiser);
            _ship.StartConstruction();
        }
    }
}
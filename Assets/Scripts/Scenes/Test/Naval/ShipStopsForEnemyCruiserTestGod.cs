using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Block;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class ShipStopsForEnemyCruiserTestGod : TestGodBase
    {
        protected override void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

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

            ShipController ship = FindObjectOfType<ShipController>();
            Assert.IsNotNull(ship);
            helper.InitialiseUnit(ship, enemyCruiser: redCruiser);
            ship.StartConstruction();
        }
    }
}
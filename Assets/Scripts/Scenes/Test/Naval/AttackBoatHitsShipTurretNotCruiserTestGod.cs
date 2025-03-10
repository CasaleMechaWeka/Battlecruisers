using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Naval
{
    public class AttackBoatHitsShipTurretNotCruiserTestGod : TestGodBase
    {
        private ShipController _ship;
        private TurretController _shipTurret;

        protected override List<GameObject> GetGameObjects()
        {
            List<GameObject> gameObjects = new List<GameObject>();

            _ship = FindObjectOfType<ShipController>();
            gameObjects.Add(_ship.GameObject);

            _shipTurret = FindObjectOfType<TurretController>();
            gameObjects.Add(_shipTurret.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            // Setup ship
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

            helper.InitialiseUnit(_ship, Faction.Blues, enemyCruiser: redCruiser);
            _ship.StartConstruction();

            // Setup ship turret
            helper.InitialiseBuilding(_shipTurret, Faction.Reds);
            _shipTurret.StartConstruction();

            // Setup fake cruiser
            TestTarget fakeCruiser = FindObjectOfType<TestTarget>();
            fakeCruiser.Initialise(helper.CommonStrings, Faction.Reds);
        }
    }
}
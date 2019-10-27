using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class KamikazeRecyclingTestGod : TestGodBase
    {
        private IFactory _target;
        private AirFactory _airFactory;
        private KamikazeSignal[] _kamikazeSignals;

        public UnitWrapper bomberPrefab;
        public int kamikazeDelayInS = 5;

        protected override async Task<Helper> CreateHelperAsync(IUpdaterProvider updaterProvider)
        {
            return await HelperFactory.CreateHelperAsync(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.FAST, updaterProvider: updaterProvider);
        }

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<NavalFactory>();
            _airFactory = FindObjectOfType<AirFactory>();
            _kamikazeSignals = FindObjectsOfType<KamikazeSignal>();

            List<GameObject> gameObjects
                = _kamikazeSignals
                    .Select(kamikazeSignal => kamikazeSignal.GameObject)
                    .ToList();
            gameObjects.Add(_target.GameObject);
            gameObjects.Add(_airFactory.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(bomberPrefab);
            Assert.IsTrue(bomberPrefab.GetComponentInChildren<BomberController>() != null);

            // Setup target
            helper.InitialiseBuilding(_target);
            _target.StartConstruction();

            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(_target.GameObject);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));

            // Setup air factory
            IAircraftProvider aircraftProvider = Substitute.For<IAircraftProvider>();
            IList<Vector2> bomberPatrolPonts = new List<Vector2>()
            {
                new Vector2(-12, 10),
                new Vector2(12, 10)
            };
            aircraftProvider.FindBomberPatrolPoints(default).ReturnsForAnyArgs(bomberPatrolPonts);

            helper.InitialiseBuilding(_airFactory, aircraftProvider: aircraftProvider);
            _airFactory.StartConstruction();
            _airFactory.CompletedBuildable += (sender, e) => _airFactory.StartBuildingUnit(bomberPrefab);

            // Setup kamikaze signals
            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            
            for (int i = 0; i < _kamikazeSignals.Length; ++i)
            {
                KamikazeSignal kamikazeSignal = _kamikazeSignals[i];

                timeScaleDeferrer.Defer(() =>
                {
                    helper.InitialiseBuilding(kamikazeSignal, enemyCruiser: enemyCruiser);
                    kamikazeSignal.StartConstruction();
                },
                delayInS: (i + 1) * kamikazeDelayInS);
            }
        }
    }
}

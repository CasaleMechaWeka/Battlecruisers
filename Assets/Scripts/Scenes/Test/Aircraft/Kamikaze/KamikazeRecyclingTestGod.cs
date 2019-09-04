using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Performance.ObjectPooling
{
    public class KamikazeRecyclingTestGod : TestGodBase
    {
        public UnitWrapper bomberPrefab;
        public int kamikazeDelayInS = 5;

        protected override void Start()
        {
            base.Start();

            Assert.IsNotNull(bomberPrefab);
            Assert.IsTrue(bomberPrefab.GetComponentInChildren<BomberController>() != null);

            Helper helper = new Helper(buildSpeedMultiplier: BCUtils.BuildSpeedMultipliers.FAST, updaterProvider: _updaterProvider);

            // Setup target
            IFactory _target = FindObjectOfType<NavalFactory>();
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

            AirFactory airFactory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(airFactory, aircraftProvider: aircraftProvider);
            airFactory.StartConstruction();
            airFactory.CompletedBuildable += (sender, e) => airFactory.StartBuildingUnit(bomberPrefab);

            // Setup kamikaze signals
            TimeScaleDeferrer timeScaleDeferrer = GetComponent<TimeScaleDeferrer>();
            KamikazeSignal[] kamikazeSignals = FindObjectsOfType<KamikazeSignal>();
            
            for (int i = 0; i < kamikazeSignals.Length; ++i)
            {
                KamikazeSignal kamikazeSignal = kamikazeSignals[i];

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

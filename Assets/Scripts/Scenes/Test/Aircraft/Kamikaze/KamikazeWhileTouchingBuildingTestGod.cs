using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Kamikaze
{
    public class KamikazeWhileTouchingBuildingTestGod : TestGodBase
    {
        private IFactory _target;
        private KamikazeSignal _kamikazeSignal;
        private TestAircraftController _aircraft;

        public List<Vector2> aircraftPatrolPoints;

        protected override IList<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<Factory>();
            _kamikazeSignal = FindObjectOfType<KamikazeSignal>();
            _aircraft = FindObjectOfType<TestAircraftController>();

            return new List<GameObject>()
            {
                _target.GameObject,
                _kamikazeSignal.GameObject,
                _aircraft.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_target);
            _target.StartConstruction();

            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(_target.GameObject);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));

            // Setup kamikaze signal
            helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            // Setup aircraft
            _aircraft.PatrolPoints = aircraftPatrolPoints;
            helper.InitialiseUnit(_aircraft);
            _aircraft.StartConstruction();

            // When completed, aircraft switches to patrol movement controller.
            // Hence wait a bit after completed before setting kamikaze
            // homing movement controller.
            Invoke("Kamikaze", time: 1);
        }

        public void Kamikaze()
        {
            _kamikazeSignal.StartConstruction();
        }
    }
}

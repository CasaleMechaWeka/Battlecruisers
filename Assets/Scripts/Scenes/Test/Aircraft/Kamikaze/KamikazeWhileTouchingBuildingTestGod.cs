using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Kamikaze
{
    public class KamikazeWhileTouchingBuildingTestGod : MonoBehaviour
    {
        private KamikazeSignal _kamikazeSignal;

        public List<Vector2> aircraftPatrolPoints;

        void Start()
        {
            Helper helper = new Helper();

            // Setup target
            IFactory _target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(_target);
            _target.StartConstruction();

            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(_target.GameObject);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));

            // Setup kamikaze signal
            _kamikazeSignal = FindObjectOfType<KamikazeSignal>();
            helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            // Setup aircraft
            TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
            aircraft.PatrolPoints = aircraftPatrolPoints;
            helper.InitialiseUnit(aircraft);
            aircraft.StartConstruction();

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

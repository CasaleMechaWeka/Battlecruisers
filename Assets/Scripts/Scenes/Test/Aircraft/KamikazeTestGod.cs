using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class KamikazeTestGod : MonoBehaviour
	{
        private KamikazeSignal _kamikazeSignal;

		void Start()
		{
			Helper helper = new Helper();
			
            // Setup target
            IFactory _target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(_target);
			_target.StartConstruction();

			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(_target.GameObject);
			enemyCruiser.AttackCapabilities.Returns(new List<TargetType>());
            
			// Setup kamikaze signal
			_kamikazeSignal = FindObjectOfType<KamikazeSignal>();
            helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            // Setup AA
            TurretController aaTurret = FindObjectOfType<TurretController>();
            helper.InitialiseBuilding(aaTurret);
            aaTurret.StartConstruction();

            // Setup aircraft
            AircraftController[] aircraftList = FindObjectsOfType<AircraftController>();
            foreach (AircraftController aircraft in aircraftList)
            {
                helper.InitialiseUnit(aircraft);
                aircraft.StartConstruction();
            }

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

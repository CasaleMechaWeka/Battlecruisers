using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.Test.Aircraft.Kamikaze
{
    public class KamikazeTestGod : TestGodBase
    {
        public int kamikaziDelayInS = 1;

        protected override async void Start()
        {
            base.Start();

            Helper helper = new Helper(updaterProvider: _updaterProvider);

            // Setup target
            IFactory _target = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(_target);
            _target.StartConstruction();

            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(_target.GameObject);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));

            // Setup AA
            TurretController[] aaTurrets = FindObjectsOfType<TurretController>();
            foreach (TurretController aaTurret in aaTurrets)
            {
                helper.InitialiseBuilding(aaTurret);
                aaTurret.StartConstruction();
            }

            // Setup aircraft
            AircraftController[] aircraftList = FindObjectsOfType<AircraftController>();
            foreach (AircraftController aircraft in aircraftList)
            {
                helper.InitialiseUnit(aircraft);
                aircraft.StartConstruction();
            }

            // Setup kamikaze signal.  When completed, aircraft switches to patrol movement controller.
            KamikazeSignal kamikazeSignal = FindObjectOfType<KamikazeSignal>();
            helper.InitialiseBuilding(kamikazeSignal, enemyCruiser: enemyCruiser);

            await Task.Delay(kamikaziDelayInS * 1000);

            kamikazeSignal.StartConstruction();
        }
    }
}

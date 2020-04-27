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
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Kamikaze
{
    public class KamikazeTestGod : TestGodBase
    {
        private IFactory _target;
        private TurretController[] _aaTurrets;
        private AircraftController[] _aircraftList;
        private KamikazeSignal _kamikazeSignal;

        public int kamikaziDelayInS = 1;

        protected override List<GameObject> GetGameObjects()
        {
            _target = FindObjectOfType<Factory>();
            _kamikazeSignal = FindObjectOfType<KamikazeSignal>();
            _aaTurrets = FindObjectsOfType<TurretController>();
            _aircraftList = FindObjectsOfType<AircraftController>();

            List<GameObject> gameObjects = new List<GameObject>()
            {
                _target.GameObject,
                _kamikazeSignal.GameObject
            };
            gameObjects.AddRange(_aaTurrets.Select(turret => turret.GameObject));
            gameObjects.AddRange(_aircraftList.Select(aircraft => aircraft.GameObject));

            return gameObjects;
        }

        protected async override void Setup(Helper helper)
        {
            // Setup target
            helper.InitialiseBuilding(_target);
            _target.StartConstruction();

            ICruiser enemyCruiser = Substitute.For<ICruiser>();
            enemyCruiser.GameObject.Returns(_target.GameObject);
            enemyCruiser.AttackCapabilities.Returns(new ReadOnlyCollection<TargetType>(new List<TargetType>()));
            // Mimic cruiser being destroyed
            _target.Destroyed += (sender, e) => enemyCruiser.GameObject.Returns((GameObject)null);

            // Setup AA
            foreach (TurretController aaTurret in _aaTurrets)
            {
                helper.InitialiseBuilding(aaTurret);
                aaTurret.StartConstruction();
            }

            // Setup aircraft
            foreach (AircraftController aircraft in _aircraftList)
            {
                helper.InitialiseUnit(aircraft);
                aircraft.StartConstruction();
            }

            // Setup kamikaze signal.  When completed, aircraft switches to patrol movement controller.
            helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            await Task.Delay(kamikaziDelayInS * 1000);

            _kamikazeSignal.StartConstruction();
        }

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}

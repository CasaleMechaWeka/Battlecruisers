using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Cruisers;
using NSubstitute;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTest : AircraftVsShieldsTest
    {
        private KamikazeSignal _kamikazeSignal;

        protected override void OnInitialised()
        {
            _kamikazeSignal = GetComponentInChildren<KamikazeSignal>();
            Assert.IsNotNull(_kamikazeSignal);

            IBuildable rightMostEnemyBuildable = _rightGroup.Buildables.Last();
            ICruiser enemyCruiser = _helper.CreateCruiser(rightMostEnemyBuildable.GameObject);
            enemyCruiser.Faction.Returns(rightMostEnemyBuildable.Faction);
            _helper.InitialiseBuilding(_kamikazeSignal, enemyCruiser: enemyCruiser);

            // Give time for aircraft to start patrolling
            Invoke("Kamikaze", time: 1);
        }

        public void Kamikaze()
        {
            _kamikazeSignal.StartConstruction();
        }
    }
}

using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarTestGod : TestGodBase
	{
		private Helper _helper;

        public Building leftTarget, rightTarget;
		public DeathstarController leftDeathstar, rightDeathstar;

        protected override void Start()
        {
            base.Start();

            _helper = new Helper(updaterProvider: _updaterProvider);

            SetupPair(leftTarget, rightDeathstar, Faction.Blues);
			SetupPair(rightTarget, leftDeathstar, Faction.Reds);
		}

		private void SetupPair(IBuilding target, DeathstarController deathstar, Faction targetFaction)
		{
			// Setup target
            _helper.InitialiseBuilding(target, targetFaction);
			
			
			// Setup deathstar
			Faction deathstarFaction = BcUtils.Helper.GetOppositeFaction(targetFaction);

			Vector2 parentCruiserPosition = deathstar.transform.position;
            Vector2 enemyCruiserPosition = target.Position;
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BcUtils.RandomGenerator.Instance);
			
            _helper.InitialiseUnit(deathstar, deathstarFaction, aircraftProvider: aircraftProvider);
			deathstar.StartConstruction();
		}
	}
}

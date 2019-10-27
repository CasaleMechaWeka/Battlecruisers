using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Aircraft.Satellites
{
    public class DeathstarTestGod : TestGodBase
	{
        public Building leftTarget, rightTarget;
		public DeathstarController leftDeathstar, rightDeathstar;

        protected override IList<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                leftTarget.GameObject,
                rightTarget.GameObject,
                leftDeathstar.GameObject,
                rightDeathstar.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            SetupPair(helper, leftTarget, rightDeathstar, Faction.Blues);
			SetupPair(helper, rightTarget, leftDeathstar, Faction.Reds);
		}

		private void SetupPair(
            Helper helper,
            IBuilding target, 
            DeathstarController deathstar, 
            Faction targetFaction)
		{
			// Setup target
            helper.InitialiseBuilding(target, targetFaction);
			
			// Setup deathstar
			Faction deathstarFaction = BcUtils.Helper.GetOppositeFaction(targetFaction);

			Vector2 parentCruiserPosition = deathstar.transform.position;
            Vector2 enemyCruiserPosition = target.Position;
            IAircraftProvider aircraftProvider = new AircraftProvider(parentCruiserPosition, enemyCruiserPosition, BcUtils.RandomGenerator.Instance);
			
            helper.InitialiseUnit(deathstar, deathstarFaction, aircraftProvider: aircraftProvider);
			deathstar.StartConstruction();
		}
	}
}

using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft
{
    public class BombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft, bomberToRight;
		public List<Vector2> leftPatrolPoints, rightPatrolPoints;

		void Start() 
		{
			Helper helper = new Helper();

			AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Blues);

            IList<TargetType> targetTypes = new List<TargetType>() { factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(factory.GameObject, targetFilter);

            IAircraftProvider leftAircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: leftPatrolPoints);
            helper.InitialiseUnit(bomberToLeft, Faction.Reds, aircraftProvider: leftAircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Right);
            bomberToLeft.StartConstruction();

            IAircraftProvider rightAircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: rightPatrolPoints);
            helper.InitialiseUnit(bomberToRight, Faction.Reds, aircraftProvider: rightAircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Left);
            bomberToRight.StartConstruction();
		}
	}
}

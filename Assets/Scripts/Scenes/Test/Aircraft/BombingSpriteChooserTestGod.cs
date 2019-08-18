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
    public class BombingSpriteChooserTestGod : MonoBehaviour
    {
        public List<Vector2> patrolPoints;

        void Start()
        {
            Helper helper = new Helper();

            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Blues);

            IList<TargetType> targetTypes = new List<TargetType>() { factory.TargetType };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(factory.Faction, targetTypes);
            ITargetFactories targetFactories = helper.CreateTargetFactories(factory.GameObject, targetFilter: targetFilter);

            BomberController bomber = FindObjectOfType<BomberController>();
            IAircraftProvider aircraftProvider = helper.CreateAircraftProvider(bomberPatrolPoints: patrolPoints);
            helper.InitialiseUnit(bomber, Faction.Reds, aircraftProvider: aircraftProvider, targetFactories: targetFactories, parentCruiserDirection: Direction.Right);
            bomber.StartConstruction();
        }
    }
}

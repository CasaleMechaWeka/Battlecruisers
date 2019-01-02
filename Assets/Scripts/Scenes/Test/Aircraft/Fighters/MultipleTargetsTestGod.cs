using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class MultipleTargetsTestGod : MonoBehaviour 
	{
		void Start() 
		{
            Helper helper = new Helper();

            IList<Vector2> patrolPoints = new List<Vector2>()
            {
                new Vector2(-12, 5),
                new Vector2(12, 5)
            };

            // Fighter
            FighterController fighter = FindObjectOfType<FighterController>();
            helper.InitialiseUnit(fighter, Faction.Blues);
            fighter.StartConstruction();

            // Targets
            TestAircraftController[] targets = FindObjectsOfType<TestAircraftController>();

            foreach (TestAircraftController target in targets)
            {
                target.PatrolPoints = patrolPoints;
                helper.InitialiseUnit(target, Faction.Reds);
                target.StartConstruction();
            }
		}
	}
}

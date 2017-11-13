using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class DogfightTestsGod : MonoBehaviour 
	{
        public FighterController rightFighter, leftFighter;

		void Start() 
		{
			Helper helper = new Helper();

            helper.InitialiseUnit(rightFighter, faction: Faction.Reds, parentCruiserDirection: Direction.Left);
			rightFighter.StartConstruction();

            helper.InitialiseUnit(leftFighter, faction: Faction.Blues, parentCruiserDirection: Direction.Right);
			leftFighter.StartConstruction();
		}
	}
}

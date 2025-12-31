using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class DogfightTestsGod : TestGodBase 
	{
        public FighterController rightFighter, leftFighter;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                rightFighter.GameObject,
                leftFighter.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            helper.InitialiseUnit(rightFighter, faction: Faction.Reds, parentCruiserDirection: Direction.Left, enemyCruiser: blueCruiser);
			rightFighter.StartConstruction();
            Helper.SetupUnitForUnitMonitor(rightFighter, redCruiser);

            helper.InitialiseUnit(leftFighter, faction: Faction.Blues, parentCruiserDirection: Direction.Right, enemyCruiser: redCruiser);
			leftFighter.StartConstruction();
            Helper.SetupUnitForUnitMonitor(leftFighter, blueCruiser);
		}
	}
}

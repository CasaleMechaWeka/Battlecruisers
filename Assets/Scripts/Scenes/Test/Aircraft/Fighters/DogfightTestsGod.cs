using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class DogfightTestsGod : TestGodBase 
	{
        public FighterController rightFighter, leftFighter;

        protected override void Start() 
		{
            base.Start();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

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

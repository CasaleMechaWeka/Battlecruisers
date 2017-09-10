using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Aircraft.Fighters
{
    public class DogfightTestsGod : MonoBehaviour 
	{
		public FighterController fighter1, fighter2;

		void Start() 
		{
			Helper helper = new Helper();

            helper.InitialiseUnit(fighter1, faction: Faction.Reds);
			fighter1.StartConstruction();

            helper.InitialiseUnit(fighter2, faction: Faction.Blues);
			fighter2.StartConstruction();
		}
	}
}

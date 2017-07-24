using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BuildProgressTestsGod : MonoBehaviour 
	{
        public GameObject dummyEnemyCruiser;

		void Start() 
		{
			Helper helper = new Helper();
			
            ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
            ICruiser enemyCruiser = helper.CreateCruiser(dummyEnemyCruiser);

			Buildable[] buildables = FindObjectsOfType(typeof(Buildable)) as Buildable[];

			foreach (Buildable buildable in buildables)
			{
                helper.InitialiseBuildable(buildable, targetsFactory: targetsFactory, enemyCruiser: enemyCruiser);
				buildable.StartConstruction();
			}
		}
	}
}

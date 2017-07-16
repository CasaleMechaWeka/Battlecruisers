using BattleCruisers.Buildables;
using BattleCruisers.Movement;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class BuildProgressTestsGod : MonoBehaviour 
	{
		void Start() 
		{
			Helper helper = new Helper(numOfDrones: 8);
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();

			Buildable[] buildables = GameObject.FindObjectsOfType(typeof(Buildable)) as Buildable[];

			foreach (Buildable buildable in buildables)
			{
				helper.InitialiseBuildable(buildable, targetsFactory: targetsFactory);
				buildable.StartConstruction();
			}
		}
	}
}

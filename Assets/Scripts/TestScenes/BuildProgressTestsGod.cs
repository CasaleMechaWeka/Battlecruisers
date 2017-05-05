using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.TestScenes.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class BuildProgressTestsGod : MonoBehaviour 
	{
		void Start() 
		{
			Buildable[] buildables = GameObject.FindObjectsOfType(typeof(Buildable)) as Buildable[];

			Helper helper = new Helper(numOfDrones: 8);

			ITargetsFactory targetsFactory = helper.CreateTargetsFactory();

			foreach (Buildable buildable in buildables)
			{
				helper.InitialiseBuildable(buildable, targetsFactory: targetsFactory);
				buildable.StartConstruction();
			}
		}
	}
}

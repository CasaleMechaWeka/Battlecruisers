using BattleCruisers.Buildables;
using BattleCruisers.TargetFinders;
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

			ITargetFinderFactory targetFinderFactory = helper.CreateTargetFinderFactory();

			foreach (Buildable buildable in buildables)
			{
				helper.InitialiseBuildable(buildable, targetFinderFactory: targetFinderFactory);
				buildable.StartConstruction();
			}
		}
	}
}

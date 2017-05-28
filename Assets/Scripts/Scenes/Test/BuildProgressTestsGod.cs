using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BcUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test
{
	public class BuildProgressTestsGod : MonoBehaviour 
	{
		void Start() 
		{
			Buildable[] buildables = GameObject.FindObjectsOfType(typeof(Buildable)) as Buildable[];

			Helper helper = new Helper(numOfDrones: 8);

			BcUtils.IFactoryProvider factoryProvider = helper.CreateFactoryProvider();

			foreach (Buildable buildable in buildables)
			{
				helper.InitialiseBuildable(buildable, factoryProvider: factoryProvider);
				buildable.StartConstruction();
			}
		}
	}
}

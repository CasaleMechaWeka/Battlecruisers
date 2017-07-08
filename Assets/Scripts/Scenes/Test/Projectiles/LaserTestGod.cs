using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class LaserTestGod : MonoBehaviour 
	{
		private Buildable _target;

		void Start () 
		{
			// Setup target
			Helper helper = new Helper();
			_target = GameObject.FindObjectOfType<Buildable>();
			helper.InitialiseBuildable(_target, Faction.Blues);
			_target.StartConstruction();			
		}
	}
}

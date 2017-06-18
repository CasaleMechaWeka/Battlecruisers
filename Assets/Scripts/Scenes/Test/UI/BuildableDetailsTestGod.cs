using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.BuildingDetails;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
	public class BuildableDetailsTestGod : MonoBehaviour 
	{
		public BuildableDetailsController buildableDetails;
		public Buildable buildableToShow;

		void Start () 
		{
			buildableToShow.StaticInitialise();

			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			IDroneManager droneManager = Substitute.For<IDroneManager>();
			buildableDetails.Initialise(droneManager, spriteFetcher);
			buildableDetails.ShowBuildableDetails(buildableToShow, allowDelete: true);
		}
	}
}

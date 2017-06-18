using BattleCruisers.Buildables;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
	public class BuildableDetailsTestGod : MonoBehaviour 
	{
		public ComparableBuildableDetailsController buildableDetails;
		public Buildable buildableToShow;

		void Start () 
		{
			buildableToShow.StaticInitialise();

			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetails.Initialise(spriteFetcher);
			buildableDetails.ShowBuildableDetails(buildableToShow);
		}
	}
}

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.UI
{
    public class BuildableDetailsTestGod : MonoBehaviour 
	{
		public ComparableBuildingDetailsController buildableDetails;
		public Building buildableToShow;

		void Start () 
		{
			buildableToShow.StaticInitialise();

			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetails.Initialise(spriteFetcher);
			buildableDetails.ShowItemDetails(buildableToShow);
		}
	}
}

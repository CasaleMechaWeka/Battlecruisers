using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
    // FELIX  Avoid duplicate code with BaseBuildableDetails, create ItemDetails class?
    public class CruiserDetailsController : MonoBehaviour, IComparableItemDetails<Cruiser>
	{
		public CruiserStatsController statsController;
		public Text cruiserName;
		public Text cruiserDescription;
		public Image cruiserImage;

		public void ShowItemDetails(Cruiser cruiser, Cruiser cruiserToCompareTo = null)
		{
			Assert.IsNotNull(cruiser);

			statsController.ShowStats(cruiser, cruiserToCompareTo);
			cruiserName.text = cruiser.name;
			cruiserDescription.text = cruiser.description;
			cruiserImage.sprite = cruiser.Sprite;

			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}

using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls
{
	// FELIX  Avoid duplciate code with UnlockedItemsRow
	public class UnlockedHullsRow : MonoBehaviour
	{
		private IList<UnlockedHull> _unlockedHullButtons;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(HullsRow hullsRow, IUIFactory uiFactory, IList<Cruiser> unlockedCruisers, Cruiser loadoutCruiser)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);
			Assert.IsTrue(unlockedCruisers.Count > 0);

			_unlockedHullButtons = new List<UnlockedHull>();
			float totalWidth = 0;

			foreach (Cruiser unlockedCruiser in unlockedCruisers)
			{
				bool isInLoadout = object.ReferenceEquals(loadoutCruiser, unlockedCruiser);
				UnlockedHull hullButton = uiFactory.CreateUnlockedHull(layoutGroup, hullsRow, unlockedCruiser, isInLoadout);
				_unlockedHullButtons.Add(hullButton);
				totalWidth += hullButton.Size.x;
			}

			totalWidth += (unlockedCruisers.Count - 1) * layoutGroup.spacing;

			scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
		}

		public void UpdateSelectedHull(Cruiser selectedCruiser)
		{
			foreach (UnlockedHull unlockedHullButton in _unlockedHullButtons)
			{
				unlockedHullButton.OnNewHullSelected(selectedCruiser);
			}
		}
	}
}

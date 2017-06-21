using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildingDetails
{
	public class BaseBuildableDetails : MonoBehaviour 
	{
		private ISpriteFetcher _spriteFetcher;
		protected Buildable _buildable;

		public BuildableStatsController statsController;
		public Text buildableName;
		public Text buildableDescription;
		public Image buildableImage;
		public Image slotImage;

		public void Initialise(ISpriteFetcher spriteFetcher)
		{
			_spriteFetcher = spriteFetcher;
			Hide();
		}

		public virtual void ShowBuildableDetails(Buildable buildable, Buildable buildableToCompareTo = null)
		{
			Assert.IsNotNull(buildable);

			if (_buildable != null)
			{
				CleanUp();
			}

			_buildable = buildable;
			gameObject.SetActive(true);

			statsController.ShowBuildableStats(_buildable, buildableToCompareTo);
			buildableName.text = _buildable.buildableName;
			buildableDescription.text = _buildable.description;
			buildableImage.sprite = _buildable.Sprite;

			bool hasSlot = _buildable.slotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite((SlotType)_buildable.slotType);
			}
			slotImage.gameObject.SetActive(hasSlot);
		}

		public void Hide()
		{
			CleanUp();
			gameObject.SetActive(false);
		}

		protected virtual void CleanUp() { }
	}
}

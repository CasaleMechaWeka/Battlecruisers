using BattleCruisers.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BuildingDetails
{
	public class BuildableDetailsController : MonoBehaviour 
	{
		private Buildable _buildable;
		private bool _allowDelete;
		// FELIX  Inject?
		private SpriteFetcher _spriteFetcher;

		public BuildableStatsController statsController;
		public Text buildableName;
		public Text buildableDescription;
		public Image buildableImage;
		public Image slotImage;
		public Button deleteButton;

		// Use this for initialization
		void Start () 
		{
			_spriteFetcher = new SpriteFetcher();
			_allowDelete = false;
			Hide();
		}

		public void ShowBuildableDetails(Buildable buildable, bool allowDelete)
		{
			Assert.IsNotNull(buildable);

			_buildable = buildable;
			_allowDelete = allowDelete;
			gameObject.SetActive(true);

			statsController.ShowBuildableStats(_buildable);
			buildableName.text = _buildable.buildableName;
			buildableDescription.text = _buildable.description;
			buildableImage.sprite = _buildable.Sprite;

			bool hasSlot = _buildable.slotType != SlotType.None;
			if (hasSlot)
			{
				slotImage.sprite = _spriteFetcher.GetSlotSprite((SlotType)_buildable.slotType);
			}
			slotImage.gameObject.SetActive(hasSlot);

			deleteButton.gameObject.SetActive(allowDelete);
			if (allowDelete)
			{
				deleteButton.onClick.AddListener(DeleteBuildable);
			}
		}

		public void DeleteBuildable()
		{
			Assert.IsTrue(_allowDelete);
			Assert.IsNotNull(_buildable);

			_buildable.InitiateDelete();
			Hide();
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}

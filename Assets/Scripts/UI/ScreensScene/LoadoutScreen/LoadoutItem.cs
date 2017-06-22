using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	// FELIX  Move to own classes
	public interface IComparableItem
	{
		Sprite Sprite { get; }
	}

	public abstract class LoadoutItem<TItem> : MonoBehaviour where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		public Image itemImage;
		public Image selectedFeedbackImage;

		private TItem _item;
		public TItem Item 
		{
			get { return _item; }
			protected set
			{
				Assert.IsFalse(value.Equals(default(TItem)));
				_item = value;
				itemImage.sprite = _item.Sprite;
			}
		}

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

		protected void InternalInitialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
		{
			Item = item;
			_itemDetailsManager = itemDetailsManager;
		}

		public void SelectItem()
		{
			_itemDetailsManager.SelectItem(this);
		}
	}

	// FELIX  Move to own classes
	public class LoadoutBuildableItem : LoadoutItem<Buildable>
	{
		public void Initialise(Buildable buildable, BuildableDetailsManager buildableDetailsManager)
		{
			InternalInitialise(buildable, buildableDetailsManager);
		}
	}

	public class LoadoutHullItem : LoadoutItem<Cruiser>
	{
		public void Initialise(Cruiser hull, CruiserDetailsManager cruiserDetailsManager)
		{
			InternalInitialise(hull, cruiserDetailsManager);
		}

		public void UpdateHull(Cruiser newHull)
		{
			Item = newHull;
		}
	}
}

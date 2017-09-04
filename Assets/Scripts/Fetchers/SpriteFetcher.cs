using System;
using BattleCruisers.Cruisers.Slots;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
    public class SpriteFetcher : ISpriteFetcher
	{
		private const string SLOT_SPRITES_BASE_PATH = "Sprites/Slots/";
		private const string SLOT_SPRITE_NAME_PREFIX = "slot-";

		public Sprite GetSlotSprite(SlotType slotType)
		{
			string spritePath = GetSlotFilePath(slotType);
			Sprite sprite = Resources.Load<Sprite>(spritePath);

			if (sprite == null)
			{
				throw new ArgumentException("Invalid sprite path: " + spritePath);
			}

			return sprite;
		}

		private string GetSlotFilePath(SlotType slotType)
		{
			return SLOT_SPRITES_BASE_PATH + SLOT_SPRITE_NAME_PREFIX + slotType.ToString().ToLower();
		}
	}
}

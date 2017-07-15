using BattleCruisers.Cruisers;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	public interface ISpriteFetcher
	{
		Sprite GetSlotSprite(SlotType slotType);
	}
}

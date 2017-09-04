using BattleCruisers.Cruisers.Slots;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
    public interface ISpriteFetcher
	{
		Sprite GetSlotSprite(SlotType slotType);
	}
}

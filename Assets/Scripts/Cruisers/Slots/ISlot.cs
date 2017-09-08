using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Remove SternTop & BowTop slots?
    public enum SlotType
	{
		None, SternTop, SternBottom, BowTop, BowBottom, Platform, Deck, Utility, Mast
	}

    public interface ISlot
    {
        bool IsFree { get; }
        SlotType Type { get; }
        float XDistanceFromParentCruiser { get; }
        bool IsActive { set; }
        IBuilding Building { set; }
        IBoostConsumer BoostConsumer { get; }
	}
}

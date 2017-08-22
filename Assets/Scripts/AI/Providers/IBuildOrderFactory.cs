using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderFactory
    {
        IDynamicBuildOrder GetBasicBuildOrder(int levelNum, ISlotWrapper slotWrapper);
        IDynamicBuildOrder GetAdaptiveBuildOrder(int levelNum, ISlotWrapper slotWrapper);
		IDynamicBuildOrder CreateAntiAirBuildOrder(int levelNum, ISlotWrapper slotWrapper);
        IDynamicBuildOrder CreateAntiNavalBuildOrder(int levelNum, ISlotWrapper slotWrapper);
	}
}

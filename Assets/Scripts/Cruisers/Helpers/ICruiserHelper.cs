using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;

namespace BattleCruisers.Cruisers.Helpers
{
    public interface ICruiserHelper
    {
        void FocusCameraOnCruiser();
        void OnBuildingConstructionStarted(IBuilding buildingStarted, ISlotAccessor slotAccessor);
    }
}

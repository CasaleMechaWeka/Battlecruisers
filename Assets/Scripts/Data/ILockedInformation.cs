using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Data
{
    public interface ILockedInformation
    {
        int NumOfLevelsUnlocked { get; }
        int NumOfLockedHulls { get; }
        int NumOfLockedBuildings(BuildingCategory buildingCategory);
        int NumOfLockedUnits(UnitCategory unitCategory);
    }
}
    
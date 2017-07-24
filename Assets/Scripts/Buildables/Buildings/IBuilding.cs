namespace BattleCruisers.Buildables.Buildings
{
    public interface IBuilding : IBuildable
    {
        BuildingCategory Category { get; }
		float CustomOffsetProportion { get; }
    }
}
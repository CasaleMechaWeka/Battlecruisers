namespace BattleCruisers.Buildables.Buildings
{
	public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical, Ultra
	}

    public interface IBuilding : IBuildable
    {
        BuildingCategory Category { get; }
		float CustomOffsetProportion { get; }
        bool PreferCruiserFront { get; }
	}
}
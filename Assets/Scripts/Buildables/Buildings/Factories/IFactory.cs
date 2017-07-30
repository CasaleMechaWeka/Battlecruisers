using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public interface IFactory : IBuilding
    {
		UnitCategory UnitCategory { get; }
	}
}
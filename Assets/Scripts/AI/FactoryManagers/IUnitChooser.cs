using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitChooser
	{
        IBuildableWrapper<IUnit> ChooseUnit(int numOfDrones);
	}
}

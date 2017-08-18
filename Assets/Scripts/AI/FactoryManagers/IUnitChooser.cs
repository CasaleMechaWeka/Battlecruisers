using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitChooser
	{
        // FELIX  Take no arguments.  MostExpensiveUnitChooser should have drone manager as a property :)
        IBuildableWrapper<IUnit> ChooseUnit(int numOfDrones);
	}
}

using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitChooser : IManagedDisposable
	{
        event EventHandler ChosenUnitChanged;

        IBuildableWrapper<IUnit> ChosenUnit { get; }
	}
}

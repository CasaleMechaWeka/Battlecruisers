using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitChooser : IDisposable
	{
        event EventHandler ChosenUnitChanged;

        IBuildableWrapper<IUnit> ChosenUnit { get; }
	}
}

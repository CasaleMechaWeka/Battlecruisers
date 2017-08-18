using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public interface IUnitChooser : IDisposable
	{
        IBuildableWrapper<IUnit> ChosenUnit { get; }
	}
}

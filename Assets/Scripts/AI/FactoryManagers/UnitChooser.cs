using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public abstract class UnitChooser : IUnitChooser
    {
        public event EventHandler ChosenUnitChanged;

        private IBuildableWrapper<IUnit> _chosenUnit;
        public IBuildableWrapper<IUnit> ChosenUnit
        {
            get { return _chosenUnit; }
            protected set
            {
                IBuildableWrapper<IUnit> oldChosenUnit = _chosenUnit;
                _chosenUnit = value;

                if (!ReferenceEquals(oldChosenUnit, _chosenUnit))
                {
                    ChosenUnitChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public abstract void DisposeManagedState();
    }
}

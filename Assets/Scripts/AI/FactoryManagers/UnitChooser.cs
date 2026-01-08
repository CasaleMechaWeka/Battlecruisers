using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.AI.FactoryManagers
{
    public abstract class UnitChooser
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

        /// <summary>
        /// Optional hook for choosers that want to rotate the chosen unit after a unit finishes building.
        /// Default behaviour is no-op to preserve existing chooser logic.
        /// </summary>
        public virtual void OnUnitBuilt()
        {
            // no-op
        }
    }
}

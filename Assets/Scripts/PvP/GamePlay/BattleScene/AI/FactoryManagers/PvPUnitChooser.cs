using System;
using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public abstract class PvPUnitChooser : IPvPUnitChooser
    {
        public event EventHandler ChosenUnitChanged;

        private IPvPBuildableWrapper<IPvPUnit> _chosenUnit;
        public IPvPBuildableWrapper<IPvPUnit> ChosenUnit
        {
            get { return _chosenUnit; }
            protected set
            {
                IPvPBuildableWrapper<IPvPUnit> oldChosenUnit = _chosenUnit;
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

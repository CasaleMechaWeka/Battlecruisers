using System;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IPvPUnitChooser : IManagedDisposable
    {
        event EventHandler ChosenUnitChanged;

        IPvPBuildableWrapper<IPvPUnit> ChosenUnit { get; }
    }
}

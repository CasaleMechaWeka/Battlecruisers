using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public interface IPvPUnitChooser : IPvPManagedDisposable
    {
        event EventHandler ChosenUnitChanged;

        IPvPBuildableWrapper<IPvPUnit> ChosenUnit { get; }
    }
}

using BattleCruisers.Buildables.Repairables;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public class PvPRepairCommand : PvPParameterisedCommand<float>, IRepairCommand
    {
        public IRepairable Repairable { get; }

        public PvPRepairCommand(Action<float> action, Func<bool> canExecute, IRepairable repairable)
            : base(action, canExecute)
        {
            Assert.IsNotNull(repairable);
            Repairable = repairable;
        }
    }
}


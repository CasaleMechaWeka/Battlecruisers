using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public class PvPRepairCommand : PvPParameterisedCommand<float>, IPvPRepairCommand
    {
        public IPvPRepairable Repairable { get; }

        public PvPRepairCommand(Action<float> action, Func<bool> canExecute, IPvPRepairable repairable)
            : base(action, canExecute)
        {
            Assert.IsNotNull(repairable);
            Repairable = repairable;
        }
    }
}


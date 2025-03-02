using BattleCruisers.Buildables.Repairables;
using BattleCruisers.UI.Commands;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public class PvPRepairCommand : ParameterisedCommand<float>, IRepairCommand
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


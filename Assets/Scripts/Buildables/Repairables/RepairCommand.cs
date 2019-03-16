using System;
using BattleCruisers.UI.Commands;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Repairables
{
    public class RepairCommand : ParameterisedCommand<float>, IRepairCommand
	{
		public IRepairable Repairable { get; }

        public RepairCommand(Action<float> action, Func<bool> canExecute, IRepairable repairable) 
            : base(action, canExecute)
        {
            Assert.IsNotNull(repairable);
            Repairable = repairable;
        }
	}
}

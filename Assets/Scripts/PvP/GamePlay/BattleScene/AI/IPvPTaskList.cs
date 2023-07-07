using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPTaskList
    {
        bool IsEmpty { get; }
        IPvPPrioritisedTask HighestPriorityTask { get; }

        event EventHandler HighestPriorityTaskChanged;
        event EventHandler IsEmptyChanged;

        void Add(IPvPPrioritisedTask taskToAdd);
        void Remove(IPvPPrioritisedTask taskToRemove);
    }
}

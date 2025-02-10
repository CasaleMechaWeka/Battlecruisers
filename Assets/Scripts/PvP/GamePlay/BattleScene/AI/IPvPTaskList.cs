using System;
using BattleCruisers.AI.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public interface IPvPTaskList
    {
        bool IsEmpty { get; }
        IPrioritisedTask HighestPriorityTask { get; }

        event EventHandler HighestPriorityTaskChanged;
        event EventHandler IsEmptyChanged;

        void Add(IPrioritisedTask taskToAdd);
        void Remove(IPrioritisedTask taskToRemove);
    }
}

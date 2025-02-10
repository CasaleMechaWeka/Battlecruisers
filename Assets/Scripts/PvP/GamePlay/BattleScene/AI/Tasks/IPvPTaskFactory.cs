using BattleCruisers.AI.Tasks;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public interface IPvPTaskFactory
    {
        IPrioritisedTask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey);
    }
}
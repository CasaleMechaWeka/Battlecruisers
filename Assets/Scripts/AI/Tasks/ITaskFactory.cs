using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Tasks
{
    public interface ITaskFactory
    {
        IPrioritisedTask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey);
    }
}
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.AI.Tasks
{
    public interface ITaskFactory
    {
        ITask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey);
    }
}
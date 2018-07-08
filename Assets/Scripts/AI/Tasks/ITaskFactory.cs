using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Tasks
{
    public interface ITaskFactory
    {
        ITask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey);
        ITask CreateWaitForUnitConstructionTask(TaskPriority priority, IFactory factory);
    }
}
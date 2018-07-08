using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Tasks
{
    public interface ITaskFactory
    {
        IPrioritisedTask CreateConstructBuildingTask(TaskPriority taskPriority, IPrefabKey buildingKey);
        IPrioritisedTask CreateWaitForUnitConstructionTask(TaskPriority priority, IFactory factory);
    }
}
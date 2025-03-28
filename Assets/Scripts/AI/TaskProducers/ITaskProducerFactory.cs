using BattleCruisers.AI.BuildOrders;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        ITaskProducer CreateBasicTaskProducer(TaskList tasks, IDynamicBuildOrder buildOrder);
        ITaskProducer CreateReplaceDestroyedBuildingsTaskProducer(TaskList tasks);

        // Anti-<threat type> task producers
        ITaskProducer CreateAntiAirTaskProducer(TaskList tasks, IDynamicBuildOrder antiAirBuildOrder);
        ITaskProducer CreateAntiNavalTaskProducer(TaskList tasks, IDynamicBuildOrder antiNavalBuildOrder);
        ITaskProducer CreateAntiRocketLauncherTaskProducer(TaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder);
        ITaskProducer CreateAntiStealthTaskProducer(TaskList tasks, IDynamicBuildOrder antiStealthBuildOrder);
    }
}

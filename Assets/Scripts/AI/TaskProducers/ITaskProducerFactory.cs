using BattleCruisers.AI.BuildOrders;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        ITaskProducer CreateBasicTaskProducer(ITaskList tasks, IDynamicBuildOrder buildOrder);
		ITaskProducer CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks);

        // Anti-<threat type> task producers
        ITaskProducer CreateAntiAirTaskProducer(ITaskList tasks, IDynamicBuildOrder antiAirBuildOrder);
        ITaskProducer CreateAntiNavalTaskProducer(ITaskList tasks, IDynamicBuildOrder antiNavalBuildOrder);
        ITaskProducer CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder);
        ITaskProducer CreateAntiStealthTaskProducer(ITaskList tasks, IDynamicBuildOrder antiStealthBuildOrder);
	}
}

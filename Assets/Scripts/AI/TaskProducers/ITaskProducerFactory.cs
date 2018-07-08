using BattleCruisers.AI.BuildOrders;

namespace BattleCruisers.AI.TaskProducers
{
    // FELIX  Return, so have a reference, so does not get garbage collected & can dispose.
    public interface ITaskProducerFactory
    {
        void CreateBasicTaskProducer(ITaskList tasks, IDynamicBuildOrder buildOrder);
		void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks);
        void CreatePostFactoryTaskProducer(ITaskList tasks);

        // Anti-<threat type> task producers
        void CreateAntiAirTaskProducer(ITaskList tasks, IDynamicBuildOrder antiAirBuildOrder);
        void CreateAntiNavalTaskProducer(ITaskList tasks, IDynamicBuildOrder antiNavalBuildOrder);
        void CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder);
        void CreateAntiStealthTaskProducer(ITaskList tasks, IDynamicBuildOrder antiStealthBuildOrder);
	}
}

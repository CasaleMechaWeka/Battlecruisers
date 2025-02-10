using BattleCruisers.AI;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public interface IPvPTaskProducerFactory
    {
        ITaskProducer CreateBasicTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder buildOrder);
        ITaskProducer CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks);

        // Anti-<threat type> task producers
        ITaskProducer CreateAntiAirTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiAirBuildOrder);
        ITaskProducer CreateAntiNavalTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiNavalBuildOrder);
        ITaskProducer CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiRocketLauncherBuildOrder);
        ITaskProducer CreateAntiStealthTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiStealthBuildOrder);
    }
}

using BattleCruisers.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public interface IPvPTaskProducerFactory
    {
        IPvPTaskProducer CreateBasicTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder buildOrder);
        IPvPTaskProducer CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks);

        // Anti-<threat type> task producers
        IPvPTaskProducer CreateAntiAirTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiAirBuildOrder);
        IPvPTaskProducer CreateAntiNavalTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiNavalBuildOrder);
        IPvPTaskProducer CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiRocketLauncherBuildOrder);
        IPvPTaskProducer CreateAntiStealthTaskProducer(ITaskList tasks, IPvPDynamicBuildOrder antiStealthBuildOrder);
    }
}

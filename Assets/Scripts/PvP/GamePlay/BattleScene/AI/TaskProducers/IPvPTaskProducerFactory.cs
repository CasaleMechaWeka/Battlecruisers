using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public interface IPvPTaskProducerFactory
    {
        IPvPTaskProducer CreateBasicTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder buildOrder);
        IPvPTaskProducer CreateReplaceDestroyedBuildingsTaskProducer(IPvPTaskList tasks);

        // Anti-<threat type> task producers
        IPvPTaskProducer CreateAntiAirTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiAirBuildOrder);
        IPvPTaskProducer CreateAntiNavalTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiNavalBuildOrder);
        IPvPTaskProducer CreateAntiRocketLauncherTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiRocketLauncherBuildOrder);
        IPvPTaskProducer CreateAntiStealthTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiStealthBuildOrder);
    }
}

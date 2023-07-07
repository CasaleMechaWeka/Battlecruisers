using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public class PvPTaskProducerFactory : IPvPTaskProducerFactory
    {
        private readonly IPvPCruiserController _aiCruiser;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPTaskFactory _taskFactory;
        private readonly IPvPSlotNumCalculatorFactory _slotNumCalculatorFactory;
        private readonly IStaticData _staticData;
        private readonly IPvPThreatMonitorFactory _threatMonitorFactory;

        // For spy satellite launcher and shields.  All cruisers have at least 6
        // deck slots:
        // + Anti-air: 2
        // + Anti-ship: 2
        // + Shields/spy satellite: 2
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 2;

        public PvPTaskProducerFactory(
            IPvPCruiserController aiCruiser,
            IPvPPrefabFactory prefabFactory,
            IPvPTaskFactory taskFactory,
            IPvPSlotNumCalculatorFactory slotNumCalculatorFactory,
            IStaticData staticData,
            IPvPThreatMonitorFactory threatMonitorFactory)
        {
            PvPHelper.AssertIsNotNull(aiCruiser, prefabFactory, taskFactory, slotNumCalculatorFactory, staticData, threatMonitorFactory);

            _aiCruiser = aiCruiser;
            _prefabFactory = prefabFactory;
            _taskFactory = taskFactory;
            _slotNumCalculatorFactory = slotNumCalculatorFactory;
            _staticData = staticData;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IPvPTaskProducer CreateBasicTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder buildOrder)
        {
            return new PvPBasicTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, buildOrder);
        }

        public IPvPTaskProducer CreateReplaceDestroyedBuildingsTaskProducer(IPvPTaskList tasks)
        {
            return new PvPReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, _staticData.BuildingKeys);
        }

        public IPvPTaskProducer CreateAntiAirTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiAirBuildOrder)
        {
            IPvPThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());

            int maxNumOfDeckSlots = PvPHelper.Half(_aiCruiser.SlotAccessor.GetSlotCount(PvPSlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            IPvPSlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            return new PvPAntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

        public IPvPTaskProducer CreateAntiNavalTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiNavalBuildOrder)
        {
            IPvPThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            int maxNumOfDeckSlots = PvPHelper.Half(_aiCruiser.SlotAccessor.GetSlotCount(PvPSlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);
            IPvPSlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiNavalSlotNumCalculator(maxNumOfDeckSlots);

            return new PvPAntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
        }

        public IPvPTaskProducer CreateAntiRocketLauncherTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiRocketLauncherBuildOrder)
        {
            IPvPThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
            IPvPSlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

            return new PvPAntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
        }

        public IPvPTaskProducer CreateAntiStealthTaskProducer(IPvPTaskList tasks, IPvPDynamicBuildOrder antiStealthBuildOrder)
        {
            IPvPThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
            IPvPSlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

            return new PvPAntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiStealthBuildOrder, stealthThreatMonitor, slotNumCalculator);
        }
    }
}

using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Collections;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings;

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
            return new PvPReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, convertPvEBuildingKey2PvPBuildingKey(_staticData.BuildingKeys));
        }

        private IList<PvPBuildingKey> convertPvEBuildingKey2PvPBuildingKey(IList<BuildingKey> keys)
        {
            IList<PvPBuildingKey> iPvPKeys = new List<PvPBuildingKey>();
            foreach (BuildingKey key in keys)
            {
                iPvPKeys.Add(new PvPBuildingKey(convertPvEBuildingCategory2PvPBuildingCategory(key.BuildingCategory), "PvP" + key.PrefabName));
            }

            return iPvPKeys;
        }

        private PvPBuildingCategory convertPvEBuildingCategory2PvPBuildingCategory(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Ultra:
                    return PvPBuildingCategory.Ultra;
                case BuildingCategory.Tactical:
                    return PvPBuildingCategory.Tactical;
                case BuildingCategory.Factory:
                    return PvPBuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return PvPBuildingCategory.Offence;
                case BuildingCategory.Defence:
                    return PvPBuildingCategory.Defence;
                default:
                    throw new System.Exception();
            }
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

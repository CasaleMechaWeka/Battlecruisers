﻿using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Cruisers
{
	public class CruiserFactory : ICruiserFactory
	{
        private readonly IUIManager _uiManager;
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;

		public CruiserFactory(IUIManager uiManager, IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(uiManager, prefabFactory, deferrer);
            
            _uiManager = uiManager;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
        }

        public void InitialiseCruiser(Cruiser cruiser, ICruiser enemyCruiser,
             HealthBarController healthBar, Faction faction, Direction facingDirection)
        {
            IFactoryProvider factoryProvider = new FactoryProvider(_prefabFactory, cruiser, enemyCruiser);
            IDroneManager droneManager = new DroneManager();
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
            RepairManager repairManager = new RepairManager(_deferrer, feedbackFactory);
            new FogOfWarManager(cruiser.Fog, cruiser, enemyCruiser);
            bool shouldShowFog = facingDirection == Direction.Left;

            cruiser.Initialise(faction, enemyCruiser, healthBar, _uiManager, droneManager,
                droneConsumerProvider, factoryProvider, facingDirection, repairManager, shouldShowFog);
        }
	}
}
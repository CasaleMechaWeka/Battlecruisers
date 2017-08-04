﻿using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers
{
	public class CruiserFactory : ICruiserFactory
	{
        private readonly IUIManager _uiManager;
        private readonly IPrefabFactory _prefabFactory;

        public CruiserFactory(IUIManager uiManager, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(uiManager, prefabFactory);
            
            _uiManager = uiManager;
            _prefabFactory = prefabFactory;
        }

        // FELIX  Use interface for:
        // + enemyCruiser
        // + uiManager
        public void InitialiseCruiser(Cruiser cruiser, ICruiser enemyCruiser, 
             HealthBarController healthBar, Faction faction, Direction facingDirection)
        {
            IFactoryProvider factoryProvider = new FactoryProvider(_prefabFactory, cruiser, enemyCruiser);
            IDroneManager droneManager = new DroneManager();
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            cruiser.Initialise(faction, enemyCruiser, healthBar, _uiManager, droneManager,
                droneConsumerProvider, factoryProvider, facingDirection);
        }
	}
}
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
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;
        private readonly ISpriteProvider _spriteProvider;

        public CruiserFactory(IPrefabFactory prefabFactory, IDeferrer deferrer, ISpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, spriteProvider);
            
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _spriteProvider = spriteProvider;
        }

        public void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser,
            HealthBarController healthBar, 
            IUIManager uiManager,
            Faction faction, 
            Direction facingDirection)
        {
            Helper.AssertIsNotNull(cruiser, enemyCruiser, healthBar, uiManager);

            IFactoryProvider factoryProvider = new FactoryProvider(_prefabFactory, cruiser, enemyCruiser, _spriteProvider);
            IDroneManager droneManager = new DroneManager();
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
            RepairManager repairManager = new RepairManager(_deferrer, feedbackFactory);
            new FogOfWarManager(cruiser.Fog, cruiser, enemyCruiser);
            bool shouldShowFog = facingDirection == Direction.Left;

            ICruiserArgs cruiserArgs 
                = new CruiserArgs(
                    faction, 
                    enemyCruiser, 
                    healthBar, 
                    uiManager, 
                    droneManager,
                    droneConsumerProvider, 
                    factoryProvider, 
                    facingDirection, 
                    repairManager, 
                    shouldShowFog);

            cruiser.Initialise(cruiserArgs);
        }
	}
}
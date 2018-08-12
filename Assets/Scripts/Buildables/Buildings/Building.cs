using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public class Building : Buildable, IBuilding
	{
        private BoxCollider2D _collider;

        protected ISlot _parentSlot;

		public BuildingCategory category;
		// Proportional to building size
		public float customOffsetProportion;
        public bool preferCruiserFront;

        public override TargetType TargetType { get { return TargetType.Buildings; } }
        public override Vector2 Size { get { return _collider.size; } }
        public BuildingCategory Category { get { return category; } }
        public float CustomOffsetProportion { get { return customOffsetProportion; } }
        public bool PreferCruiserFront { get { return preferCruiserFront; } }
        
        public SlotType slotType;
        public SlotType SlotType { get { return slotType; } }

        protected override HealthBarController HealthBarController
        {
            get
            {
                BuildingWrapper buildableWrapper = gameObject.GetComponentInInactiveParent<BuildingWrapper>();
                return buildableWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
            }
        }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            _collider = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(_collider);
        }

        public void Initialise(
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IFactoryProvider factoryProvider,
            ISlot parentSlot)
        {
            base.Initialise(parentCruiser, enemyCruiser, uiManager, factoryProvider);

            Assert.IsNotNull(parentSlot);

            _parentSlot = parentSlot;
            _boostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);

            OnInitialised();
		}

        protected override void OnSingleClick()
        {
            _uiManager.SelectBuilding(this);
        }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();

            // FELIX  Create double click handler?  Have Player-/AI- handlers, will avoid ugly Faction check :/
            // Only allow double clicks to control player cruiser drones :P
            if (Faction == Faction.Blues)
            {
                // Toggle drone consumer focus on double click :)
                if (BuildableState == BuildableState.NotStarted
                    || BuildableState == BuildableState.InProgress
                    || BuildableState == BuildableState.Paused)
                {
                    _droneManager.ToggleDroneConsumerFocus(DroneConsumer);
                }
                // Toggle repair drone consumer focus
                else if (BuildableState == BuildableState.Completed
                    && RepairCommand.CanExecute)
                {
                    IDroneConsumer repairDroneConsumer = ParentCruiser.RepairManager.GetDroneConsumer(this);
                    _droneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
                }
            }

            // Set as user chosen target, to make everything attack this building
            _factoryProvider.TargetsFactory.UserChosenTargetHelper.ToggleChosenTarget(this);
        }
    }
}

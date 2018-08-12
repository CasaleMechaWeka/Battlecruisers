using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public class Building : Buildable, IBuilding
	{
        private BoxCollider2D _collider;

        private IBuildingDoubleClickHandler _doubleClickHandler;
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
            ISlot parentSlot,
            IBuildingDoubleClickHandler doubleClickHandler)
        {
            base.Initialise(parentCruiser, enemyCruiser, uiManager, factoryProvider);

            Helper.AssertIsNotNull(parentCruiser, doubleClickHandler);

            _parentSlot = parentSlot;
            _doubleClickHandler = doubleClickHandler;
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
            _doubleClickHandler.OnDoubleClick(this);
        }
    }
}

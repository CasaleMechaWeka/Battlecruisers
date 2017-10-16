using System;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Buildables.Buildings
{
    public class Building : Buildable, IPointerClickHandler, IBuilding
	{
        protected ISlot _parentSlot;

		public BuildingCategory category;
		// Proportional to building size
		public float customOffsetProportion;
        public bool preferCruiserFront;

		public override TargetType TargetType { get { return TargetType.Buildings; } }
        public BuildingCategory Category { get { return category; } }
		public float CustomOffsetProportion { get { return customOffsetProportion; } }
        public bool PreferCruiserFront { get { return preferCruiserFront; } }

		protected override HealthBarController HealthBarController
		{
			get
			{
                BuildingWrapper buildableWrapper = gameObject.GetComponentInInactiveParent<BuildingWrapper>();
				return buildableWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
			}
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

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.SelectBuilding(this, _parentCruiser);
			OnClicked();
		}

		protected virtual void OnClicked() { }

		public override void InitiateDelete()
		{
			Destroy();
		}

		public void CancelDelete()
		{
            // FELIX  Implement and use :)
			throw new NotImplementedException();
		}
	}
}

using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public abstract class Building : Buildable, IBuilding
	{
        private BoxCollider2D _collider;

        private IDoubleClickHandler<IBuilding> _doubleClickHandler;
        protected ISlot _parentSlot;

        public BuildingFunction function;
        public bool preferCruiserFront;
        public SlotType slotType;

        public override TargetType TargetType => TargetType.Buildings;
        public override Vector2 Size => _collider.size;
        public SlotSpecification SlotSpecification { get; private set; }
        public Vector3 PuzzleRootPoint { get; private set; }

        public BuildingCategory category;
        public BuildingCategory Category => category;

        // FELIX  Remove :)
        public Transform tempPuzzleRootPoint;

        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building1;

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

            SlotSpecification = new SlotSpecification(slotType, function, preferCruiserFront);

            // FELIX  Avoid if, once all prefabs have this :)
            //Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            if (tempPuzzleRootPoint != null)
            {
                PuzzleRootPoint = tempPuzzleRootPoint.position;
            }
        }

        public void Initialise(
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IUIManager uiManager, 
            IFactoryProvider factoryProvider,
            ISlot parentSlot,
            IDoubleClickHandler<IBuilding> doubleClickHandler)
        {
            base.Initialise(parentCruiser, enemyCruiser, uiManager, factoryProvider);

            Helper.AssertIsNotNull(parentCruiser, doubleClickHandler);

            _parentSlot = parentSlot;
            _doubleClickHandler = doubleClickHandler;
            _localBoosterBoostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);

            OnInitialised();
		}

        protected override void OnSingleClick()
        {
            Logging.LogDefault(Tags.BUILDING);
            _uiManager.SelectBuilding(this);
        }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();
            _doubleClickHandler.OnDoubleClick(this);
        }
    }
}

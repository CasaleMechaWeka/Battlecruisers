using BattleCruisers.Buildables.ActivationArgs;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public abstract class Building : Buildable<BuildingActivationArgs>, IBuilding
	{
        private BoxCollider2D _collider;

        private IDoubleClickHandler<IBuilding> _doubleClickHandler;
        protected ISlot _parentSlot;

        public BuildingFunction function;
        public bool preferCruiserFront;
        public SlotType slotType;

        public override TargetType TargetType => TargetType.Buildings;
        public SlotSpecification SlotSpecification { get; private set; }
        public Vector3 PuzzleRootPoint { get; private set; }

        public BuildingCategory category;
        public BuildingCategory Category => category;

        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Building1;

        protected override HealthBarController HealthBarController
        {
            get
            {
                BuildingWrapper buildableWrapper = gameObject.GetComponentInInactiveParent<BuildingWrapper>();
                return buildableWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
            }
        }

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _collider = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(_collider);

            SlotSpecification = new SlotSpecification(slotType, function, preferCruiserFront);

            Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            PuzzleRootPoint = puzzleRootPoint.position;
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _parentSlot = activationArgs.ParentSlot;
            _doubleClickHandler = activationArgs.DoubleClickHandler;
            _localBoosterBoostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);
        }

        protected override void OnSingleClick()
        {
            Logging.LogMethod(Tags.BUILDING);
            _uiManager.SelectBuilding(this);
        }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();
            _doubleClickHandler.OnDoubleClick(this);
        }
    }
}

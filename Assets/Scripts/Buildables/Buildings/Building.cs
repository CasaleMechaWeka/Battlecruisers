using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public abstract class Building : Buildable<BuildingActivationArgs>, IBuilding
	{
        private BoxCollider2D _collider;

        private IDoubleClickHandler<IBuilding> _doubleClickHandler;
        protected ISlot _parentSlot;

        private IAudioClipWrapper _placementSound;
        public AudioClip placementSound;

        [Header("Slots")]
        public BuildingFunction function;
        public bool preferCruiserFront;
        public SlotType slotType;

        public override TargetType TargetType => TargetType.Buildings;
        public ISlotSpecification SlotSpecification { get; private set; }
        public Vector3 PuzzleRootPoint { get; private set; }

        [Header("Other")]
        public BuildingCategory category;
        public BuildingCategory Category => category;

        public virtual bool IsBoostable => false;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _collider = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(_collider);

            SlotSpecification = new SlotSpecification(slotType, function, preferCruiserFront);

            Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            PuzzleRootPoint = puzzleRootPoint.position;

            Assert.IsNotNull(placementSound);
            _placementSound = new AudioClipWrapper(placementSound);

            Name = _commonStrings.GetString($"Buildables/Buildings/{stringKeyName}Name");
            Description = _commonStrings.GetString($"Buildables/Buildings/{stringKeyName}Description");
        }

        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _parentSlot = activationArgs.ParentSlot;
            _doubleClickHandler = activationArgs.DoubleClickHandler;
            _localBoosterBoostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);
        }

        public override void StartConstruction()
        {
            base.StartConstruction();

            if (ParentCruiser.IsPlayerCruiser)
            {
                _factoryProvider.Sound.UISoundPlayer.PlaySound(_placementSound);
            }
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

        public void Activate(BuildingActivationArgs activationArgs, Faction faction)
        {
        }
    }
}

using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using Unity.Netcode;
using BattleCruisers.Data.Static;
using BattleCruisers.Data;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings
{
    public abstract class PvPBuilding : PvPBuildable<PvPBuildingActivationArgs>, IPvPBuilding
    {
        private Collider2D _collider;

        [SerializeField]
        private List<GameObject> additionalRenderers = new List<GameObject>(); // Added for handling additional renderers
        private IPvPDoubleClickHandler<IPvPBuilding> _doubleClickHandler;
        protected IPvPSlot _parentSlot;

        private IAudioClipWrapper _placementSound;
        public AudioClip placementSound;

        [Header("Slots")]
        public PvPBuildingFunction function;
        public bool preferCruiserFront;
        public PvPSlotType slotType;

        public override TargetType TargetType => TargetType.Buildings;
        public IPvPSlotSpecification SlotSpecification { get; private set; }
        public Vector3 PuzzleRootPoint { get; private set; }

        [Header("Other")]
        public BuildingCategory category;
        public BuildingCategory Category => category;

        public virtual bool IsBoostable => false;

        private bool isImmune = false;
        private NetworkVariable<int> pvp_variantIndex = new NetworkVariable<int>();
        private bool isAppliedVariant = false;
        public int variantIndex
        {
            get { return pvp_variantIndex.Value; }
            set
            {
                if (IsHost)
                    pvp_variantIndex.Value = value;
            }
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            SlotSpecification = new PvPSlotSpecification(slotType, function, preferCruiserFront);

            Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            PuzzleRootPoint = puzzleRootPoint.position;

            Assert.IsNotNull(placementSound);
            _placementSound = new AudioClipWrapper(placementSound);

            Name = _commonStrings.GetString($"Buildables/Buildings/{stringKeyName}Name");
            Description = _commonStrings.GetString($"Buildables/Buildings/{stringKeyName}Description");

            if (!IsHost)
            {
                _doubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            }

            foreach (var renderer in additionalRenderers)
            {
                SetRendererVisibility(renderer, false);
            }
        }

        private void SetRendererVisibility(GameObject obj, bool isVisible)
        {
            if (obj != null)
            {
                SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in spriteRenderers)
                {
                    sr.enabled = isVisible;
                }
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsHost)
                pvp_variantIndex.OnValueChanged += ApplyVariantIconOnClient;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsHost)
                pvp_variantIndex.OnValueChanged -= ApplyVariantIconOnClient;
        }

        private async void ApplyVariantIconOnClient(int oldVal, int newVal)
        {
            if (newVal != -1)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(newVal));
                HealthBar.variantIcon.sprite = variant.variantSprite;
                HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                HealthBar.variantIcon.enabled = true;
            }
        }


        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            _placementSound = new AudioClipWrapper(placementSound);
        }

        public override void Activate(PvPBuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _parentSlot = activationArgs.ParentSlot;
            _doubleClickHandler = activationArgs.DoubleClickHandler;
            _localBoosterBoostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);


            variantIndex = activationArgs.VariantIndex;
            // HealthBar.variantIcon.sprite = null;
            HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 0f);
            HealthBar.variantIcon.enabled = false;

            if (!isAppliedVariant)
            {
                ApplyVariantPvP(this, variantIndex);
                isAppliedVariant = true;
            }
            else
            {
                if (variantIndex != -1)
                {
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                    HealthBar.variantIcon.enabled = true;
                }
            }
        }

        public override void Activate_PvPClient()
        {
            base.Activate_PvPClient();
        }

        public async void ApplyVariantPvP(IPvPBuilding building, int variant_index)
        {
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            if (variant_index != -1)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variant_index));
                if (variant != null)
                {
                    HealthBar.variantIcon.sprite = variant.variantSprite;
                    HealthBar.variantIcon.enabled = true;
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 1f);
                    Name = _commonStrings.GetString(dataProvider.StaticData.Variants[variant_index].VariantNameStringKeyBase);
                    Description = _commonStrings.GetString(dataProvider.StaticData.Variants[variant_index].VariantDescriptionStringKeyBase);
                    ApplyVariantStats(variant.statVariant);
                }
                else
                {
                    HealthBar.variantIcon.sprite = null;
                    HealthBar.variantIcon.color = new Color(HealthBar.variantIcon.color.r, HealthBar.variantIcon.color.g, HealthBar.variantIcon.color.b, 0f);
                    HealthBar.variantIcon.enabled = false;
                }
            }
        }

        public void ApplyVariantStats(StatVariant statVariant)
        {
            maxHealth *= statVariant.max_health;
            pvp_Health.Value = maxHealth;
            numOfDronesRequired += statVariant.drone_num;
            buildTimeInS += statVariant.build_time;

            _healthTracker.OverrideHealth(maxHealth);
            _healthTracker.OverrideMaxHealth(maxHealth);
            _buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
            HealthGainPerDroneS = maxHealth / _buildTimeInDroneSeconds;

            HealthBar.OverrideHealth(this);
        }

        public override void StartConstruction()
        {
            base.StartConstruction();

            foreach (var renderer in additionalRenderers)
            {
                SetRendererVisibility(renderer, false);
            }
        }

        protected override void OnBuildableCompleted()
        {

            base.OnBuildableCompleted();
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
            // _coreEngineAudioSource.Play(isSpatial: true, loop: true);
            foreach (var renderer in additionalRenderers)
            {
                SetRendererVisibility(renderer, true);
            }
        }
        protected override void OnBuildableCompleted_PvPClient()
        {
            base.OnBuildableCompleted_PvPClient();
            _smokeInitialiser.Initialise(this, ShowSmokeWhenDestroyed);
        }

        public void AddAdditionalRenderer(GameObject renderer)
        {
            if (!additionalRenderers.Contains(renderer))
            {
                additionalRenderers.Add(renderer);
            }
        }

        protected virtual void PlayPlacementSound()
        {
            if (IsClient && IsOwner)
            {
                _factoryProvider.Sound.UISoundPlayer.PlaySound(_placementSound);
            }
        }



        protected override void OnSingleClick()
        {
            // Logging.LogMethod(Tags.BUILDING);
            _uiManager.SelectBuilding(this);
        }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();
            _doubleClickHandler.OnDoubleClick(this);
        }

        public void Activate(PvPBuildingActivationArgs activationArgs, Faction faction)
        {
        }

        public override void SetBuildingImmunity(bool boo)
        {
            isImmune = boo;
        }

        public override bool IsBuildingImmune()
        {
            return isImmune;
        }

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders);
        }
    }
}

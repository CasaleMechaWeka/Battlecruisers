using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Buildables.Buildings
{
    public abstract class Building : Buildable<BuildingActivationArgs>, IBuilding
    {
        private Collider2D _collider;

        private IDoubleClickHandler<IBuilding> _doubleClickHandler;
        protected ISlot _parentSlot;

        private AudioClipWrapper _placementSound;
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

        private bool isImmune = false;
        public int variantIndex { get; set; }
        private bool isAppliedVariant = false;
        [SerializeField]
        private List<GameObject> additionalRenderers = new List<GameObject>();

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _collider = GetComponent<Collider2D>();
            Assert.IsNotNull(_collider);

            SlotSpecification = new SlotSpecification(slotType, function, preferCruiserFront);

            Transform puzzleRootPoint = transform.FindNamedComponent<Transform>("PuzzleRootPoint");
            PuzzleRootPoint = puzzleRootPoint.position;

            Assert.IsNotNull(placementSound);
            _placementSound = new AudioClipWrapper(placementSound);

            Name = LocTableCache.CommonTable.GetString($"Buildables/Buildings/{stringKeyName}Name");
            Description = LocTableCache.CommonTable.GetString($"Buildables/Buildings/{stringKeyName}Description");
            variantIndex = -1;

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

        public void OverwriteComparableItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public override void Activate(BuildingActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _parentSlot = activationArgs.ParentSlot;
            _doubleClickHandler = activationArgs.DoubleClickHandler;
            _localBoosterBoostableGroup.AddBoostProvidersList(_parentSlot.BoostProviders);
            HealthBar.variantIcon.enabled = false;
            if (ParentCruiser.IsPlayerCruiser && !isAppliedVariant)
            {
                // Set variant for Player
                ApplyVariantToPlayer(this);
                isAppliedVariant = true;
            }
            else if (!ParentCruiser.IsPlayerCruiser && !isAppliedVariant)
            {
                // Set variant for AI
                if (ApplicationModelProvider.ApplicationModel.Mode == GameMode.CoinBattle && UnityEngine.Random.Range(0, 5) == 2)
                {
                    ApplyRandomeVariantToAI(this);
                    isAppliedVariant = true;
                }
            }
        }

        public void ApplyRandomeVariantToAI(IBuilding building)
        {
            int randomID = GetRandomVariantForAI(building);
            if (randomID != -1)
            {
                VariantPrefab variant = _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(randomID));
                if (variant != null)
                {
                    DataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
                    // apply icon, name and description
                    HealthBar.variantIcon.sprite = variant.variantSprite;
                    HealthBar.variantIcon.enabled = true;
                    variantIndex = randomID;
                    Name = LocTableCache.CommonTable.GetString(dataProvider.StaticData.Variants[randomID].VariantNameStringKeyBase);
                    Description = LocTableCache.CommonTable.GetString(dataProvider.StaticData.Variants[randomID].VariantDescriptionStringKeyBase);

                    // apply variant stats for building (maxhealth, numof drones required, build time)
                    ApplyVariantStats(variant.statVariant);
                }
                else
                {
                    HealthBar.variantIcon.enabled = false;
                    variantIndex = -1;
                }
            }
        }

        private int GetRandomVariantForAI(IBuilding building)
        {
            int variant_ID = -1;
            DataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            List<int> ids = new List<int>();
            for (int i = 0; i < dataProvider.StaticData.Variants.Count; i++)
            {
                VariantPrefab variant = _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(i));
                if (variant != null)
                {
                    if (building.PrefabName.ToUpper().Replace("(CLONE)", "") == variant.GetPrefabKey().PrefabName.ToUpper())
                    {
                        ids.Add(i);
                    }
                }
            }

            if (ids.Count != 0)
            {
                variant_ID = ids[UnityEngine.Random.Range(0, ids.Count)];
            }
            return variant_ID;
        }

        public void ApplyVariantToPlayer(IBuilding building)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            VariantPrefab variant = applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariant(_factoryProvider.PrefabFactory, building);

            if (variant != null)
            {
                // apply icon, name and description
                HealthBar.variantIcon.sprite = variant.variantSprite;
                HealthBar.variantIcon.enabled = true;
                int index = applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_factoryProvider.PrefabFactory, building);
                variantIndex = index;
                Name = LocTableCache.CommonTable.GetString(applicationModel.DataProvider.StaticData.Variants[index].VariantNameStringKeyBase);
                Description = LocTableCache.CommonTable.GetString(applicationModel.DataProvider.StaticData.Variants[index].VariantDescriptionStringKeyBase);

                // apply variant stats for building (maxhealth, numof drones required, build time)
                ApplyVariantStats(variant.statVariant);
            }
            else
            {
                HealthBar.variantIcon.enabled = false;
                variantIndex = -1;
            }
        }

        public void ApplyVariantStats(StatVariant statVariant)
        {
            maxHealth *= statVariant.max_health;
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

            if (ParentCruiser.IsPlayerCruiser)
            {
                _factoryProvider.Sound.UISoundPlayer.PlaySound(_placementSound);
            }

            foreach (var renderer in additionalRenderers)
            {
                SetRendererVisibility(renderer, false);
            }
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            // Show all additional renderers once construction is completed
            foreach (var renderer in additionalRenderers)
            {
                SetRendererVisibility(renderer, true);
            }
        }

        public void AddAdditionalRenderer(GameObject renderer)
        {
            if (!additionalRenderers.Contains(renderer))
            {
                additionalRenderers.Add(renderer);
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

        public override void SetBuildingImmunity(bool boo)
        {
            isImmune = boo;
        }

        public override bool IsBuildingImmune()
        {
            return isImmune;
        }

        protected override void AddHealthBoostProviders(IGlobalBoostProviders globalBoostProviders, IList<ObservableCollection<IBoostProvider>> healthBoostProvidersList)
        {
            base.AddHealthBoostProviders(globalBoostProviders, healthBoostProvidersList);
            healthBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders);
        }

        protected override void AddBuildRateBoostProviders(
    IGlobalBoostProviders globalBoostProviders,
    IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders);
        }
    }
}

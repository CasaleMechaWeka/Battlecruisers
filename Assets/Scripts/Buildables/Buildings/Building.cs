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
using BattleCruisers.Utils.Factories;
using static BattleCruisers.Effects.Smoke.StaticSmokeStats;
using System.Configuration;
using static UnityEditor.UIElements.ToolbarMenu;
using System.Threading.Tasks;
using BattleCruisers.Data.Static;
//using Unity.Tutorials.Core.Editor;

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

        private bool isImmune = false;
        public int variantIndex { get; set; }

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
            variantIndex = -1;
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
            if (ParentCruiser.IsPlayerCruiser)
            {
                // Set variant for Player
                ApplyVariantToPlayer(this);
            }
            else
            {
                // Set variant for AI
            //    if (ApplicationModelProvider.ApplicationModel.Mode == GameMode.CoinBattle && UnityEngine.Random.Range(0, 5) == 2)
                    ApplyRandomeVariantToAI(this);
            }
        }

        public async void ApplyRandomeVariantToAI(IBuilding building)
        {
            int randomID = await GetRandomVariantForAI(building);
            if (randomID != -1)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(randomID));
                if (variant != null)
                {
                    IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
                    // apply icon, name and description
                    HealthBar.variantIcon.sprite = variant.variantSprite;
                    HealthBar.variantIcon.enabled = true;
                    variantIndex = randomID;
                    Name = _commonStrings.GetString(dataProvider.GameModel.Variants[randomID].VariantNameStringKeyBase);
                    Description = _commonStrings.GetString(dataProvider.GameModel.Variants[randomID].VariantDescriptionStringKeyBase);

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

        private async Task<int> GetRandomVariantForAI(IBuilding building)
        {
            int variant_ID = -1;
            IDataProvider dataProvider = ApplicationModelProvider.ApplicationModel.DataProvider;
            List<int> ids = new List<int>();
            for (int i = 0; i < dataProvider.GameModel.Variants.Count; i++)
            {
                VariantPrefab variant = await _factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(i));
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

        public async void ApplyVariantToPlayer(IBuilding building)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            VariantPrefab variant = await applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariant(_factoryProvider.PrefabFactory, building);

            if (variant != null)
            {
                // apply icon, name and description
                HealthBar.variantIcon.sprite = variant.variantSprite;
                HealthBar.variantIcon.enabled = true;
                int index = await applicationModel.DataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(_factoryProvider.PrefabFactory, building);
                variantIndex = index;
                Name = _commonStrings.GetString(applicationModel.DataProvider.GameModel.Variants[index].VariantNameStringKeyBase);
                Description = _commonStrings.GetString(applicationModel.DataProvider.GameModel.Variants[index].VariantDescriptionStringKeyBase);

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
            maxHealth += statVariant.max_health;
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

        protected override void AddBuildRateBoostProviders(
    IGlobalBoostProviders globalBoostProviders,
    IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders);
        }
    }
}

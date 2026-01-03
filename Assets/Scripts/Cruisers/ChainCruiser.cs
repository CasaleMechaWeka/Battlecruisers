using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetTrackers;
using System.Linq;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class ChainCruiser : Cruiser
    {
        [Header("Multi-Hull Configuration")]
        [Tooltip("Individual hulls that can be independently targeted and selected.")]
        public Hull[] Hulls;


        private Hull _primaryHull;
        private ITargetConsumer _userChosenTargetConsumer;


        // Override Color to apply to all hulls (ChainCruiser has no root renderer)
        public override Color Color {
            set {
                // Apply color to all hulls
                if (Hulls != null) {
                    foreach (var hull in Hulls) {
                        if (hull != null) {
                            hull.Color = value;
                        }
                    }
                }
            }
        }

        // Hide base Sprite to return primary hull sprite (base not virtual)
        public new Sprite Sprite {
            get {
                // Return primary hull sprite, or first available
                if (_primaryHull != null && _primaryHull.SpriteRenderer != null) {
                    return _primaryHull.SpriteRenderer.sprite;
                }

                foreach (var hull in Hulls) {
                    if (hull != null && hull.SpriteRenderer != null && hull.SpriteRenderer.sprite != null) {
                        return hull.SpriteRenderer.sprite;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the primary hull's health for UI compatibility.
        /// </summary>
        public new float Health => _primaryHull?.Health ?? 0;

        public new float MaxHealth => _primaryHull?.MaxHealth ?? maxHealth;

        public new bool IsDestroyed => _primaryHull?.IsDestroyed ?? true;

        public override bool IsAlive => _primaryHull != null && !_primaryHull.IsDestroyed;

        // Override Size for ChainCruiser - use primary hull collider only (for camera zoom consistency)
        public override Vector2 Size
        {
            get
            {
                // Use primary hull's collider for camera zoom calculations
                if (_primaryHull != null && _primaryHull.PrimaryCollider != null)
                {
                    return (Vector2)_primaryHull.PrimaryCollider.bounds.size;
                }

                return Vector2.one; // Fallback size
            }
        }

        public new void MakeInvincible()
        {
            if (Hulls != null)
            {
                foreach (var hull in Hulls)
                {
                    hull?.MakeInvincible();
                }
            }
        }

        public new void MakeDamagable()
        {
            if (Hulls != null)
            {
                foreach (var hull in Hulls)
                {
                    hull?.MakeDamageable();
                }
            }
        }

        // Override to handle ChainCruiser-specific destruction behavior
        protected override void OnDestroyed()
        {
            base.OnDestroyed(); // Call base first to handle standard persistent objects and destruction
        }

        // Override FixedUpdate to add null check for _enemyCruiser (prevents crash during initialization)
        public override void FixedUpdate()
        {
            // Add null check before accessing _enemyCruiser.IsAlive
            if (IsPlayerCruiser && _enemyCruiser != null && _enemyCruiser.IsAlive)
            {
                BattleSceneGod.AddPlayedTime(TargetType.PlayedTime, _time.DeltaTime);
            }

            if (RepairManager != null)
            {
                RepairManager.Repair(_time.DeltaTime);
            }
        }

        // Multi-hull specific events
        public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;
        public event EventHandler<HullSectionDestroyedEventArgs> SecondaryHullDestroyed;

        public override void StaticInitialise()
        {
            // Find primary hull BEFORE base.StaticInitialise so maxHealth is set correctly
            if (Hulls != null && Hulls.Length > 0)
            {
                _primaryHull = System.Linq.Enumerable.FirstOrDefault(Hulls, h => h != null && h.IsPrimary);

                if (_primaryHull != null)
                {
                    // Set ChainCruiser's maxHealth to primary hull's maxHealth
                    // This ensures the inherited health bar UI works correctly
                    maxHealth = _primaryHull.maxHealth;
                }
            }

            // ChainCruiser doesn't need its own SpriteRenderer (hulls have sprites)
            // Initialize SlotWrapperController
            if (SlotWrapperController == null)
            {
                SlotWrapperController = GetComponentInChildren<SlotWrapperController>(includeInactive: true);
            }

            if (SlotWrapperController != null)
            {
                SlotWrapperController.StaticInitialise();
                SlotNumProvider = SlotWrapperController;
            }

            // Initialize fog
            _fog = GetComponentInChildren<FogOfWar>(includeInactive: true);
            Assert.IsNotNull(_fog, $"[ChainCruiser] {name}: No FogOfWar component found!");

            // Initialize click handler
            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            if (clickHandlerWrapper != null)
            {
                _clickHandler = clickHandlerWrapper.GetClickHandler();
            }

            // Initialize name and description
            Name = LocTableCache.CommonTable.GetString($"Cruisers/{stringKeyBase}Name");
            Description = LocTableCache.CommonTable.GetString($"Cruisers/{stringKeyBase}Description");

            // Initialize monitors
            BuildingMonitor = new CruiserBuildingMonitor(this);
            UnitMonitor = new CruiserUnitMonitor(BuildingMonitor);
            PopulationLimitMonitor = new PopulationLimitMonitor(UnitMonitor);
            UnitTargets = new UnitTargets(UnitMonitor);

            // Calculate drone area size
            _droneAreaSize = new Vector2(Size.x, Size.y * 0.8f);

            // Validate
            if (_primaryHull == null)
            {
                Debug.LogError($"[ChainCruiser] {name}: No primary hull found!");
            }

            Debug.Log($"[ChainCruiser] {name} StaticInitialise - Primary: {_primaryHull?.HullId}, Hulls: {Hulls?.Length}");
        }

        public override async void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            // Set up hull targeting for ChainCruisers
            _userChosenTargetConsumer = (ITargetConsumer)CruiserSpecificFactories.Targets.UserChosenTargetTracker;

            // Initialize all hulls
            if (Hulls != null)
            {
                foreach (var hull in Hulls)
                {
                    if (hull != null)
                    {
                        hull.Initialize();
                        hull.Destroyed += OnHullDestroyedEvent;
                    }
                }
            }

            Debug.Log($"[ChainCruiser] {name} Initialise complete");
        }

        private void OnHullDestroyedEvent(object sender, DestroyedEventArgs e)
        {
            // Event handler - actual logic in OnHullDestroyed
        }



        /// <summary>
        /// Called when any hull is clicked. Routes the click to the main cruiser selection.
        /// </summary>
        public void OnHullClicked(Hull hull)
        {
            // Reuse the inherited click behavior
            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();
            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);
            // Clicked event is handled by base class - don't invoke it directly
        }

        /// <summary>
        /// Called when any hull is double-clicked. Routes to cruiser double-click handler.
        /// </summary>
        public void OnHullDoubleClicked(Hull hull)
        {
            _cruiserDoubleClickHandler?.OnDoubleClick(this);
        }

        /// <summary>
        /// Called when any hull is triple-clicked. Used for hull-specific targeting.
        /// </summary>
        public void OnHullTripleClicked(Hull hull)
        {
            Logging.Log(Tags.CRUISER, $"Hull {hull.HullId} triple-clicked for targeting");

            // Raise event to notify listeners that a hull should be targeted
            HullSectionTargeted?.Invoke(this, new HullSectionTargetedEventArgs(hull));

            // Also call the cruiser double-click handler for standard targeting behavior
            _cruiserDoubleClickHandler?.OnDoubleClick(this);
        }

        /// <summary>
        /// Called by Hull when it is destroyed.
        /// </summary>
        public void OnHullDestroyed(Hull hull)
        {
            Debug.Log($"[ChainCruiser] Hull destroyed: {hull.HullId}, IsPrimary: {hull.IsPrimary}");

            if (hull.IsPrimary)
            {
                // Primary hull death = game over
                // Trigger the inherited death flow from Cruiser/Target
                Destroy();
            }
            else
            {
                // Secondary hull death = battle continues
                SecondaryHullDestroyed?.Invoke(this, new HullSectionDestroyedEventArgs(hull));

                // Add partial destruction score
                if (Faction == Faction.Reds)
                {
                    BattleSceneGod.AddDeadBuildable(TargetType.Buildings, (int)(hull.maxHealth * 0.3f));
                }
            }
        }


        /// <summary>
        /// Gets the health tracker for sharing with hulls.
        /// </summary>
        public IHealthTracker GetHealthTracker()
        {
            return _healthTracker;
        }

    }
}


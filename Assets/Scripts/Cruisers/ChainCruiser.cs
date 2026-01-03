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


        // Note: Color, Sprite, Health, MaxHealth, IsDestroyed, IsAlive, Size, MakeInvincible, MakeDamagable
        // are all handled by base Cruiser class now - no need to override them here!

        // Override to handle ChainCruiser-specific destruction behavior
        protected override void OnDestroyed()
        {
            base.OnDestroyed(); // Call base first to handle standard persistent objects and destruction
        }

        // Note: FixedUpdate is handled by base Cruiser class - no override needed!

        // Multi-hull specific events
        public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;
        public event EventHandler<HullSectionDestroyedEventArgs> SecondaryHullDestroyed;

        public override void StaticInitialise()
        {
            // Find primary hull for reference
            if (Hulls != null && Hulls.Length > 0)
            {
                _primaryHull = System.Linq.Enumerable.FirstOrDefault(Hulls, h => h != null && h.IsPrimary);
            }

            // Call base class to handle all standard initialization
            // SetupHulls will be called from base.StaticInitialise if needed
            // For now, manually setup the hulls
            SetupHulls(Hulls);

            // Base class handles: SlotWrapperController, fog, click handler, name, description, monitors, drone area size
            // We just call the parent implementation which sets those up
            // Actually, since ChainCruiser doesn't have a root SpriteRenderer, we need to handle initialization ourselves

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

            // Initialize all hulls and hook up destruction events
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

        // Event handler for when a hull is destroyed
        private void OnHullDestroyedEvent(object sender, DestroyedEventArgs e)
        {
            // Forward to the virtual method
            OnHullDestroyed(sender as Hull);
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
        /// Override hull destruction to handle secondary hull scoring.
        /// </summary>
        public override void OnHullDestroyed(Hull hull)
        {
            Debug.Log($"[ChainCruiser] Hull destroyed: {hull.HullId}, IsPrimary: {hull.IsPrimary}");

            if (hull.IsPrimary)
            {
                // Primary hull death = game over - call base implementation
                base.OnHullDestroyed(hull);
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


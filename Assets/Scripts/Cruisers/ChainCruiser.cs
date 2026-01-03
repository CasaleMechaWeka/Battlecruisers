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
        [Tooltip("Individual hull sections that can be independently targeted and selected.")]
        public HullSection[] HullSections;

        private HullSection _primaryHull;
        private ITargetConsumer _userChosenTargetConsumer;


        // Override Color to also highlight hull sections
        public override Color Color {
            set {
                // Apply color to root sprite if it exists
                if (_renderer != null) {
                    _renderer.color = value;
                }

                // Apply color to all hull sections
                if (HullSections != null) {
                    foreach (var hullSection in HullSections) {
                        if (hullSection != null) {
                            hullSection.Color = value;
                        }
                    }
                }
            }
        }

        // Hide base Sprite to return primary hull sprite
        public new Sprite Sprite {
            get {
                // Return primary hull sprite, or first available
                if (_primaryHull != null && _primaryHull.SpriteRenderer != null) {
                    return _primaryHull.SpriteRenderer.sprite;
                }

                foreach (var hull in HullSections) {
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

        public new void MakeInvincible()
        {
            if (HullSections != null)
            {
                foreach (var hull in HullSections)
                {
                    hull?.MakeInvincible();
                }
            }
        }

        public new void MakeDamagable()
        {
            if (HullSections != null)
            {
                foreach (var hull in HullSections)
                {
                    hull?.MakeDamageable();
                }
            }
        }

        // Multi-hull specific events
        public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;
        public event EventHandler<HullSectionDestroyedEventArgs> SecondaryHullDestroyed;

        public override void StaticInitialise()
        {
            // Find primary hull BEFORE base.StaticInitialise so maxHealth is set correctly
            if (HullSections != null && HullSections.Length > 0)
            {
                _primaryHull = System.Linq.Enumerable.FirstOrDefault(HullSections, h => h != null && h.IsPrimary);

                if (_primaryHull != null)
                {
                    // Set ChainCruiser's maxHealth to primary hull's maxHealth
                    // This ensures the inherited health bar UI works correctly
                    maxHealth = _primaryHull.maxHealth;
                }
            }

            base.StaticInitialise();

            // Validate
            if (_primaryHull == null)
            {
                Debug.LogError($"[ChainCruiser] {name}: No primary hull section found!");
            }

            Debug.Log($"[ChainCruiser] {name} StaticInitialise - Primary: {_primaryHull?.HullId}, Hulls: {HullSections?.Length}");
        }

        public override async void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            // Set up hull section targeting for ChainCruisers
            _userChosenTargetConsumer = (ITargetConsumer)CruiserSpecificFactories.Targets.UserChosenTargetTracker;

            // Initialize all hull sections
            if (HullSections != null)
            {
                foreach (var hull in HullSections)
                {
                    if (hull != null)
                    {
                        hull.Initialize();
                        hull.Destroyed += OnHullSectionDestroyedEvent;
                    }
                }
            }

            Debug.Log($"[ChainCruiser] {name} Initialise complete");
        }

        private void OnHullSectionDestroyedEvent(object sender, DestroyedEventArgs e)
        {
            // Event handler - actual logic in OnHullSectionDestroyed
        }



        /// <summary>
        /// Called when any hull section is clicked. Routes the click to the main cruiser selection.
        /// </summary>
        public void OnHullSectionClicked(HullSection hullSection)
        {
            // Reuse the inherited click behavior
            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();
            FactoryProvider.Sound.UISoundPlayer.PlaySound(_selectedSound);
            // Clicked event is handled by base class - don't invoke it directly
        }

        /// <summary>
        /// Called when any hull section is double-clicked. Routes to cruiser double-click handler.
        /// </summary>
        public void OnHullSectionDoubleClicked(HullSection hullSection)
        {
            _cruiserDoubleClickHandler?.OnDoubleClick(this);
        }

        /// <summary>
        /// Called when any hull section is triple-clicked. Used for hull-specific targeting.
        /// </summary>
        public void OnHullSectionTripleClicked(HullSection hullSection)
        {
            Logging.Log(Tags.CRUISER, $"Hull section {hullSection.HullId} triple-clicked for targeting");

            // Raise event to notify listeners that a hull section should be targeted
            HullSectionTargeted?.Invoke(this, new HullSectionTargetedEventArgs(hullSection));

            // Also call the cruiser double-click handler for standard targeting behavior
            _cruiserDoubleClickHandler?.OnDoubleClick(this);
        }

        /// <summary>
        /// Called by HullSection when it is destroyed.
        /// </summary>
        public void OnHullSectionDestroyed(HullSection hull)
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
        /// Gets the health tracker for sharing with hull sections.
        /// </summary>
        public IHealthTracker GetHealthTracker()
        {
            return _healthTracker;
        }

    }
}


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

        // Hide base Sprite to check hull sections
        public new Sprite Sprite {
            get {
                // Use root sprite if available
                if (_renderer != null && _renderer.sprite != null) {
                    return _renderer.sprite;
                }

                // Fall back to first hull section sprite
                if (HullSections != null) {
                    foreach (var hullSection in HullSections) {
                        if (hullSection != null && hullSection.SpriteRenderer != null && hullSection.SpriteRenderer.sprite != null) {
                            return hullSection.SpriteRenderer.sprite;
                        }
                    }
                }

                return null;
            }
        }

        // Multi-hull specific events
        public event EventHandler<HullSectionTargetedEventArgs> HullSectionTargeted;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            // Initialize hull sections
            if (HullSections != null)
            {
                Debug.Log($"[ChainCruiser] Initializing {HullSections.Length} hull sections");
                int nullCount = 0;
                foreach (var hullSection in HullSections)
                {
                    if (hullSection == null)
                    {
                        nullCount++;
                        continue;
                    }

                    // Set parent cruiser reference and initialize
                    hullSection.ParentCruiser = this;
                    hullSection.Initialize();
                }

                if (nullCount > 0)
                {
                    Debug.LogWarning($"[ChainCruiser] {name}: {nullCount} null hull section(s) found in HullSections array. They will be ignored.");
                }
            }
        }

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            // Set up hull section targeting for ChainCruisers
            _userChosenTargetConsumer = (ITargetConsumer)CruiserSpecificFactories.Targets.UserChosenTargetTracker;
            HullSectionTargeted += OnHullSectionTargeted;

            // Initialize hull sections
            if (HullSections != null)
            {
                foreach (var hullSection in HullSections)
                {
                    if (hullSection != null)
                    {
                        hullSection.Initialize();
                    }
                }
            }
        }



        /// <summary>
        /// Called when any hull section is clicked. Routes the click to the main cruiser selection.
        /// </summary>
        public void OnHullSectionClicked(HullSection hullSection)
        {
            Logging.Log(Tags.CRUISER, $"Hull section {hullSection.HullId} clicked");

            OnClicked();
        }

        /// <summary>
        /// Called when any hull section is double-clicked. Routes to cruiser double-click handler.
        /// </summary>
        public void OnHullSectionDoubleClicked(HullSection hullSection)
        {
            Logging.Log(Tags.CRUISER, $"Hull section {hullSection.HullId} double-clicked");

            _cruiserDoubleClickHandler.OnDoubleClick(this);
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
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        /// <summary>
        /// Event handler for when a hull section is targeted. Sets it as the user-chosen target.
        /// </summary>
        private void OnHullSectionTargeted(object sender, HullSectionTargetedEventArgs e)
        {
            if (_userChosenTargetConsumer != null)
            {
                // Set the hull section as the highest priority target
                _userChosenTargetConsumer.Target = e.HullSection;
                Debug.Log($"[ChainCruiser] Set hull section {e.HullSection.HullId} as user-chosen target");
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

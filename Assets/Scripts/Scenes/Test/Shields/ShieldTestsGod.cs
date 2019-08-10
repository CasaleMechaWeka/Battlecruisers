using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Shields
{
    public class ShieldTestsGod : TestGodBase 
	{
        protected override void Start()
        {
            base.Start();

            // Setup shield
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            IAudioSource audioSource = new AudioSourceBC(platformAudioSource);
            IPrioritisedSoundPlayer soundPlayer
                = new PrioritisedSoundPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));

            ShieldStats shieldStats = FindObjectOfType<ShieldStats>();
            shieldStats.BoostMultiplier = 1;
            ShieldController shield = FindObjectOfType<ShieldController>();
            shield.StaticInitialise();
			shield.Initialise(Faction.Reds, soundPlayer);


            // Setup turret
            BarrelController turret = FindObjectOfType<BarrelController>();
            turret.StaticInitialise();

            IList<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(shield.Faction, targetTypes);

            IBarrelControllerArgs barrelControllerArgs
                = new Helper()
                    .CreateBarrelControllerArgs(
                    turret,
                    _updaterProvider.PerFrameUpdater,
                    targetFilter);

            turret.Initialise(barrelControllerArgs);
			turret.Target = shield;
		}
	}
}

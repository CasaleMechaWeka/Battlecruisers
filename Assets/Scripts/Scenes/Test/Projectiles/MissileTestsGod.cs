using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test
{
    public class MissileTestsGod : TestGodBase
    {
        private MissileController[] _missiles;

        public AudioClip impactSound;

        protected override List<GameObject> GetGameObjects()
        {
            _missiles = FindObjectsOfType<MissileController>();
            return
                _missiles
                    .Select(missile => missile.gameObject)
                    .ToList();
        }

        protected void SetupMissiles(Helper helper, ITarget target)
        {
            Assert.IsNotNull(impactSound);

            // Setup missiles
            IExactMatchTargetFilter targetFilter = new ExactMatchTargetFilter()
            {
                Target = target
            };

            ProjectileStats missileStats = GetComponent<ProjectileStats>();
            Vector2 initialVelocity = new Vector2(5, 5);
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);
            ITarget parent = Substitute.For<ITarget>();

            foreach (MissileController missile in _missiles)
            {
                missile.Initialise();
                missile.Activate(
                    new TargetProviderActivationArgs<ProjectileStats>(
                        missile.Position,
                        missileStats,
                        initialVelocity,
                        targetFilter,
                        parent,
                        new AudioClipWrapper(impactSound),
                        target));
            }
        }
    }
}

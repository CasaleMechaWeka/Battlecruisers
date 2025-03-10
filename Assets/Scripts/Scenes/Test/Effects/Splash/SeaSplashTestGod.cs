using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Splash
{
    /// <summary>
    /// NOTE:
    /// Bullet and bomb don't create a splash.
    /// 
    /// Bomb creates a splash if I move it to x position 15 :/
    /// 
    /// The problem is with the Game2DWater asset we bought, and I'm sick of debugging it :P
    /// </summary>
    public class SeaSplashTestGod : TestGodBase
    {
        private TestAircraftController _aircraft;
        private ProjectileController[] _projectiles;

        public AudioClip impactSound;

        protected override List<GameObject> GetGameObjects()
        {
            _aircraft = FindObjectOfType<TestAircraftController>();
            _projectiles = FindObjectsOfType<ProjectileController>();

            List<GameObject> gameObjects
                = _projectiles
                    .Select(projectile => projectile.gameObject)
                    .ToList();
            gameObjects.Add(_aircraft.GameObject);
            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
            Assert.IsNotNull(impactSound);

            helper.InitialiseUnit(_aircraft);
            _aircraft.StartConstruction();
            _aircraft.CompletedBuildable += Aircraft_CompletedBuildable;

            IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();
            IProjectileStats projectileStats = Substitute.For<IProjectileStats>();
            projectileStats.GravityScale.Returns(1);
            ITarget parent = Substitute.For<ITarget>();
            ITargetFilter targetFilter = Substitute.For<ITargetFilter>();

            foreach (ProjectileController projectile in _projectiles)
            {
                projectile.Initialise(helper.CommonStrings, factoryProvider);
                projectile.Activate(
                    new ProjectileActivationArgs<IProjectileStats>(
                        projectile.Position,
                        projectileStats,
                        Vector2.zero,
                        targetFilter,
                        parent,
                        new AudioClipWrapper(impactSound)));
            }
        }

        private void Aircraft_CompletedBuildable(object sender, EventArgs e)
        {
            TestAircraftController aircraft = sender as TestAircraftController;
            aircraft.TakeDamage(aircraft.MaxHealth, damageSource: null);
        }
    }
}
using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Factories;
using NSubstitute;
using System;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Effects
{
    /// <summary>
    /// NOTE:
    /// Bullet and bomb don't create a splash.
    /// 
    /// Bomb creates a splash if I move it to x position 15 :/
    /// 
    /// The problem is with the Game2DWater asset we bought, and I'm sick of debugging it :P
    /// </summary>
    public class SeaSplashTestGod : MonoBehaviour
    {
        private void Start()
        {
            TestAircraftController aircraft = FindObjectOfType<TestAircraftController>();
            Helper helper = new Helper();
            helper.InitialiseUnit(aircraft);
            aircraft.StartConstruction();
            aircraft.CompletedBuildable += Aircraft_CompletedBuildable;

            IFactoryProvider factoryProvider = Substitute.For<IFactoryProvider>();
            IProjectileStats projectileStats = Substitute.For<IProjectileStats>();
            projectileStats.GravityScale.Returns(1);
            ITarget parent = Substitute.For<ITarget>();
            ITargetFilter targetFilter = Substitute.For<ITargetFilter>();

            ProjectileController[] projectiles = FindObjectsOfType<ProjectileController>();
            foreach (ProjectileController projectile in projectiles)
            {
                projectile.Initialise(factoryProvider);
                projectile.Activate(
                    new ProjectileActivationArgs<IProjectileStats>(
                        projectile.Position,
                        projectileStats,
                        Vector2.zero,
                        targetFilter,
                        parent));
            }
        }

        private void Aircraft_CompletedBuildable(object sender, EventArgs e)
        {
            TestAircraftController aircraft = sender as TestAircraftController;
            aircraft.TakeDamage(aircraft.MaxHealth, damageSource: null);
        }
    }
}
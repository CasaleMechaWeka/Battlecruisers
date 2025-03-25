using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Projectiles.Spawners.Beams.Lightning;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Projectiles
{
    public class LightningEmitterTestGod : TestGodBase
    {
        public LightningEmitter lightningEmitter;
        public AirFactory target;

        protected override void Setup(Helper helper)
        {
            BCUtils.Helper.AssertIsNotNull(lightningEmitter, target);

            // Setup lightning emitter
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            ITarget parent = Substitute.For<ITarget>();

            lightningEmitter.Initialise(targetFilter, damage: 1, parent);
            InvokeRepeating(nameof(FireLightning), time: 0.5f, repeatRate: 1);

            // Setup target
            helper.InitialiseBuilding(target);
            target.StartConstruction();
        }

        private void FireLightning()
        {
            lightningEmitter.FireBeam(angleInDegrees: 0, isSourceMirrored: false);
        }
    }
}
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Lightning;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPLightningBarrelController : PvPBarrelController
    {
        public PvPLightningEmitter lightningEmitter;

        public override Vector3 ProjectileSpawnerPosition => lightningEmitter.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();
            Assert.IsNotNull(lightningEmitter);
        }

#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected override async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args)
        {
            lightningEmitter
                .Initialise(
                    args.TargetFilter,
                    _projectileStats.Damage,
                    args.Parent,
                   /* args.FactoryProvider.SettingsManager */  null);
        }
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

        public override void Fire(float angleInDegrees)
        {
            lightningEmitter.FireBeam(angleInDegrees, transform.IsMirrored());
        }

        public override void CleanUp()
        {
            base.CleanUp();
            lightningEmitter.DisposeManagedState();
        }
    }
}


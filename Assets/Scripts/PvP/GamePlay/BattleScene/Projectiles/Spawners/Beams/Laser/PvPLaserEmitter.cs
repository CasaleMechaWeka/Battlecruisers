using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserEmitter : PvPBeamEmitter, IPvPLaserEmitter
    {
        private IPvPLaserRenderer _laserRenderer;
        private IPvPLaserSoundPlayer _laserSoundPlayer;
        private float _damagePerS;
        private PvPLaserImpact _laserImpact;
        private IPvPParticleSystemGroup _laserMuzzleEffect;
        private IPvPDeltaTimeProvider _deltaTimeProvider;

        private IPvPSettableBroadcastingProperty<bool> _isLaserFiring;
        public IPvPBroadcastingProperty<bool> IsLaserFiring { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new PvPLaserRenderer(lineRenderer);

            _laserImpact = GetComponentInChildren<PvPLaserImpact>();
            Assert.IsNotNull(_laserImpact);

            IPvPParticleSystemGroupInitialiser laserMuzzleEffectInitialiser = transform.FindNamedComponent<IPvPParticleSystemGroupInitialiser>("LaserMuzzleEffect");
            _laserMuzzleEffect = laserMuzzleEffectInitialiser.CreateParticleSystemGroup();

            _isLaserFiring = new PvPSettableBroadcastingProperty<bool>(false);
            IsLaserFiring = new PvPBroadcastingProperty<bool>(_isLaserFiring);
        }

        public async Task InitialiseAsync(
            IPvPTargetFilter targetFilter,
            float damagePerS,
            IPvPTarget parent,
            ISettingsManager settingsManager,
            IPvPDeltaTimeProvider deltaTimeProvider,
            IPvPDeferrer timeScaleDeferrer)
        {
            base.Initialise(targetFilter, parent, settingsManager);
            Assert.IsNotNull(deltaTimeProvider);
            Assert.IsTrue(damagePerS > 0);

            _damagePerS = damagePerS;
            _deltaTimeProvider = deltaTimeProvider;
            _laserSoundPlayer = new PvPLaserSoundPlayer(_laserRenderer, _audioSource);
            _laserImpact.Initialise(timeScaleDeferrer);
        }

        protected override void HandleCollision(IPvPBeamCollision collision)
        {
            _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);
            _laserImpact.Show(collision.CollisionPoint);
            _laserMuzzleEffect.Play();

            float damage = _deltaTimeProvider.DeltaTime * _damagePerS;
            collision.Target.TakeDamage(damage, _parent);

            _isLaserFiring.Value = true;
        }

        public void StopLaser()
        {
            // Logging.LogMethod(Tags.BEAM);

            _laserRenderer.HideLaser();
            _laserMuzzleEffect.Stop();
            _isLaserFiring.Value = false;
        }

        public override void DisposeManagedState()
        {
            base.DisposeManagedState();
            _laserSoundPlayer?.DisposeManagedState();
        }
    }
}

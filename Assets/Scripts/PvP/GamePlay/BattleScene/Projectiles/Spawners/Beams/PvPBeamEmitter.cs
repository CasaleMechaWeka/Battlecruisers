using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams
{
    public abstract class PvPBeamEmitter : NetworkBehaviour, IPvPBeamEmitter
    {
        private IPvPBeamCollisionDetector _collisionDetector;
        protected IPvPTarget _parent;

        [SerializeField]
        private AudioSource _platformAudioSource;
        protected IPvPAudioSource _audioSource;

        public PvPBroadcastingParticleSystem constantSparks;
        public LayerMask unitsLayerMask, shieldsLayerMask;

        protected virtual void Awake()
        {
            Assert.IsNotNull(constantSparks);
            constantSparks.Initialise();

            Assert.IsNotNull(_platformAudioSource);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
                _audioSource
                    = new PvPEffectVolumeAudioSource(
                        new PvPAudioSourceBC(_platformAudioSource),
                        PvPBattleSceneGodClient.Instance.dataProvider.SettingsManager);
        }

        protected void Initialise(IPvPTargetFilter targetFilter, IPvPTarget parent, ISettingsManager settingsManager)
        {
            // Logging.Verbose(Tags.BEAM, $"parent: {parent}  unitsLayerMask: {unitsLayerMask.value}  shieldsLayerMask: {shieldsLayerMask.value}");
            PvPHelper.AssertIsNotNull(targetFilter, parent, settingsManager);

            _parent = parent;
            /*            _audioSource
                            = new PvPEffectVolumeAudioSource(
                                new PvPAudioSourceBC(_platformAudioSource),
                                settingsManager);*/

            ContactFilter2D contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
            };
            _collisionDetector = new PvPBeamCollisionDetector(contactFilter, targetFilter);

            constantSparks.Play();
            PlaySparks_PvP();
        }

        protected virtual void PlaySparks_PvP()
        {
            throw new NotImplementedException();
        }

        protected virtual void StopSparks_PvP()
        {
            throw new NotImplementedException();
        }

        public void FireBeam(float angleInDegrees, bool isSourceMirrored)
        {
            // Logging.LogMethod(Tags.BEAM);

            IPvPBeamCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);
            if (collision == null)
            {
                // Logging.Warn(Tags.BEAM, "Beam should only be fired if there is a target in our sights, so should always get a collision :/");
                return;
            }

            // Logging.Log(Tags.BEAM, $"Have a collision with: {collision.Target} at {collision.CollisionPoint}");
            HandleCollision(collision);
        }

        protected abstract void HandleCollision(IPvPBeamCollision collision);

        public virtual void DisposeManagedState()
        {
            constantSparks.Stop();
            StopSparks_PvP();
        }
    }
}

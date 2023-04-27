using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools
{
    public class PvPAudioSourcePoolable : IPvPAudioSourcePoolable
    {
        private readonly IPvPAudioSource _source;
        private readonly IPvPDeferrer _realTimeDeferrer;

        public event EventHandler Deactivated;

        public PvPAudioSourcePoolable(IPvPAudioSource source, IPvPDeferrer realTimeDeferrer)
        {
            PvPHelper.AssertIsNotNull(source, realTimeDeferrer);

            _source = source;
            _realTimeDeferrer = realTimeDeferrer;

            // Do not activate until we are ready to play
            _source.IsActive = false;
        }

        public void Activate(PvPAudioSourceActivationArgs activationArgs)
        {
            Assert.IsNotNull(activationArgs);

            _source.IsActive = true;
            _source.AudioClip = activationArgs.Sound;
            _source.Position = activationArgs.Position;
            _source.Play();

            _realTimeDeferrer.Defer(CleanUp, activationArgs.Sound.Length);
        }

        public void Activate(PvPAudioSourceActivationArgs activationArgs, PvPFaction faction)
        {
        }

        private void CleanUp()
        {
            _source.IsActive = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}
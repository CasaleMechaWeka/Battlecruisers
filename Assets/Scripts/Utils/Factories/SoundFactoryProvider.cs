using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Factories
{
    public class SoundFactoryProvider : ISoundFactoryProvider
    {
        public ISoundFetcher SoundFetcher { get; private set; }
        public ISoundManager SoundManager { get; private set; }
        public ISoundPlayerFactory SoundPlayerFactory { get; private set; }

        public SoundFactoryProvider(IVariableDelayDeferrer deferrer)
		{
            Assert.IsNotNull(deferrer);

            SoundFetcher = new SoundFetcher();
            SoundManager = new SoundManager(SoundFetcher, new SoundPlayer());
            SoundPlayerFactory = new SoundPlayerFactory(SoundFetcher, deferrer);
        }
	}
}

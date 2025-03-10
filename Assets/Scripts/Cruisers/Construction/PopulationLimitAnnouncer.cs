using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.Timers;
using System;

namespace BattleCruisers.Cruisers.Construction
{
    public class PopulationLimitAnnouncer
    {
        private readonly IPopulationLimitMonitor _populationLimitMonitor;
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _debouncer;
        private readonly IGameObject _popLimitReachedFeedback;

        public PopulationLimitAnnouncer(
            IPopulationLimitMonitor populationLimitMonitor,
            IPrioritisedSoundPlayer soundPlayer, 
            IDebouncer debouncer, 
            IGameObject popLimitReachedFeedback)
        {
            Helper.AssertIsNotNull(populationLimitMonitor, soundPlayer, debouncer, popLimitReachedFeedback);

            _populationLimitMonitor = populationLimitMonitor;
            _soundPlayer = soundPlayer;
            _debouncer = debouncer;
            _popLimitReachedFeedback = popLimitReachedFeedback;

            _populationLimitMonitor.IsPopulationLimitReached.ValueChanged += IsPopulationLimitReached_ValueChanged;
        }

        private void IsPopulationLimitReached_ValueChanged(object sender, EventArgs e)
        {
            if (_populationLimitMonitor.IsPopulationLimitReached.Value)
            {
                _debouncer.Debounce(() => _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached));
            }

            _popLimitReachedFeedback.IsVisible = _populationLimitMonitor.IsPopulationLimitReached.Value;
        }
    }
}
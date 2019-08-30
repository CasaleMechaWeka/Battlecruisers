using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using System;

namespace BattleCruisers.Cruisers.Construction
{
    public class PopulationLimitAnnouncer
    {
        private readonly IPrioritisedSoundPlayer _soundPlayer;
        private readonly IDebouncer _debouncer;

        public PopulationLimitAnnouncer(IPrioritisedSoundPlayer soundPlayer, IDebouncer debouncer, IPopulationLimitMonitor populationLimitMonitor)
        {
            Helper.AssertIsNotNull(soundPlayer, debouncer, populationLimitMonitor);

            _soundPlayer = soundPlayer;
            _debouncer = debouncer;

            populationLimitMonitor.PopulationLimitReached += PopulationLimitMonitor_PopulationLimitReached;
        }

        private void PopulationLimitMonitor_PopulationLimitReached(object sender, EventArgs e)
        {
            _debouncer.Debounce(() => _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.PopulationLimitReached));
        }
    }
}
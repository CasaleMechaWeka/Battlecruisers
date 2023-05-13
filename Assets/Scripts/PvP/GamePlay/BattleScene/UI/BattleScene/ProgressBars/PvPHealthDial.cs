using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthDial<TDamagable> : IPvPHealthDial<TDamagable> where TDamagable : IPvPDamagable
    {
        private readonly IPvPFillableImage _healthDialImage;
        private readonly IPvPFilter<TDamagable> _visibilityFilter;
        private PvPDamageTakenIndicator _damageTakenIndicator;

        private float previousHealthProportion;

        private TDamagable _damagable;
        public TDamagable Damagable
        {
            set
            {
                if (_damagable != null)
                {
                    _damagable.HealthChanged -= _damagable_HealthChanged;
                    _healthDialImage.IsVisible = false;
                }

                _damagable = value;

                if (_damagable != null
                    && _visibilityFilter.IsMatch(_damagable))
                {
                    _healthDialImage.IsVisible = true;
                    UpdateDial();
                    _damagable.HealthChanged += _damagable_HealthChanged;
                }
            }
        }

        public PvPHealthDial(IPvPFillableImage healthDialImage, IPvPFilter<TDamagable> visibilityFilter, PvPDamageTakenIndicator damageTakenIndicator)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;

            _damageTakenIndicator = damageTakenIndicator;
            _damageTakenIndicator.initialise();
            previousHealthProportion = 1.0f;
        }

        public PvPHealthDial(IPvPFillableImage healthDialImage, IPvPFilter<TDamagable> visibilityFilter)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;
        }

        private void _damagable_HealthChanged(object sender, EventArgs e)
        {
            UpdateDial();
        }

        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.Health / _damagable.MaxHealth;
            _healthDialImage.FillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.updateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }
    }
}
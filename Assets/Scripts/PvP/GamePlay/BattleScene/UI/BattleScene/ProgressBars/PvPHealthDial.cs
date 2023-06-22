using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public class PvPHealthDial : IPvPHealthDial
    {
        private readonly IPvPFillableImage _healthDialImage;
        private readonly IPvPFilter<PvPTarget> _visibilityFilter;
        private PvPDamageTakenIndicator _damageTakenIndicator;

        private float previousHealthProportion;

        private PvPTarget _damagable;
        public PvPTarget Damagable
        {
            set
            {
                if (_damagable != null)
                {
                    _damagable.pvp_Health.OnValueChanged -= _damagable_HealthChanged;
                    _healthDialImage.IsVisible = false;         
                  
                }

                _damagable = value;

                if (_damagable != null
                    && _visibilityFilter.IsMatch(_damagable))
                {
                    _healthDialImage.IsVisible = true;
                    UpdateDial();
                    _damagable.pvp_Health.OnValueChanged += _damagable_HealthChanged;
                }
            }
        }

        public PvPHealthDial(IPvPFillableImage healthDialImage, IPvPFilter<PvPTarget> visibilityFilter, PvPDamageTakenIndicator damageTakenIndicator)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;

            _damageTakenIndicator = damageTakenIndicator;
            _damageTakenIndicator.initialise();
            previousHealthProportion = 1.0f;
        }

        public PvPHealthDial(IPvPFillableImage healthDialImage, IPvPFilter<PvPTarget> visibilityFilter)
        {
            PvPHelper.AssertIsNotNull(healthDialImage, visibilityFilter);

            _healthDialImage = healthDialImage;
            _visibilityFilter = visibilityFilter;

            _healthDialImage.IsVisible = false;
        }

        private void _damagable_HealthChanged(float oldVal, float newVal)
        {
            UpdateDial(newVal);
        }

        private void UpdateDial(float curHealth)
        {
            float proportionOfMaxHealth = curHealth / _damagable.MaxHealth;
            _healthDialImage.FillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.updateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }


        private void UpdateDial()
        {
            float proportionOfMaxHealth = _damagable.pvp_Health.Value / _damagable.MaxHealth;
            _healthDialImage.FillAmount = proportionOfMaxHealth;
            if (proportionOfMaxHealth < previousHealthProportion && _damageTakenIndicator != null)
            {
                _damageTakenIndicator.updateDamageTakenIndicator();
            }
            previousHealthProportion = proportionOfMaxHealth;
        }
    }
}
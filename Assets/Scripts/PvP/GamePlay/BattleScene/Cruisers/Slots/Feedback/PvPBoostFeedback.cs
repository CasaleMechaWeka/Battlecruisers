using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.Feedback
{
    public class PvPBoostFeedback : IPvPBoostFeedback
    {
        private readonly IGameObject _singleBoostEffect, _doubleBoostEffect;

        private PvPBoostState _state;
        public PvPBoostState State
        {
            get => _state;
            set
            {
                _state = value;

                _singleBoostEffect.IsVisible = _state == PvPBoostState.Single;
                _doubleBoostEffect.IsVisible = _state == PvPBoostState.Double;
            }
        }

        public PvPBoostFeedback(IGameObject singleBoostEffect, IGameObject doubleBoostEffect)
        {
            Helper.AssertIsNotNull(singleBoostEffect, doubleBoostEffect);

            _singleBoostEffect = singleBoostEffect;
            _doubleBoostEffect = doubleBoostEffect;

            State = PvPBoostState.Off;
        }
    }
}
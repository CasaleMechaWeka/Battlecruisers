using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Cruisers.Slots.Feedback
{
    public class BoostFeedback : IBoostFeedback
    {
        private readonly IGameObject _singleBoostEffect, _doubleBoostEffect;

        private BoostState _state;
        public BoostState State
        {
            get => _state;
            set
            {
                _state = value;

                _singleBoostEffect.IsVisible = _state == BoostState.Single;
                _doubleBoostEffect.IsVisible = _state == BoostState.Double;
            }
        }

        public BoostFeedback(IGameObject singleBoostEffect, IGameObject doubleBoostEffect)
        {
            Helper.AssertIsNotNull(singleBoostEffect, doubleBoostEffect);

            _singleBoostEffect = singleBoostEffect;
            _doubleBoostEffect = doubleBoostEffect;

            State = BoostState.Off;
        }
    }
}
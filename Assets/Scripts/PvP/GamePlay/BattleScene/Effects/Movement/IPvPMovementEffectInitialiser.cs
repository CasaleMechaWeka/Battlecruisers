using BattleCruisers.Effects.Movement;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Movement
{
    public interface IPvPMovementEffectInitialiser
    {
        IMovementEffect CreateMovementEffects();
    }
}
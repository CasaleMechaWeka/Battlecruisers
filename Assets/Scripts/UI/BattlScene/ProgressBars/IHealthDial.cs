using BattleCruisers.Buildables;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public interface IHealthDial<TDamagable> where TDamagable : IDamagable
    {
        TDamagable Damagable { set; }
    }
}
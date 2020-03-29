namespace BattleCruisers.Effects.Trails
{
    /// <summary>
    /// A projectile trail can consist of two types of effects:
    /// 1. Only show while the projectile is alive, pre-impact.  Eg: glow
    /// 2. Shown both while the projectile is alive and after the projectile
    ///     dies, post-impact.  Eg: trail smoke
    /// </summary>
    public interface IProjectileTrail
    {
        void ShowAllEffects();
        void HideAliveEffects();
        void HideAllEffects();
    }
}
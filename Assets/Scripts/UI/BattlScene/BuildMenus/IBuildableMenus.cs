namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public interface IBuildableMenus<TCategories>
    {
        IBuildablesMenu GetBuildablesMenu(TCategories buildableCategory);
    }
}
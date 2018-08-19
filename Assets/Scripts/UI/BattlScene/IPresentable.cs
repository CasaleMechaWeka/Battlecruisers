namespace BattleCruisers.UI.BattleScene
{
    public interface IPresentable
    {
        // About to be shown
        // FELIX  I don't like the lack of type saftery of activationParameter :/  
        // => Create generic interface that takes activationParameter type?  
        // (Only used by UnitsMenuController?)
        void OnPresenting(object activationParameter);

        // About to be hidden
        void OnDismissing();
    }
}

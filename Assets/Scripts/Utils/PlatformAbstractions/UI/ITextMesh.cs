namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface ITextMesh
    {
        string Text { get; set; }
        void SetActive(bool isActive);
    }
}

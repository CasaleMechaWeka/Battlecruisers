namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class DummyTextMesh : ITextMesh
    {
        public string Text { get; set; }

        public void SetActive(bool isActive)
        {
            // Emtpy
        }
    }
}

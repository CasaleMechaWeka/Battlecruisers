using BattleCruisers.Utils.Localisation;

namespace BattleCruisers
{
    public interface IPrefab
    {
        void StaticInitialise(ILocTable commonStrings);
    }
}
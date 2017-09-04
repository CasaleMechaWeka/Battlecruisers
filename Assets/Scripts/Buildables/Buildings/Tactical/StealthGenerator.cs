namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class StealthGenerator : Building, IStealthGenerator
    {
        // Empty.  Solely exists so that classes like FogOfWarManager and
        // stealth threat responder can keep track of when this building is built.
    }
}

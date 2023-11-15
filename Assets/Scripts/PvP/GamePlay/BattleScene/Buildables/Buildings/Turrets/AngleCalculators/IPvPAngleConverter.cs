namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IPvPAngleConverter
    {
        /// <summary>
        /// Convert from (0*, 360*) to (-180*, 180*).
        /// </summary>
        /// <param name="unsignedAngleInDegrees">0* to 360*</param>
        /// <returns>-180* to 180*</returns>
        float ConvertToSigned(float unsignedAngleInDegrees);

        /// <summary>
        /// Convert from (-180*, 180*) to (0*, 360*).
        /// </summary>/// <param name="signedAngleInDegrees">-180* to 180*</param>
        /// <returns>0* to 360*</returns>
        float ConvertToUnsigned(float signedAngleInDegrees);
    }
}
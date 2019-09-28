namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleConverter
    {
        /// <param name="unsignedAngleInDegrees">0* to 360*</param>
        /// <returns>-180* to 180*</returns>
        float ConvertToSigned(float unsignedAngleInDegrees);

        /// <param name="signedAngleInDegrees">-180* to 180*</param>
        /// <returns>0* to 360*</returns>
        float ConvertToUnsigned(float signedAngleInDegrees);
    }
}
namespace BattleCruisers.AI.FactoryManagers
{
    public class BufferUnitFilter : IUnitFilter
	{
        private readonly int _droneBuffer;

        public BufferUnitFilter(int droneBuffer)
        {
            _droneBuffer = droneBuffer;
        }

		public bool IsBuildableAcceptable(int buildableDroneNum, int droneManagerDroneNum)
        {
            return buildableDroneNum + _droneBuffer <= droneManagerDroneNum;
        }
	}
}

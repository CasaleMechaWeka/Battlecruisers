using System;

public static class Utils
{
	public const int MIN_DRONE_NUM = 2;
	public const int MAX_DRONE_NUM = 20;

	private const int T2_THRESHOLD = 6;
	private const int T3_THRESHOLD = 12;

	public static TechLevel DroneNumberToTechLevel(int numOfDrones)
	{
		if (numOfDrones < MIN_DRONE_NUM || numOfDrones > MAX_DRONE_NUM)
		{
			throw new ArgumentException();
		}

		if (numOfDrones < T2_THRESHOLD)
		{
			return TechLevel.T1;
		}
		if (numOfDrones < T3_THRESHOLD)
		{
			return TechLevel.T2;
		}
		return TechLevel.T3;
	}
}
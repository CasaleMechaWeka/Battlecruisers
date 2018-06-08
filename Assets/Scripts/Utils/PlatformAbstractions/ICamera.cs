namespace BattleCruisers.Utils.PlatformAbstractions
{
	public interface ICamera : ITransform
	{
		float OrthographicSize { get; set; }
        float Aspect { get; }
	}
}
